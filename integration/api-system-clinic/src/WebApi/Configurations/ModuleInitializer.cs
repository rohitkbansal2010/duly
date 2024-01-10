// <copyright file="ModuleInitializer.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Fhir.Adapter.Exceptions;
using Duly.Common.Infrastructure.Components;
using Duly.Common.Security.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;

namespace Duly.Clinic.Api.Configurations
{
    internal static class ModuleInitializer
    {
        public static IServiceCollection AddApiModules(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAllergyIntoleranceRepository, FhirAllergyIntoleranceRepository>();
            services.AddTransient<IConditionRepository, FhirConditionRepository>();
            services.AddTransient<IEncounterRepository, FhirEncounterRepository>();
            services.AddTransient<IPractitionerGeneralInfoRepository, FhirPractitionerGeneralInfoRepository>();
            services.AddTransient<IParticipantRepository, FhirParticipantRepository>();
            services.AddTransient<ICareTeamParticipantRepository, FhirCareTeamParticipantRepository>();
            services.AddTransient<IPatientRepository, FhirPatientRepository>();
            services.AddTransient<IMedicationRepository, FhirMedicationRepository>();
            services.AddTransient<IObservationRepository, FhirObservationRepository>();
            services.AddTransient<IDiagnosticReportRepository, FhirDiagnosticReportRepository>();
            services.AddTransient<IImmunizationRepository, FhirImmunizationRepository>();
            services.AddTransient<IServiceRequestRepository, FhirServiceRequestRepository>();

            services.AddTransient<IEnrichBMI, BMIEnricher>();
            services.AddTransient<IObservationEnricher, ObservationEnricher>();

            services.AddTransient<IScheduleRepository, ScheduleRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();

            return services;
        }

        public static IServiceCollection AddApiMappings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(ClientToSystemApiContractsProfile));

            return services;
        }

        public static void ExtendErrorHandlingOptions(ErrorHandlingOptions errorHandlingOptions)
        {
            errorHandlingOptions.ResultStatusCodesAndMessages.Add(typeof(ObfuscatorException), ((int)HttpStatusCode.BadRequest, null));
            errorHandlingOptions.ResolveStatusCode = ResolveStatusCode;
        }

        private static int ResolveStatusCode(Exception exception, IDictionary<Type, (int, string)> valueTuples)
        {
            var result = ExceptionHandler.ExtendHandling(exception) ?? Wipfli.Adapter.Exceptions.ExceptionHandler.ExtendHandling(exception);

            return ExcludeOkIfError(result);
        }

        private static int ExcludeOkIfError(int? result)
        {
            var nonNullableResult = result ?? default;
            return nonNullableResult == 200 ? default : nonNullableResult;
        }
    }
}
