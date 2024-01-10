'use strict';

const dotenv = require('dotenv').config();
const { sharedSchema } = require('../sharedSchema');
const { axiosService } = require('../services/axios.service');

module.exports = async function (fastify, opts) {
  const jsonSchema = {
    schema: {
      description: 'Get schedule days for provider',
      tags: [],
      summary: 'Epic Private GetScheduleDaysForProvider2',
      requestBody: {
        type: 'object',
        properties: {
          StartDate: {
            type: 'string',
            description: 'Start date'
          },
          EndDate: {
            type: 'string',
            description: 'End date',
          },
          ProviderID: {
            type: 'string',
            description: 'Provider identifier',
          },
          ProviderIDType: {
            type: 'string',
            description: 'Provider identifier type',
          },
          DepartmentIDs: {
            type: 'array',
            description: 'Department IDs',
          },
          VisitTypeIDs: {
            type: 'array',
            description: 'Visit type IDs',
          },
        },
        anyOf: [
          { required: ['StartDate'] },
          { required: ['EndDate'] },
          { required: ['ProviderID'] },
          { required: ['ProviderIDType'] },
          { required: ['DepartmentIDs'] },
          { required: ['VisitTypeIDs'] },
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
                Departments: { type: 'array' },
                Provider: {
                  type: 'object',
                  properties: {
                    DisplayName: { type: 'string' },
                    IDs: { type: 'array' },
                  }
                },
                VisitTypes: { type: 'array' },
                ScheduleDays: { type: 'array' },
              }
            }
          }
        },
      }
    }
  };

  fastify.post('/GetScheduleDaysForProvider', jsonSchema, async function (request, reply) {
    try {
      // Read incoming data
      const {
        StartDate,
        EndDate,
        ProviderID,
        ProviderIDType,
        DepartmentIDs,
        VisitTypeIDs,
      } = request.body;
  
      const result = await axiosService.post('/epic/2017/PatientAccess/External/GetScheduleDaysForProvider2/Scheduling/Open/Provider/GetScheduleDays2', {
        StartDate,
        EndDate,
        ProviderID,
        ProviderIDType,
        DepartmentIDs,
        VisitTypeIDs,
      });
  
      // console.log(result?.data);
  
      if (!result?.data) {
        reply = {
          status: 404,
          body: {
            success: false,
            message: 'No data found for that provider.'
          },
        };
        return reply;
      }
  
      // Add or change code here
      const message = result?.data;
  
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
