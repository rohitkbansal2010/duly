// -----------------------------------------------------------------------
// <copyright file="LabDetails.cs" company="Duly Health and Care">
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
    /// Lab Detail for a specific patient.
    /// </summary>
    public class LabDetails
    {
        /// <summary>
        /// Identifier by which this Lab Detail is known.
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
        /// Location of lab.
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