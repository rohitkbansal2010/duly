// <copyright file="SendSmsAfterVisitedService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    public class SendSmsAfterVisitedService : ISendSmsAfterVisitedService
    {
        private readonly IPatientService _patientService;
        private readonly IConfiguration _configuration;

        public SendSmsAfterVisitedService(IPatientService patientService, IConfiguration configuration)
        {
            _patientService = patientService;
            _configuration = configuration;
        }

        public async Task<SendAfterVisitPdfSms> SendAfterVisitPdfSms(SendSms request)
        {
            var patient = await _patientService.GetPatientByIdAsync(request.PatientId);
            if (patient == null)
            {
                throw new EntityNotFoundException(nameof(patient));
            }

            var requestResendResponse = BuildRequest(patient, request);
            return requestResendResponse;
        }

        private SendAfterVisitPdfSms BuildRequest(
           Patient patientModel,
           SendSms requestModel)
        {
            var sendSMSSetting = Convert.ToBoolean(_configuration.GetSection("SendSMSSettings:IsProd").Value);

            var parameters = new Dictionary<string, string>();
            parameters.Add("patientName", patientModel.GeneralInfo.HumanName.FamilyName);
            parameters.Add("patientDateOfBirth", Convert.ToString(patientModel.BirthDate));
            parameters.Add("appointmentId", requestModel.AppointmentId);
            parameters.Add("afterVisitSummaryPdfId", requestModel.PdfId);

            var addressParams = new Dictionary<string, string>();
            if (sendSMSSetting)
            {
                if (!string.IsNullOrEmpty(requestModel.PhoneNumber))
                {
                    addressParams.Add("to", requestModel.PhoneNumber.Trim());
                }
                else if (patientModel.PhoneNumber != null)
                {
                    var patientMobileNo = patientModel.PhoneNumber.Where(x => x.Use == "Mobile").ToList();
                    if (patientMobileNo.Count > 0)
                    {
                        foreach (var phn in patientMobileNo)
                        {
                            addressParams.Add("to", phn.PhoneNum.Trim());
                        }
                    }
                    else
                    {
                        throw new EntityNotFoundException(nameof(patientModel.PhoneNumber), "Mobile number not found.");
                    }
                }
                else
                {
                    throw new EntityNotFoundException(nameof(patientModel.PhoneNumber));
                }
            }
            else
            {
                addressParams.Add("to", _configuration.GetSection("SendSMSSettings:TestPhoneNumber").Value);
            }

            var request = new SendAfterVisitPdfSms
            {
                ConfigurationToken = "AfterVisitCareplanPDF",
                CorrelationToken = requestModel.AppointmentId,
                Addresses = new List<AddressModelSMS>
                {
                new()
                    {
                    Parameters = addressParams,
                    AddressPointer = "to",
                    CorrelationToken = requestModel.PdfId,
                    TimeZone = null
                    }
                },
                Parameters = parameters
            };
            return request;
        }
    }
}