// <copyright file="AuditMeta.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Clinic.Fhir.Adapter.Audit
{
    /// <summary>
    /// Additional information.
    /// </summary>
    public class AuditMeta
    {
        /// <summary>
        /// Response code.
        /// </summary>
        public string ResponseCode { get; init; }

        /// <summary>
        /// Exception details.
        /// </summary>
        public string ExceptionMessage { get; init; }
    }
}