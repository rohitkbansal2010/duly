// -----------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using Wipfli.Adapter.Adapters.Implementations;
using Wipfli.Adapter.Adapters.Interfaces;
using Wipfli.Adapter.Client;

namespace Wipfli.Adapter.Configuration
{
    public static class ModuleInitializer
    {
        public static IServiceCollection AddPrivateApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PrivateApiOptions>(configuration.GetSection(nameof(PrivateApiOptions)));

            services.AddSingleton<IPrivateApiCertificateProvider, PrivateApiCertificateProvider>();
            services.AddTransient<WipfliHttpClientHandler>();

            services.AddTransient<WipfliAuthorizationMessageHandler>();

            services
                .AddHttpClient<IWipfliClient, WipfliClient>(SetUpClient)
                .ConfigurePrimaryHttpMessageHandler<WipfliHttpClientHandler>()
                .AddHttpMessageHandler<WipfliAuthorizationMessageHandler>();

            services.AddTransient<IScheduleAdapter, ScheduleAdapter>();

            return services;
        }

        private static void SetUpClient(IServiceProvider provider, HttpClient httpClient)
        {
            var options = provider.GetService<IOptionsMonitor<PrivateApiOptions>>().CurrentValue;

            httpClient.BaseAddress = new Uri(options.ApiBaseAddress);
            httpClient.Timeout = TimeSpan.FromSeconds(options.Timeout);
        }
    }
}