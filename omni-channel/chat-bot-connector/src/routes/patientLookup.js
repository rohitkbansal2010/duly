'use strict';

const dotenv = require('dotenv').config();
const Axios = require('axios');
const FormData = require('form-data');
const { sharedSchema } = require('../sharedSchema');
const { axiosService } = require('../services/axios.service');

module.exports = async function (fastify, opts) {
  const ADDRESS_LINE_SEPARATOR = ' ';

  const getToken = async () => {
    const formData = new FormData();
    formData.append('client_id', process.env.OAUTH_CLIENT_ID, { header: { 'subscription-key': process.env.OAUTH_SUBSCRIPTION_KEY } });
    formData.append('client_secret', process.env.OAUTH_CLIENT_SECRET);
    formData.append('scope', process.env.OAUTH_SCOPE);
    formData.append('grant_type', process.env.OAUTH_GRANT_TYPE);

    const response = await Axios.post(process.env.OAUTH_URL, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        'subscription-key': process.env.OAUTH_SUBSCRIPTION_KEY,
      },
    });
    return response.data;
  };

  const getName = (patient) => {
    return patient?.Name || 'None';
  };

  const getFhirId = (patient) => {
    return patient?.identifier?.filter((entry) => entry?.type?.text === 'FHIR')[0]?.value || 'None';
  };

  const getEPIId = (patient) => {
    return patient?.IDs.find((identifier) => identifier.Type === 'EPI')?.ID || 'None';
  }

  const getMRN = (patient) => {
    return patient?.identifier?.filter((entry) => entry?.type?.text === 'MRN')[0]?.value || 'None';
  };

  const getAddress = (patient) => {
    let homeAddr = patient?.Addresses?.find((entry) => entry?.Type === 'Permanent');
    return `${homeAddr?.Street || ''}${ADDRESS_LINE_SEPARATOR}${homeAddr?.City || ''} ${homeAddr?.State || ''} ${homeAddr?.Zip || ''}`;
  };

  const jsonSchema = {
    schema: {
      description: 'Find a patient in Epic given certain demographic information',
      tags: [],
      summary: 'Epic Private PatientLookup',
      query: {
        type: 'object',
        properties: {
          Name: {
            type: 'string',
            description: 'patient full name'
          },
          DOB: {
            type: 'string',
            description: 'patient date of birth',
          },
          Gender: {
            type: 'string',
            description: 'patient gender/sex',
          },
          Phone: {
            type: 'string',
            description: 'patient phone number',
          },
          Last4SSN: {
            type: 'string',
            description: 'patient last 4 digits of SSN',
          },
          MRN: {
            type: 'string',
            description: 'patient medical record number',
          },
          GEC: {
            type: 'string',
            description: 'patient GEC chart id',
          },
          EPI: {
            type: 'string',
            description: 'patient EPI id'
          }
        },
        anyOf: [
          { required: ['Name'] },
          { required: ['DOB'] },
          { required: ['Gender'] },
          { required: ['Phone'] },
          { required: ['Last4SSN'] },
          { required: ['MRN'] },
          { required: ['GEC'] },
          { required: ['EPI'] },
        ]
      },
      response: {
        404: sharedSchema.response[404],
        500: sharedSchema.response[500],
        200: {
          description: 'Successful response',
          type: 'object',
          properties: {
            status: {
              type: 'number',
            },
            body: {
              type: 'object',
              properties: {
                success: { type: 'boolean' },
                epiId: { type: 'string' },
                fhirId: { type: 'string' },
                name: { type: 'string' },
                mrn: { type: 'string' },
                address: { type: 'string' },
              }
            }
          }
        },
      }
    }
  };

  fastify.get('/PatientLookup', jsonSchema, async function (request, reply) {
    try {
      // Read incoming data
      const {
        Name,
        DOB,
        Gender,
        Last4SSN,
        Phone,
        MRN,
        GEC,
        EPI
      } = request.query;
  
      const patientResult = await axiosService.post('/epic/2017/EMPI/External/PatientLookup3/Patient/Lookup3', {
        ...(Name ? { Name } : {}),
        ...(DOB ? { DOB } : {}),
        ...(Gender ? { Gender } : {}),
        ...(Last4SSN ? { Last4SSN } : {}),
        ...(Phone ? { Demographics: { Phones: [ { Type: 'Home', Number: Phone }] } } : {}),
        ...(MRN ? { PatientIDType: 'MRN', PatientId: MRN } : {}),
        ...(EPI ? { PatientIDType: 'EPI', PatientId: EPI } : {}),
        ...(GEC ? { PatientIdType: 'GEC CHART ID', PatientId: GEC } : {}), // for testing
        IDTypeMnemonic: 'FHIR',
      });
  
      // console.log(patientResult.data);
      const firstPatient = patientResult?.data;
      // console.log(JSON.stringify(firstPatient, null, 2));
  
      if (!firstPatient?.Name) {
        reply = {
          status: 404,
          body: {
            success: false,
            message: 'No patients found with the provided criteria.'
          },
        };
        return reply;
      }

      // retrieve FHIR ID as it is not consistently returned by the IDTypeMnemonic
      let fhirId = getFhirId(firstPatient);
      if (fhirId === 'None') {
        const PatientID = getEPIId(firstPatient);
        const PatientIDType = 'EPI';
        const result = await axiosService.post('/epic/2015/Common/Patient/GetPatientIdentifiers/Patient/Identifiers', {
            ...(PatientID ? { PatientID } : {}),
            ...(PatientIDType ? { PatientIDType } : {}),
            UserID: process.env.EPIC_USER_ID,
            UserIDType: process.env.EPIC_USER_ID_TYPE,
        });

        fhirId = result?.data?.Identifiers.find((identifier => identifier.IDType === 'FHIR'))?.ID || 'None';
      }
  
      // Add or change code here
      const message = {
        epiId: getEPIId(firstPatient),
        fhirId,
        name: getName(firstPatient),
        mrn: getMRN(firstPatient),
        address: getAddress(firstPatient),
      };
  
      // Construct response
      const responseJSON = {
        "success": true,
        ...message,
      }
  
      reply = {
        status: 200,
        body: responseJSON,
      };

      return reply;
    } catch(err) {
      console.log(err);
      reply.statusCode = 500;
      reply = {
        status: 500,
        body: {
          success: false,
          message: process.env.ENVIRONMENT_NAME === 'dev' ? err.message : 'Internal server error',
        },
      };
      return reply;
    }
  });
}
