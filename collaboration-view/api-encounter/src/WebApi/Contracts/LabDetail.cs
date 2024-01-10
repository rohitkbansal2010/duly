// -----------------------------------------------------------------------
// <copyright file="LabDetail.cs" company="Duly Health and Care">
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
    public class LabDetail
    {
        public int ID { get; set; }
        [Description("Optional Parameter Type Lab or Imaging")]
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