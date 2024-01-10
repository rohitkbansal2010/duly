// <copyright file="IAuditProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Audit
{
    /// <summary>
    /// Provides Audit functionality.
    /// </summary>
    public interface IAuditProvider
    {
        /// <summary>
        /// Sends log Entry to Audit.
        /// </summary>
        /// <param name="logEntry">Entry that needs to be added to Audit.</param>
        /// <returns>Task on completion.</returns>
        Task LogAsync(AuditLogEntry logEntry);
    }
}
