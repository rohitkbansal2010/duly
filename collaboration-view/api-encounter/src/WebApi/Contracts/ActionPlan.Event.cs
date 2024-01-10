// -----------------------------------------------------------------------
// <copyright file="ActionPlan.Event.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class ActionPlanEvent
    {
        [Required]
        public string Id { get; set; }
        public string Title { get; set; }
    }
}