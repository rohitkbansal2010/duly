// <copyright file="ModuleInitializer.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contexts.Implementations;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations.Dashboard;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.Dashboard;
using Duly.CollaborationView.Encounter.Api.Repositories.Mappings;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.Dashboard;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.Dashboard;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.Common.DataAccess.Extensions;
using Duly.Common.Infrastructure.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    public static class ModuleInitializer
    {
        public static IServiceCollection AddApiModules(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContexts(configuration);

            // Remove after testing
            services.AddTransient<IEncounterContext, EncounterContext>();

            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IPatientRepository, PatientRepository>();

            services.AddTransient<IPractitionerService, PractitionerService>();
            services.AddTransient<IPractitionerRepository, PractitionerRepository>();

            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();

            services.AddTransient<IPartyService, PartyService>();
            services.AddTransient<IEncounterParticipantRepository, EncounterParticipantRepository>();
            services.AddTransient<ICareTeamRepository, CareTeamRepository>();

            services.AddTransient<ISiteService, SiteService>();
            services.AddTransient<ISitesRepository, SitesRepository>();

            services.AddTransient<IMedicationService, MedicationService>();
            services.AddTransient<IMedicationRepository, MedicationRepository>();

            services.AddTransient<IObservationRepository, ObservationRepository>();
            services.AddTransient<IVitalService, VitalService>();

            services.AddTransient<IAllergyService, AllergyService>();
            services.AddTransient<IAllergyIntoleranceRepository, AllergyIntoleranceRepository>();

            services.AddTransient<Services.Interfaces.IConditionService, Services.Implementations.ConditionService>();
            services.AddTransient<Repositories.Interfaces.IConditionRepository, Repositories.Implementations.ConditionRepository>();

            services.AddTransient<ITestReportService, TestReportService>();
            services.AddTransient<IDiagnosticReportRepository, DiagnosticReportRepository>();

            services.AddTransient<IPastImmunizationRepository, PastImmunizationRepository>();
            services.AddTransient<IRecommendedImmunizationRepository, RecommendedImmunizationRepository>();
            services.AddTransient<IImmunizationService, ImmunizationService>();

            services.AddTransient<ILabDetailRepository, LabDetailRepository>();
            services.AddTransient<ILabDetailService, LabDetailService>();

            services.AddTransient<IImagingDetailRepository, ImagingDetailRepository>();
            services.AddTransient<IImagingDetailService, ImagingDetailService>();

            services.AddTransient<IScheduleFollowupService, ScheduleFollowupService>();
            services.AddTransient<IScheduleFollowupRepository, ScheduleFollowupRepository>();

            services.AddTransient<IReferralService, ReferralService>();
            services.AddTransient<IReferralRepository, ReferralRepository>();

            services.AddTransient<IUserService, UserService>();

            services.AddSingleton<ICvxCodeRepository, CvxCodeRepository>();

            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<IProviderRepository, ProviderRepository>();

            services.AddTransient<ICheckOutDetailsservice, CheckOutDetailsService>();
            services.AddTransient<ICheckOutDetailsRepository, CheckOutDetailsRepository>();

            services.AddTransient<IPharmacyService, PharmacyService>();
            services.AddTransient<IPharmacyRepository, PharmacyRepository>();

            services.AddTransient<IGetSlotsservice, GetSlotsService>();
            services.AddTransient<IGetSlotsRepository, GetSlotsRepository>();
            services.AddTransient<IGetSlotDataRepository, GetSlotDataRepository>();

            services.AddTransient<IScheduleSlotsservice, ScheduleSlotsService>();
            services.AddTransient<IScheduleSlotsRepository, ScheduleSlotsRepository>();

            services.AddTransient<Services.Interfaces.CarePlan.IConditionService, Services.Implementations.CarePlan.ConditionService>();
            services.AddTransient<Repositories.Interfaces.CarePlan.IConditionRepository, Repositories.Implementations.CarePlan.ConditionRepository>();

            services.AddTransient<IConditionTargetsService, ConditionTargetsService>();
            services.AddTransient<IConditionTargetsRepository, ConditionTargetsRepository>();

            services.AddTransient<ITargetActionsService, TargetActionsService>();
            services.AddTransient<ITargetActionsRepository, TargetActionsRepository>();

            services.AddTransient<IPatientTargetsService, PatientTargetsService>();
            services.AddTransient<IPatientTargetsRepository, PatientTargetsRepository>();

            services.AddTransient<IPatientPlanService, PatientPlanService>();
            services.AddTransient<IPatientPlanRepository, PatientPlanRepository>();

            services.AddTransient<IPatientConditionsService, PatientConditionsService>();
            services.AddTransient<IPatientConditionsRepository, PatientConditionsRepository>();

            services.AddTransient<ICustomActionsService, CustomActionsService>();
            services.AddTransient<ICustomActionsRepository, CustomActionsRepository>();

            services.AddTransient<IPatientActionsService, PatientActionsService>();
            services.AddTransient<IPatientActionsRepository, PatientActionsRepository>();

            services.AddTransient<IServiceRequestService, ServiceRequestService>();
            services.AddTransient<IServiceRequestRepository, ServiceRequestRepository>();

            services.AddTransient<IAfterVisitPdfService, AfterVisitPdfService>();
            services.AddTransient<IAfterVisitPdfRepository, AfterVisitPdfRepository>();

            services.AddTransient<IPatientLifeGoalService, PatientLifeGoalService>();
            services.AddTransient<IPatientLifeGoalRepository, PatientLifeGoalRepository>();

            services.AddTransient<ISendSmsAfterVisitedService, SendSmsAfterVisitedService>();
            services.AddTransient<IIngestionClient, IngestionClient>();

            services.AddTransient<IDashboardService,DashboardService>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();


            return services;
        }

        public static IServiceCollection AddApiMappings(this IServiceCollection services, IConfiguration configuration)
        {
            // Specifies additional configuration parameters and registries all the AutoMapper profiles.
            services.AddAutoMapper(
                cfg =>
                {
                    cfg.Advanced.AllowAdditiveTypeMapCreation = true;
                },
                typeof(SystemContractsToRepositoryModelsProfile),
                typeof(RepositoryModelsToProcessContractsProfile));

            return services;
        }

        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CdcApiOptions>(configuration.GetSection(nameof(CdcApiOptions)));
            services.Configure<AppointmentOptions>(configuration.GetSection(nameof(AppointmentOptions)));

            // Adds mock site data from appSettings.
            services.Configure<SiteDataOptions>(configuration.GetSection(nameof(SiteDataOptions)));

            return services;
        }

        public static void SetupErrorHandlingAction(ErrorHandlingOptions options)
        {
            options.ResolveStatusCode = ResolveStatusCode;
            options.ResolveErrorMessage = ResolveErrorMessage;
        }

        private static string ResolveErrorMessage(Exception exception, IDictionary<Type, (int, string)> tuples)
        {
            return exception switch
            {
                Clinic.Api.Client.ApiException<Clinic.Api.Client.FaultResponse> apiClinicException =>
                    apiClinicException.Result.ErrorMessage,
                Ngdp.Api.Client.ApiException<Ngdp.Api.Client.FaultResponse> apiNgdpException =>
                    apiNgdpException.Result.ErrorMessage,
                _ => default
            };
        }

        private static int ResolveStatusCode(Exception exception, IDictionary<Type, (int, string)> tuples)
        {
            return exception switch
            {
                Clinic.Api.Client.ApiException apiClinicException => CheckErrorStatusCode(apiClinicException.StatusCode),
                Ngdp.Api.Client.ApiException apiNgdpException => CheckErrorStatusCode(apiNgdpException.StatusCode),
                _ => default
            };
        }

        private static int CheckErrorStatusCode(int statusCode)
        {
            return statusCode == 200 ? default : statusCode;
        }
    }
}
