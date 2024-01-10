// <copyright file="LabDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Lab Details of the Patient")]
    public class LabDetails
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
        public DateTime? CreatedDate { get; set; }
        [Description("Id of the Appoitment")]
        public string Appointment_ID { get; set; }
        [Description("Id of the Patient")]
        public string Patient_ID { get; set; }
        [Description("Skipped")]
        public bool Skipped { get; set; }
    }
}