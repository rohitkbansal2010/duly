// <copyright file="AuditEntryType.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Clinic.Fhir.Adapter.Audit
{
    /// <summary>
    /// Types of audit event.
    /// </summary>
    public enum AuditEntryType
    {
        /// <summary>
        /// Data is being requested.
        /// </summary>
        Request,

        /// <summary>
        /// Data was received.
        /// </summary>
        Response,

        /// <summary>
        /// Data request failed.
        /// </summary>
        Exception
    }
}