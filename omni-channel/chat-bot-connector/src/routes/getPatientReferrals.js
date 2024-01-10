'use strict';

const dotenv = require('dotenv').config();
const { sharedSchema } = require('../sharedSchema');
const { axiosService } = require('../services/axios.service');
const { mockData } = require('../services/mock.service');

const formatEpicDate = (date) => {
  return `${date.getMonth()}/${date.getDate()}/${date.getFullYear()}`;
};

module.exports = async function (fastify, opts) {
  const jsonSchema = {
    schema: {
      description: 'Get referrals for a patient',
      tags: [],
      summary: 'Epic Private GetPatientReferrals',
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
                "Referrals": {
                  "type": "array",
                  "items": {
                    "type": "object",
                    "properties": {
                      "Status": {
                        "type": "string"
                      },
                      "Type": {
                        "type": "string"
                      },
                      "ExternalReferralNumber": {
                        "type": "string"
                      },
                      "AuthorizationStartDate": {
                        "type": "string"
                      },
                      "AuthorizationEndDate": {
                        "type": "string"
                      },
                      "ReferredByProviderName": {
                        "type": "string"
                      },
                      "ReferredToProviderName": {
                        "type": "string"
                      },
                      "ReferredToFacilityName": {
                        "type": "string"
                      },
                      "PCPFirstName": {
                        "type": "string"
                      },
                      "PCPLastName": {
                        "type": "string"
                      },
                      "ReferralID": {
                        "type": "array",
                        "items": {
                          "type": "object",
                          "properties": {
                            "ID": {
                              "type": "string",
                              "enum": [
                                "1148"
                              ]
                            },
                            "Type": {
                              "type": "string"
                            }
                          },
                          "required": [
                            "ID",
                            "Type"
                          ]
                        }
                      },
                      "ReferredByProviderIDs": {
                        "type": "array",
                        "items": {
                          "type": "object",
                          "properties": {
                            "ID": {
                              "type": "string"
                            },
                            "Type": {
                              "type": "string"
                            }
                          },
                          "required": [
                            "ID",
                            "Type"
                          ]
                        }
                      },
                      "ReferredToProviderIDs": {
                        "type": "array",
                        "nullable": true
                      },
                      "ReferredToFacilityIDs": {
                        "type": "array",
                        "nullable": true
                      },
                      "ReferredToDepartmentIDs": {
                        "type": "array",
                        "nullable": true
                      },
                      "PCPIDs": {
                        "type": "array",
                        "nullable": true
                      },
                      "Diagnoses": {
                        "type": "array",
                        "nullable": true
                      },
                      "Procedures": {
                        "type": "array",
                        "nullable": true
                      },
                      "PatientName": {
                        "type": "array",
                        "nullable": true
                      },
                      "PatientAddress": {
                        "type": "array",
                        "nullable": true
                      },
                      "ReferredtoFacilityAddress": {
                        "type": "array",
                        "nullable": true
                      },
                      "ReferredToProviderAddress": {
                        "type": "array",
                        "nullable": true
                      },
                      "PatientIDs": {
                        "type": "array",
                        "nullable": true
                      }
                    },
                  }
                }
              }
            }
          }
        },
      }
    }
  };

  fastify.get('/GetPatientReferrals', jsonSchema, async function (request, reply) {
    try {
      const {
        PatientID,
        PatientIDType,
      } = request.query;

      const result = await axiosService.post(`/epic/2014/Access/Referral/GETPATIENTREFERRALS/GePatientReferrals?StartDate=1/1/2020&SendExtraInformation=0&SendAllReferralsIfPCPIncluded=0&UseCoveragePCP=0`, {
        ...(PatientID ? { PatientID: { ID: PatientID, Type: PatientIDType } } : {}),
        UserID: { ID: process.env.EPIC_USER_ID_2, Type: process.env.EPIC_USER_ID_TYPE_2 },
      });

      reply = { status: 200, body: result?.data };

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
