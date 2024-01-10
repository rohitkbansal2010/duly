// -----------------------------------------------------------------------
// <copyright file="RecommendedVaccination.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Status of a recommented vaccination.")]

    public enum RecommendedVaccinationStatus
    {
        [Description("Vaccination completed.")]
        Completed,

        [Description("Vaccination addressed.")]
        Addressed,

        [Description("Vaccination is not required yet.")]
        NotDue,

        [Description("Vaccination is due soon.")]
        DueSoon,

        [Description("Vaccination should be done on ...")]
        DueOn,

        [Description("Overdue vaccination.")]
        Overdue,

        [Description("Vaccination postponed.")]
        Postponed
    }
}