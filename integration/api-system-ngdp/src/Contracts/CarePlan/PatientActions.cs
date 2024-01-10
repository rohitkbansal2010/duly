// <copyright file="PatientActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class PatientActions
    {
        [Description("Patiend Target Id")]
        public long PatientTargetId { get; set; }
        [Description("Action Id")]
        public long ActionId { get; set; }
        [Description("Action Type")]
        public string ActionType { get; set; }
        [Description("Custom Action Id")]
        public long CustomActionId { get; set; }
        [Description("Deleted")]
        public bool Deleted { get; set; }
    }
}
