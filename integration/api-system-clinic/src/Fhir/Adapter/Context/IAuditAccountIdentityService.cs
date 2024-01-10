// <copyright file="IAuditAccountIdentityService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Audit;
using System;

namespace Duly.Clinic.Fhir.Adapter.Context
{
    public interface IAuditAccountIdentityService
    {
        /// <summary>
        /// Get User from the header.
        /// </summary>
        AuditUser GetUser();

        /// <summary>
        /// Gets AppId from header.
        /// </summary>
        string GetAppId();

        /// <summary>
        /// Get the unique identifier of the request from the <see cref="Common.Infrastructure.Constants.ParameterNames.XCorrelationIdHeader" /> header.
        /// </summary>
        Guid GetXCorrelationId();
    }
}
