'use strict';

const { sharedSchema } = require('../sharedSchema');
const { axiosService } = require('../services/axios.service');

module.exports = async function (fastify, opts) {
  const jsonSchema = {
    schema: {
      description: 'Schedule appointment',
      tags: [],
      summary: 'Epic Private ScheduleAppointmentWithInsurance',
      query: {
        type: 'object',
        properties: {
          DepartmentID: {
            type: 'string',
            description: 'Department identifier'
          },
          DepartmentIDType: {
            type: 'string',
            description: 'Department identifier type',
          },
          VisitTypeID: {
            type: 'string',
            description: 'Visit type identifier',
          },
          VisitTypeIDType: {
            type: 'string',
            description: 'Visit type identifier type',
          },
          Date: {
            type: 'string',
            description: 'Date of appointment',
          },
          Time: {
            type: 'string',
            description: 'Start time of appointment',
          },
          ProviderID: {
            type: 'string',
            description: 'Provider identifier',
          },
          ProviderIDType: {
            type: 'string',
            description: 'Provider identifier type',
          },
          isReviewOnly: {
            type: 'boolean',
            description: 'Must to be set to false to actually book the appointment',
          }
        },
        allOf: [
          { required: ['DepartmentID'] },
          { required: ['DepartmentIDType'] },
          { required: ['VisitTypeID'] },
          { required: ['VisitTypeIDType'] },
          { required: ['Date'] },
          { required: ['Time'] },
          { required: ['ProviderID'] },
          { required: ['ProviderIDType'] },
          { required: ['isReviewOnly'] },
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

  fastify.post('/ScheduleAppointment', jsonSchema, async function (request, reply) {
    try {
      // Read incoming data
      const {
        StartDate,
        EndDate,
        ProviderID,
        ProviderIDType,
        DepartmentIDs,
        VisitTypeIDs,
      } = request.query;
  
      const result = await axiosService.post('/epic/2018/PatientAccess/External/ScheduleAppointmentWithInsurance/Scheduling2018/Open/ScheduleWithInsurance', {
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
