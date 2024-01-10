// -----------------------------------------------------------------------
// <copyright file="PatientAndAppointmentCount.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts.Dashboard
{
    public class PatientAndAppointmentCount
    {
        [Description("Location Id")]
        public string LocationId { get; set; }
        [Description("Location Name")]
        public string LocationName { get; set; }
        [Description("Total patient count in particualar location")]
        public string TotalPatientCount { get; set; }
        [Description("Current Appointment")]
        public string TodaysAppointment { get; set; }
        //[Description("Location Zipcode")]
        //public string ZipCode { get; set; }
    }
}
