// -----------------------------------------------------------------------
// <copyright file="ScheduleReferral.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut
{
    /// <summary>
    /// ScheduleFollowUp for a specific patient.
    /// </summary>
    public class ScheduleReferral
    {
        /// <summary>
        /// Identifier by which this ScheduleFollowUp is known.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// ID of the Provider.
        /// </summary>
        public string Provider_ID { get; set; }

        /// <summary>
        /// ID of the patient.
        /// </summary>
        public string Patient_ID { get; set; }

        /// <summary>
        /// Type of Appoinment.
        /// </summary>
        public string AptType { get; set; }

        /// <summary>
        /// Format of Appoitment.
        /// </summary>
        public string AptFormat { get; set; }

        /// <summary>
        /// ID od Location.
        /// </summary>
        public string Location_ID { get; set; }

        /// <summary>
        /// appoitment schedule Date and Time.
        /// </summary>
        public DateTimeOffset? AptScheduleDate { get; set; }

        /// <summary>
        /// Booking slot.
        /// </summary>
        public string BookingSlot { get; set; }

        /// <summary>
        /// Referal visit type.
        /// </summary>
        public string RefVisitType { get; set; }

        /// <summary>
        /// Created Date and Time.
        /// </summary>
        public DateTimeOffset? Created_Date { get; set; }

        /// <summary>
        /// Optional Parameter Type Referral or Schedule.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Appointment Id.
        /// </summary>
        public string Appointment_Id { get; set; }

        /// <summary>
        /// Skipped.
        /// </summary>
        public bool Skipped { get; set; }

        /// <summary>
        /// Department Id.
        /// </summary>
        public string Department_Id { get; set; }

        /// <summary>
        /// Visit Type Id.
        /// </summary>
        public string VisitType_Id { get; set; }
    }
}