// -----------------------------------------------------------------------
// <copyright file="Swagger.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Shared.Extensions.Configurations;
using Duly.Education.Api.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

namespace Duly.Education.Api.Configurations
{
    public static class Swagger
    {
        private const string ApiVersion = "v1";
        private const string ApiTitle = "duly-education-system-api";
        private const string ApiDescription = "Perform set of Education related operations requested from process API.";
        private const string ApiName = "Education system API";

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerCommon(
                new SwaggerCommonParameters
                {
                    ApiVersion = ApiVersion,
                    ApiTitle = ApiTitle,
                    ApiDescription = ApiDescription
                });

            return services
                .AddSwaggerExamplesFromAssemblyOf<EducationalMaterial>();
        }

        public static IApplicationBuilder UseEducationSwagger(this IApplicationBuilder app)
        {
            return app.UseSwaggerCommon(ApiVersion, ApiName);
        }
    }
}