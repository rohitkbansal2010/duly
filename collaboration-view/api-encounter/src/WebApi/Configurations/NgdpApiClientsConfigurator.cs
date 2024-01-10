// -----------------------------------------------------------------------
// <copyright file="NgdpApiClientsConfigurator.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Ngdp.Api.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    /// <summary>
    /// Adds Ngdp API clients.
    /// </summary>
    public static class NgdpApiClientsConfigurator
    {
        private const string SubscriptionKeyHeaderName = "subscription-Key";

        public static IServiceCollection AddNgdpApiClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<NgdpApiClientOptions>(configuration.GetSection(nameof(NgdpApiClientOptions)));

            services.AddTransient<NgdpAuthorizationMessageHandler>();

            services.AddHttpClient<ILocationsClient, LocationsClient>();
            services.AddHttpClient<IAppointmentsClient, AppointmentsClient>();

            services.AddHttpClient<IPatientsClient, PatientsClient>("NgdpPatientsClient");
            services.AddHttpClient<ILatlngClient, LatlngClient>("NgdpProviderClient");

            services.AddHttpClient<IClient, Client>("NgdpClient");

            services.AddHttpClient<IAppointmentIdClient, AppointmentIdClient>("NgdpAppointmentClient");

            return services;
        }

        private static void SetUpClient(IServiceProvider provider, HttpClient httpClient)
        {
            var optionsMonitor = provider.GetService<IOptionsMonitor<NgdpApiClientOptions>>();
            var options = optionsMonitor.CurrentValue;
            httpClient.BaseAddress = new Uri(options.ApiBaseAddress);
            httpClient.Timeout = TimeSpan.FromSeconds(options.Timeout);
            if (!string.IsNullOrEmpty(options.SubscriptionKey))
                httpClient.DefaultRequestHeaders.Add(SubscriptionKeyHeaderName, options.SubscriptionKey);
        }

        private static void AddHttpClient<TClient, TImplementation>(this IServiceCollection services)
            where TClient : class
            where TImplementation : class, TClient
        {
            services.AddHttpClient<TClient, TImplementation>(SetUpClient).AddHttpMessageHandler<NgdpAuthorizationMessageHandler>();
        }

        private static void AddHttpClient<TClient, TImplementation>(this IServiceCollection services, string name)
            where TClient : class
            where TImplementation : class, TClient
        {
            services.AddHttpClient<TClient, TImplementation>(name, SetUpClient).AddHttpMessageHandler<NgdpAuthorizationMessageHandler>();
        }
    }
}