// <copyright file="GetLabOrImagingDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Lab Details of the Patient")]
    public class GetLabOrImagingDetails
    {
        [Description("ID of Lab Details")]
        public int ID { get; set; }
        [Description("Type of lab Details")]
        public string Type { get; set; }
        [Description("Id of Lab")]
        public string Lab_ID { get; set; }
        [Description("Location of lab")]
        public string Lab_Location { get; set; }
        [Description("Name of the Lab")]
        public string Lab_Name { get; set; }
        [Description("Created Date")]
        public DateTimeOffset CreatedDate { get; set; }
        [Description("Id of the Appoitment")]
        public string Appointment_ID { get; set; }
        [Description("Id of the Patient")]
        public string Patient_ID { get; set; }
        [Description("Provider Id")]
        public string Provider_ID { get; set; }
        [Description("Location Id")]
        public string Location_ID { get; set; }
        [Description("BookingSlot")]
        public string BookingSlot { get; set; }
        [Description("Appointment Schedule Date")]
        public DateTimeOffset AptScheduleDate { get; set; }
        [Description("Imaging Location")]
        public string ImagingLocation { get; set; }
        [Description("Imaging Type")]
        public string ImagingType { get; set; }
        [Description("Skipped")]
        public bool Skipped { get; set; }
    }
}