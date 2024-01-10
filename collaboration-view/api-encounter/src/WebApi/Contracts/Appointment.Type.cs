// <copyright file="Appointment.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Describe a type of an appointment")]
    public enum AppointmentType
    {
        [Description("On-site appointment ")]
        OnSite,
        [Description("Telehealth Apointment")]
        Telehealth
    }
}