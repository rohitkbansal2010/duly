// -----------------------------------------------------------------------
// <copyright file="HealthConditions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about all of a patient's problems in two sets of health conditions: Current health conditions and Previous health conditions.")]
    public class HealthConditions
    {
        [Description("Information about all resolved or inactive patient's health conditions (problems).")]
        [Required]
        public HealthCondition[] PreviousHealthConditions { get; set; }

        [Description("Information about all active patient's health conditions (problems).")]
        [Required]
        public HealthCondition[] CurrentHealthConditions { get; set; }
    }
}