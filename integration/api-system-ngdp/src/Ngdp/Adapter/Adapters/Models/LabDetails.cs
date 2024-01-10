// <copyright file="LabDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    public class LabDetails
    {
        /// <summary>
        /// Id of lab Details.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Type of lab Details.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Id of Lab.
        /// </summary>
        public string Lab_ID { get; set; }

        /// <summary>
        /// Location of lab.
        /// </summary>
        public string Lab_Location { get; set; }

        /// <summary>
        /// Name of the Lab.
        /// </summary>
        public string Lab_Name { get; set; }

        /// <summary>
        /// Created Date.
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Id of the Appoitment.
        /// </summary>
        public string Appointment_ID { get; set; }

        /// <summary>
        /// Id of the Patient.
        /// </summary>
        public string Patient_ID { get; set; }

        /// <summary>
        /// Skipped.
        /// </summary>
        public bool Skipped { get; set; }
    }
}