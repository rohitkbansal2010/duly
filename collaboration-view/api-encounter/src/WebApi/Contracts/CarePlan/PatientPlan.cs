// <copyright file="PatientPlan.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientPlan
    {
        [Description("PatiendId")]
        public string PatientId { get; set; }
        [Description("ProviderId")]
        public string ProviderId { get; set; }
        [Description("Flourishing Statement")]
        public string FlourishingStatement { get; set; }
        [Description("Appointment Id")]
        public string AppointmentId { get; set; }
    }
}