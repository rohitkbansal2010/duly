// -----------------------------------------------------------------------
// <copyright file="CdcApiClientConfigurator.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.CdcClient.Implementations;
using Duly.CollaborationView.CdcClient.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Duly.CollaborationView.CdcClient.Configurations
{
    /// <summary>
    /// Adds CdcClient.
    /// </summary>
    public static class CdcApiClientConfigurator
    {
        public static IServiceCollection AddCdcApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CdcApiClientOptions>(configuration.GetSection(nameof(CdcApiClientOptions)));

            services.AddHttpClient<ICdcApiClient, CdcApiClient>(SetUpClient);

            return services;
        }

        private static void SetUpClient(IServiceProvider provider, HttpClient httpClient)
        {
            var optionsMonitor = provider.GetService<IOptionsMonitor<CdcApiClientOptions>>();
            var options = optionsMonitor.CurrentValue;
            if (!string.IsNullOrEmpty(options.ApiBaseAddress))
                httpClient.BaseAddress = new Uri(options.ApiBaseAddress);
        }
    }
}