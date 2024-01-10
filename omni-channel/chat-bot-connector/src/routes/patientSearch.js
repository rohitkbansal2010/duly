'use strict';

const dotenv = require('dotenv').config();
const Axios = require('axios');
const FormData = require('form-data');
const { DefaultAzureCredential } = require("@azure/identity");
const { CertificateClient } = require("@azure/keyvault-certificates");
const { SecretClient } = require("@azure/keyvault-secrets");
const https = require('https');

module.exports = async function (fastify, opts) {
  const ADDRESS_LINE_SEPARATOR = ' ';

  const getName = (patient) => {
    return patient?.name?.filter((entry) => entry.use === 'usual')[0]?.text || 'None';
  };

  const getFhirId = (patient) => {
    return patient?.identifier?.filter((entry) => entry?.type?.text === 'FHIR')[0]?.value || 'None';
  };

  const getMRN = (patient) => {
    return patient?.identifier?.filter((entry) => entry?.type?.text === 'MRN')[0]?.value || 'None';
  };

  const getAddress = (patient) => {
    let homeAddr = patient?.address?.filter((entry) => entry?.use === 'home' && !entry?.period?.end)[0];
    return `${homeAddr.line.join(ADDRESS_LINE_SEPARATOR) || ''}${ADDRESS_LINE_SEPARATOR}${homeAddr?.city || ''} ${homeAddr?.state || ''} ${homeAddr?.postalCode || ''}`;
  };

  const credential = new DefaultAzureCredential();
  const vaultUrl = process.env.KEY_VAULT_URL;
  const certificateName = process.env.CERTIFICATE_NAME;
  const secretClient = new SecretClient(vaultUrl, credential);
  const certificateSecret = await secretClient.getSecret(certificateName);

  if (!certificateSecret) {
    return console.log('Error, failed to retrieve certificate from Azure vault!');
  }

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

  const jsonSchema = {
    schema: {
      description: 'Find a patient in Epic given certain demographic information',
      tags: [],
      summary: 'Epic Patient.Search',
      query: {
        type: 'object',
        properties: {
          given: {
            type: 'string',
            description: 'patient given/first name'
          },
          family: {
            type: 'string',
            description: 'patient family/last name',
          },
          birthdate: {
            type: 'string',
            description: 'patient date of birth',
          },
          telecom: {
            type: 'string',
            description: 'patient phone number',
          },
          mrn: {
            type: 'string',
            description: 'patient medical record number',
          }
        },
        anyOf: [
          { required: ['given'] },
          { required: ['family'] },
          { required: ['birthdate'] },
          { required: ['telecom'] },
          { required: ['mrn'] },
        ]
      },
      response: {
        404: {
          description: 'No patient found',
          type: 'object',
          properties: {
            status: {
              type: 'number',
            },
            body: {
              type: 'object',
              properties: {
                success: { type: 'boolean' },
                message: { type: 'string' },
              }
            }
          }
        },
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
                fhirId: { type: 'string' },
                name: { type: 'string' },
                mrn: { type: 'string' },
                address: { type: 'string' },
              }
            }
          }
        },
        500: {
          description: 'Internal server error',
          type: 'object',
          properties: {
            status: { type: 'number' },
            body: {
              type: 'object',
              properties: {
                success: { type: 'boolean' },
                message: { type: 'string' },
              }
            }
          }
        }
      }
    }
  };

  fastify.get('/PatientSearch', jsonSchema, async function (request, reply) {
    try {
      // Read incoming data
      const {
        given,
        family,
        birthdate,
        telecom,
        mrn,
      } = request.query;
  
      // const token = await getToken();
      // console.log(`Token: ${token}`);

      const axios = Axios.create({
        baseURL: process.env.EPIC_BASEURL,
        timeout: 60000,
        headers: {
          'Authorization': `${process.env.EPIC_TOKEN_TYPE} ${process.env.EPIC_TOKEN}`,
          'Epic-Client-ID': process.env.EPIC_CLIENT_ID,
        },
        httpsAgent: new https.Agent({
          pfx: Buffer.from(certificateSecret.value, 'base64'),
          requestCert: true,
          rejectUnauthorized: false,
        }),
      });
  
      const patientResult = await axios.get('/FHIR/R4/Patient', {
        params: {
            ...(given ? { given } : {}),
            ...(family ? { family } : {}),
            ...(birthdate ? { birthdate } : {}),
            ...(telecom ? { telecom } : {}),
            ...(mrn ? { identifier: `MRN|${mrn}` } : {}),
        },
      });
  
      const firstPatient = patientResult?.data?.total === 0 ? null : patientResult?.data?.entry[0].resource;
  
      if (!firstPatient) {
        reply = {
          status: 404,
          body: {
            success: false,
            message: 'No patients found with the provided criteria.'
          },
        };
        return reply;
      }
  
      // Add or change code here
      const message = {
        fhirId: getFhirId(firstPatient),
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
          message: 'Internal server error',
        },
      };
      return reply;
    }
  });
}
