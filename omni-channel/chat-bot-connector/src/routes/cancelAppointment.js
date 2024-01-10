'use strict';

const dotenv = require('dotenv').config();
const { sharedSchema } = require('../sharedSchema');
const { axiosService } = require('../services/axios.service');
const { mockData } = require('../services/mock.service');

module.exports = async function (fastify, opts) {
  const jsonSchema = {
    schema: {
      description: 'Cancel Appointment',
      tags: [],
      summary: 'Epic Private CancelAppointment',
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
          ContactID: {
            type: 'string',
            description: 'contact id for appointment to be cancelled',
          },
          ContactIDType: {
            type: 'string',
            description: 'contact identifier type'
          },
        },
        allOf: [
          { required: ['PatientID'] },
          { required: ['PatientIDType'] },
          { required: ['ContactID'] },
          { required: ['ContactIDType'] },
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
          }
        },
      }
    }
  };

  fastify.get('/CancelAppointment', jsonSchema, async function (request, reply) {
    try {
      const {
        PatientID,
        PatientIDType,
        ContactID,
        ContactIDType,
      } = request.query;
  
      if (process.env.MOCK_RESPONSES) {
        reply = mockData.cancelAppointment;
        return reply;
      }

      const result = await axiosService.post('/epic/2019/PatientAccess/External/CancelAppointment/Epic/Patient/Scheduling2019/CancelAppointment', {
        ...(PatientID ? { PatientID } : {}),
        ...(PatientIDType ? { PatientIDType } : {}),
        UserID: process.env.EPIC_USER_ID,
        UserIDType: process.env.EPIC_USER_ID_TYPE,
      });
  
      // console.log(result?.data);
  
      if (!result?.data) {
        reply = {
          status: 404,
          body: {
            success: false,
            message: 'Appointment not found.'
          },
        };
        return reply;
      }
  
      // Add or change code here
      const message = {
        identifiers: result?.data?.Identifiers,
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
