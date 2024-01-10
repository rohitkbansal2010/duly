'use strict';

const dotenv = require('dotenv').config();
const { sharedSchema } = require('../sharedSchema');
const { axiosService } = require('../services/axios.service');
const { mockData } = require('../services/mock.service');

const formatAppts = (appts, patId) => {
  return appts.map((appt) => ({
    'Type': appt.VisitType,
    'Provider': appt.ProviderDepartments[0]?.Provider.Name,
    'ProviderID': appt.ProviderDepartments[0]?.Provider.IDs.find((id) => id.Type === 'NPISER')?.ID,
    'Department': appt.ProviderDepartments[0]?.Department.Name,
    'DepartmentID': appt.ProviderDepartments[0]?.Department.IDs.find((id) => id.Type === 'Internal')?.ID,
    'Date': appt.Date,
    'Time': appt.Time,
    'ContactID': appt.ContactIDs?.find((id) => id.Type === 'DAT')?.ID,
    'PatientID': patId,
  }))
};

module.exports = async function (fastify, opts) {
  const jsonSchema = {
    schema: {
      description: 'Get future appointments',
      tags: [],
      summary: 'Epic Private GetFutureAppointments',
      query: {
        type: 'object',
        properties: {
          PatientID: {
            type: 'string',
            description: 'patient identifier'
          },
          PatientIDType: {
            type: 'string',
            description: 'patient identifier type',
          },
        },
        allOf: [
          { required: ['PatientID'] },
          { required: ['PatientIDType'] },
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
                appointments: { type: 'array' },
              }
            }
          }
        },
      }
    }
  };

  fastify.get('/GetAppointments', jsonSchema, async function (request, reply) {
    try {
      const {
        PatientID,
        PatientIDType,
      } = request.query;
  
      // if (process.env.MOCK_RESPONSES) {
      //   reply = mockData.getAppointments;
      //   return reply;
      // }

      const result = await axiosService.post('/epic/2014/PatientAccess/External/GetFutureAppointments/Scheduling/Future', {
        Patient: {
          ID: PatientID,
          Type: PatientIDType == 'GEC' ? 'GEC CHART ID' : PatientIDType,
        }
      });
  
      // console.log(result?.data);
  
      if (!result?.data) {
        reply = {
          status: 404,
          body: {
            success: false,
            message: 'No future appointments found for that patient.'
          },
        };
        return reply;
      }
  
      // Add or change code here
      const message = {
        appointments: formatAppts(result?.data?.Appointments, PatientID),
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
