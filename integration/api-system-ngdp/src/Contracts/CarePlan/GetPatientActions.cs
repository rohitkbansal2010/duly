// <copyright file="GetPatientActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class GetPatientActions
    {
        [Description("PatientActionId")]
        public long PatientActionId { get; set; }
        [Description("ActionId")]
        public long ActionId { get; set; }
        [Description("ActionName")]
        public string ActionName { get; set; }

        [Description("ActionDescription")]
        public string ActionDescription { get; set; }
    }
}