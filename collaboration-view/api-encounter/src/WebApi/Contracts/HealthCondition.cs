// -----------------------------------------------------------------------
// <copyright file="HealthCondition.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about the health condition (the patient's problem).")]
    public class HealthCondition
    {
        [Description("The name of the health condition (the patient's problem).")]
        [Required]
        public string Title { get; set; }

        [Description("The date the condition was recorded or resolved.")]
        public DateTimeOffset? Date { get; set; }
    }
}