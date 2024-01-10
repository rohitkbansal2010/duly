// <copyright file="ServicesExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Extensions;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Configuration;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Interfaces;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Repositories;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Extensions
{
    /// <summary>
    /// Extension method for the API services registration operations.
    /// </summary>
    public static class ServicesExtensions
    {
        public static IServiceCollection AddIngestionClient(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<IngestionSettings>(configuration.GetSection(nameof(IngestionSettings)));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration, "Authentication")
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddInMemoryTokenCaches();

            return
                services
                    .AddHttpClient()
                    .AddDbContexts(configuration, "DatabaseConnection")
                    .AddTransient<IIngestionClient, IngestionClient>()
                    .AddTransient<IReferralRepository, ReferralRepository>();
        }
    }
}
