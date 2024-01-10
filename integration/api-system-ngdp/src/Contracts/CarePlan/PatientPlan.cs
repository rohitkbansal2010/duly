// <copyright file="PatientPlan.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class PatientPlan
    {
        [Description("Flourishing Statement")]
        public string FlourishingStatement { get; set; }
        [Description("AppointmentId")]
        public string AppointmentId { get; set; }

        [Description("PatiendId")]
        public string PatientId { get; set; }
        [Description("ProviderId")]
        public string ProviderId { get; set; }
    }
}
