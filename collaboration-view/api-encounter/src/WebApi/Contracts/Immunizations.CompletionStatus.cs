// -----------------------------------------------------------------------
// <copyright file="Immunizations.CompletionStatus.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Immunizations completion status.")]

    public enum ImmunizationsCompletionStatus
    {
        [Description("All vaccinations have been made.")]
        Completed,

        [Description("Greater than or equal to 70 and less than 100 percent of vaccinations have been made.")]
        Almost,

        [Description("Less than 70 percent of vaccinations have been made.")]
        LaggingBehind
    }
}