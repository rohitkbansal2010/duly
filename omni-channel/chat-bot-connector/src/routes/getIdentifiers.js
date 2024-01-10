'use strict';

const dotenv = require('dotenv').config();
const { sharedSchema } = require('../sharedSchema');
const { axiosService } = require('../services/axios.service');

module.exports = async function (fastify, opts) {
  const jsonSchema = {
    schema: {
      description: 'Get patient identifiers',
      tags: [],
      summary: 'Epic Private GetPatientIdentifiers',
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
                identifiers: { type: 'array' },
              }
            }
          }
        },
      }
    }
  };

  fastify.get('/GetIdentifiers', jsonSchema, async function (request, reply) {
    try {
      // Read incoming data
      const {
        PatientID,
        PatientIDType,
      } = request.query;
  
      const result = await axiosService.post('/epic/2015/Common/Patient/GetPatientIdentifiers/Patient/Identifiers', {
        ...(PatientID ? { PatientID } : {}),
        ...(PatientIDType ? { PatientIDType } : {}),
        UserID: process.env.EPIC_USER_ID,
        UserIDType: process.env.EPIC_USER_ID_TYPE,
      });
  
      // console.log(result?.data?.Identifiers);
  
      if (!result?.data?.Identifiers?.length) {
        reply = {
          status: 404,
          body: {
            success: false,
            message: 'No identifiers found for that patient.'
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
