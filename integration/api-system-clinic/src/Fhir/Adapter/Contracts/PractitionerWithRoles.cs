// <copyright file="PractitionerWithRoles.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
extern alias stu3;

using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Practitioner and their registered roles.
    /// </summary>
    public class PractitionerWithRoles : IResourceWithCompartments<R4.Practitioner>
    {
        /// <summary>
        /// Practitioner.
        /// </summary>
        public R4.Practitioner Resource { get; set; }

        /// <summary>
        /// Roles that this practitioner takes.
        /// </summary>
        public R4.PractitionerRole[] Roles { get; set; }
    }

    /// <summary>
    /// Practitioner and their registered roles.
    /// </summary>
    public class PractitionerWithRolesSTU3 : IResourceWithCompartments<STU3.Practitioner>
    {
        /// <summary>
        /// Practitioner.
        /// </summary>
        public STU3.Practitioner Resource { get; set; }

        /// <summary>
        /// Roles that this practitioner takes.
        /// </summary>
        public STU3.PractitionerRole[] Roles { get; set; }
    }
}
