// -----------------------------------------------------------------------
// <copyright file="ClinicApiClientsConfigurator.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Api.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    /// <summary>
    /// Adds Clinic API clients.
    /// </summary>
    public static class ClinicApiClientsConfigurator
    {
        private const string SubscriptionKeyHeaderName = "subscription-Key";

        public static IServiceCollection AddClinicApiClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ClinicApiClientOptions>(configuration.GetSection(nameof(ClinicApiClientOptions)));

            services.AddTransient<ClinicAuthorizationMessageHandler>();

            services.AddHttpClient<IClient, Client>();
            services.AddHttpClient<IPatientsClient, PatientsClient>();
            services.AddHttpClient<ISitesClient, SitesClient>();
            services.AddHttpClient<IEncountersClient, EncountersClient>();
            services.AddHttpClient<ICareTeamClient, CareTeamClient>();
            services.AddHttpClient<IObservationsClient, ObservationsClient>();
            services.AddHttpClient<IVitalSignsClient, VitalSignsClient>();
            services.AddHttpClient<IAllergyIntoleranceClient, AllergyIntoleranceClient>();
            services.AddHttpClient<IConditionsClient, ConditionsClient>();
            services.AddHttpClient<IAppointmentIdClient, AppointmentIdClient>();
            services.AddHttpClient<IPatientIdClient, PatientIdClient>();

            return services;
        }

        private static void AddHttpClient<TClient, TImplementation>(this IServiceCollection services)
            where TClient : class
            where TImplementation : class, TClient
        {
            services.AddHttpClient<TClient, TImplementation>(SetUpClient).AddHttpMessageHandler<ClinicAuthorizationMessageHandler>();
        }

        private static void SetUpClient(IServiceProvider provider, HttpClient httpClient)
        {
            var optionsMonitor = provider.GetService<IOptionsMonitor<ClinicApiClientOptions>>();
            var options = optionsMonitor.CurrentValue;
            httpClient.BaseAddress = new Uri(options.ApiBaseAddress);
            httpClient.Timeout = TimeSpan.FromSeconds(options.Timeout);
            if (!string.IsNullOrEmpty(options.SubscriptionKey))
                httpClient.DefaultRequestHeaders.Add(SubscriptionKeyHeaderName, options.SubscriptionKey);
        }
    }
}