// -----------------------------------------------------------------------
// <copyright file="Practitioner.GeneralInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// A person who is directly or indirectly involved in the provisioning of healthcare.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    internal class PractitionerGeneralInfo : HumanGeneralInfoWithPhoto, IEntityWithIdentifiers
    {
        /// <summary>
        /// Roles which this practitioner may perform.
        /// </summary>
        public Role[] Roles { get; set; }

        /// <summary>
        /// Speciality which this practitioner may perform.
        /// </summary>
        public List<string> Speciality { get; set; }

        /// <summary>
        /// Identifiers of the practitioners. Format: (Text|VALUE).
        /// </summary>
        public string[] Identifiers { get; set; }
    }
}