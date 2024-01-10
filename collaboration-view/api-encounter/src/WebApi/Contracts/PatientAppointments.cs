// -----------------------------------------------------------------------
// <copyright file="PatientAppointments.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Grouped schedule of patient appointments.")]
    public class PatientAppointments
    {
        [Description("Information about all groups of the recent appointments for the required period.")]
        [Required]
        public PatientAppointmentsGroup[] RecentAppointments { get; set; }

        [Description("Information about all groups of the upcoming appointments.")]
        [Required]
        public PatientAppointmentsGroup[] UpcomingAppointments { get; set; }

        public static PatientAppointments BuildEmptyPatientAppointments()
        {
            return new PatientAppointments
            {
                RecentAppointments = Array.Empty<PatientAppointmentsGroup>(),
                UpcomingAppointments = Array.Empty<PatientAppointmentsGroup>()
            };
        }
    }
}
