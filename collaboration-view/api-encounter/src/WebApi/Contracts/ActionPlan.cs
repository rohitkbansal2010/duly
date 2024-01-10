// -----------------------------------------------------------------------
// <copyright file="ActionPlan.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("The list of groups of objects formed by the risks. The higher level objects contain the title of the risk and a field with an array of the actions: title, summary, etc. ")]
    public class ActionPlan
    {
        [Required]
        public string Id { get; set; }
        public string GroupTitle { get; set; }
        public IList<ActionPlanEvent> Events { get; set; }
    }
}
