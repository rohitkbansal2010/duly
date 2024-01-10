// <copyright file="AuditUser.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Clinic.Fhir.Adapter.Audit
{
    /// <summary>
    /// Information about User.
    /// </summary>
    public class AuditUser
    {
        /// <summary>
        /// Value of given_name claim from jwt.
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// Value of family_name claim from jwt.
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        /// Value of upn claim from jwt.
        /// </summary>
        public string Upn { get; init; }

        /// <summary>
        /// Value of roles claim from jwt.
        /// </summary>
        public string[] Roles { get; init; }
    }
}