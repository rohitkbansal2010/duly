// <copyright file="CareTeam.GeneralInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    [Description("Planned participants in the coordination and delivery of care for a patient or group")]
    public class CareTeamGeneralInfo
    {
        [Required]
        [Identity]
        [Description("External Id for this team")]
        public string Id { get; set; }
    }
}
