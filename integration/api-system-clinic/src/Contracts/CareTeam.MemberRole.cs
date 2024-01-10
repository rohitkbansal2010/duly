// <copyright file="CareTeam.MemberRole.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Participant Roles")]
    public enum MemberRole
    {
        [Description("Practitioner with roles")]
        Practitioner,

        [Description("Related person")]
        RelatedPerson,

        [Description("Organization")]
        Organization,

        [Description("Care team")]
        CareTeam
    }
}