// <copyright file="GetConditionTargetsResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetConditionTargetsResponse
    {
        public long ConditionId { get; set; }
        public List<Targets> Targets { get; set; }
    }

    public class Targets
    {
        [Description("Target Id")]
        public long TargetId { get; set; }

        [Description("Target Name")]
        public string TargetName { get; set; }

        [Description("Normal Value")]
        public IEnumerable<GetTargetCategory> NormalValue { get; set; }

        [Description("Description")]
        public string Description { get; set; }

        [Description("Measurement Unit")]
        public string MeasurementUnit { get; set; }

        [Description("Active")]
        public bool Active { get; set; }
    }
}