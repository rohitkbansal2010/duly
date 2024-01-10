// -----------------------------------------------------------------------
// <copyright file="ScheduleFollowUp.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class ScheduleFollowUp
    {
        [Description("Id of the Follow or referal details")]
        public int ID { get; set; }
        [Description("ID od the Provide")]
        public string Provider_ID { get; set; }
        [Description("ID of the patient")]
        public string Patient_ID { get; set; }
        [Description("Type of Appoinment")]
        public string AptType { get; set; }
        [Description("Format of Appoitment")]
        public string AptFormat { get; set; }
        [Description("ID od Location")]
        public string Location_ID { get; set; }
        [Description("appoitment schedule Date and Time")]
        public DateTimeOffset? AptScheduleDate { get; set; }
        [Description("Booking slot")]
        public string BookingSlot { get; set; }
        [Description("Referal visit type")]
        public string RefVisitType { get; set; }
        [Description("Created Date and Time")]
        public DateTimeOffset? Created_Date { get; set; }
        [Description("Type")]
        public string Type { get; set; }
        [Description("Appointment Id")]
        public string Appointment_Id { get; set; }
        [Description("Skipped")]
        public bool Skipped { get; set; }

        [Description("Visit Type Id")]
        public string VisitTypeId { get; set; }
    }
}