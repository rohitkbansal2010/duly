// -----------------------------------------------------------------------
// <copyright file="Swagger.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Api.ExampleProviders;
using Duly.Common.Annotations.Configurations;
using Duly.Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

namespace Duly.Clinic.Api.Configurations
{
    internal static class Swagger
    {
        private const string ApiVersion = "v1";
        private const string ApiTitle = "duly-clinic-api";
        private const string ApiDescription = "Perform set of Epic related operations requested from process API.";
        private const string ApiName = "Duly Clinic API";

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerCommon(
                new SwaggerCommonParameters
                {
                    ApiVersion = ApiVersion,
                    ApiTitle = ApiTitle,
                    ApiDescription = ApiDescription,
                    FaultResponseType = typeof(FaultResponse)
                },
                options =>
                {
                    var initSwaggerSecurityParameters = SwaggerCommon.ReadSwaggerSecurityParametersFromConfiguration(configuration);
                    if (initSwaggerSecurityParameters.IsValid())
                    {
                        SwaggerCommon.AddOauth2SecurityDefinition(options, initSwaggerSecurityParameters);
                    }

                    options.UseAllOfToExtendReferenceSchemas();
                });

            return services.AddSwaggerExamplesFromAssemblyOf<AllergyIntoleranceExampleProvider>();
        }

        public static IApplicationBuilder UseClinicSystemSwagger(this IApplicationBuilder app)
        {
            return app.UseSwaggerCommon(ApiVersion, ApiName);
        }
    }
}