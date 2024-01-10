// -----------------------------------------------------------------------
// <copyright file="ImagingDetail.cs" company="Duly Health and Care">
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
    public class ImagingDetail
    {
        public int ID { get; set; }
        [Description("Type of the  Imaging 'I'")]
        public string Type { get; set; }
        [Description("Type of the  Imaging")]
        public string ImagingType { get; set; }
        [Description("Id of the Appoitment")]
        public string Appointment_ID { get; set; }
        [Description("Id of the Patient")]
        public string Patient_ID { get; set; }
        [Description("Id of the Provider")]
        public string Provider_ID { get; set; }
        [Description("Id of the Location")]
        public string Location_ID { get; set; }
        [Description("Slot of The Booking")]
        public string BookingSlot { get; set; }
        [Description("AptScheduleDate")]
        public DateTime? AptScheduleDate { get; set; }
        [Description("ImagingLocation")]
        public string ImagingLocation { get; set; }
        [Description("Skipped")]

        public bool Skipped { get; set; }
    }
}