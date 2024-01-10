// -----------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

extern alias r4;
extern alias stu3;

using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces.Composite;
using Duly.Clinic.Fhir.Adapter.Audit;
using Duly.Clinic.Fhir.Adapter.Auth;
using Duly.Clinic.Fhir.Adapter.Auth.Settings;
using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Context;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Duly.Common.Messaging.Extensions;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using R4 = r4::Hl7.Fhir.Rest;
using STU3 = stu3::Hl7.Fhir.Rest;

namespace Duly.Clinic.Fhir.Adapter.Configuration
{
    /// <summary>
    /// Adds Fhir server and data mappings.
    /// </summary>
    public static class ModuleInitializer
    {
        /// <summary>
        /// Adds FhirAdapter and all related services.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddFhir(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IPractitionerWithCompartmentsAdapter, PractitionerWithCompartmentsAdapter>();
            services.AddTransient<IEncounterWithCompartmentsAdapter, EncounterWithCompartmentsAdapter>();
            services.AddTransient<ICareTeamWithCompartmentsAdapter, CareTeamWithCompartmentsAdapter>();
            services.AddTransient<IPatientWithCompartmentsAdapter, PatientWithCompartmentsAdapter>();
            services.AddTransient<IMedicationStatementWithCompartmentsAdapter, MedicationStatementWithCompartmentsAdapter>();
            services.AddTransient<IObservationWithCompartmentsAdapter, ObservationWithCompartmentsAdapter>();
            services.AddTransient<IAllergyIntoleranceAdapter, AllergyIntoleranceAdapter>();
            services.AddTransient<IHealthConditionAdapter, HealthConditionAdapter>();
            services.AddTransient<IDiagnosticReportWithCompartmentsAdapter, DiagnosticReportWithCompartmentsAdapter>();
            services.AddTransient(typeof(IGetFhirResourceById<>), typeof(RepositoryFhir<>));
            services.AddTransient<IImmunizationAdapter, ImmunizationAdapter>();
            services.AddTransient<IServiceRequestWithCompartmentsAdapter, ServiceRequestWithCompartmentsAdapter>();

            services.AddTransient<IEncounterWithCompartmentsBuilder, EncounterWithCompartmentsBuilder>();
            services.AddTransient<IPractitionerWithRolesBuilder, PractitionerWithRolesBuilder>();
            services.AddTransient<IRelatedPersonWithCompartmentsBuilder, RelatedPersonWithCompartmentsBuilder>();
            services.AddTransient<ICareTeamWithCompartmentsBuilder, CareTeamWithCompartmentsBuilder>();
            services.AddTransient<IOrganizationWithCompartmentsBuilder, OrganizationWithCompartmentsBuilder>();
            services.AddTransient<IPatientWithCompartmentsBuilder, PatientWithCompartmentsBuilder>();
            services.AddTransient<IMedicationStatementWithCompartmentsBuilder, MedicationStatementWithCompartmentsBuilder>();
            services.AddTransient<IObservationWithCompartmentsBuilder, ObservationWithCompartmentsBuilder>();
            services.AddTransient<IDiagnosticReportWithCompartmentsBuilder, DiagnosticReportWithCompartmentsBuilder>();
            services.AddTransient<IServicerequestWithCompartmentsBuilder, ServiceRequestWithCompartmentsBuilder>();
            services.AddTransient<IMedicationRequestWithCompartmentsBuilder, MedicationRequestWithCompartmentsBuilder>();

            services.Configure<DulyRestApiClientAuthSettings>(configuration.GetSection("DulyRestApiClient"));
            services.Configure<FhirAuthSettings>(configuration.GetSection("Fhir"));

            services.ConfigureServiceBus(configuration);

            services.AddTransient<IAuditAccountIdentityService, AuditAccountIdentityService>();

            services.AddSingleton<IClientAssertionCreator, ClientAssertionCreator>();
            services.AddSingleton<AuthorizationMessageHandler>();

            services.AddTransient<AuditMessageHandler>();
            services.AddTransient<IAuditProvider, ServiceBusAuditProvider>();

            services.AddTransient<IFhirClientR4, FhirClientWrapperR4>(provider => new FhirClientWrapperR4(CreateFhirR4Client(provider, configuration)));
            services.AddTransient<IFhirClientSTU3, FhirClientWrapperStu3>(provider => new FhirClientWrapperStu3(CreateFhirStu3Client(provider, configuration)));
            services.AddTransient<IPrivateEpicCall, PrivateEpicCall>(provider => new PrivateEpicCall(CreateHttpClient(provider), configuration));

            return services;
        }

        private static R4.FhirClient CreateFhirR4Client(IServiceProvider serviceProvider, IConfiguration config)
        {
            var url = config.GetSection("FhirServerUrls").GetValue<string>("BaseUrlR4");
            var fhirClientSettings = CreateDefaultFhirClientSettings();
            var authorizationMessageHandler = serviceProvider.GetService<AuthorizationMessageHandler>();

            return new R4.FhirClient(url, fhirClientSettings, authorizationMessageHandler);
        }

        private static STU3.FhirClient CreateFhirStu3Client(IServiceProvider serviceProvider, IConfiguration config)
        {
            var url = config.GetSection("FhirServerUrls").GetValue<string>("BaseUrlSTU3");
            var fhirClientSettings = CreateDefaultFhirClientSettings();
            var authorizationMessageHandler = serviceProvider.GetService<AuthorizationMessageHandler>();

            return new STU3.FhirClient(url, fhirClientSettings, authorizationMessageHandler);
        }

        private static HttpClient CreateHttpClient(IServiceProvider serviceProvider)
        {
            var authorizationMessageHandler = serviceProvider.GetService<AuthorizationMessageHandler>();
            return new HttpClient(authorizationMessageHandler);
        }

        private static FhirClientSettings CreateDefaultFhirClientSettings()
        {
            var fhirClientSettings = FhirClientSettings.CreateDefault();
            fhirClientSettings.PreferCompressedResponses = true;
            return fhirClientSettings;
        }
    }
}