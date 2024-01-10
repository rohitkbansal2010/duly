// -----------------------------------------------------------------------
// <copyright file="ImagingDetails.cs" company="Duly Health and Care">
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
    /// Imaging Details for a specific patient.
    /// </summary>
    public class ImagingDetails
    {
        /// <summary>
        /// Identifier by which this Imaging Detail is known.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Type of lab Imaging Details.
        /// </summary>
        public string ImagingType { get; set; }

        /// <summary>
        /// Id of the Appoitment.
        /// </summary>
        public string Appointment_ID { get; set; }

        /// <summary>
        /// Id of the Patient.
        /// </summary>
        public string Patient_ID { get; set; }

        /// <summary>
        /// Id of the Provider.
        /// </summary>
        public string Provider_ID { get; set; }

        /// <summary>
        /// Id of the Location.
        /// </summary>
        public string Location_ID { get; set; }

        /// <summary>
        /// BookingSlot.
        /// </summary>
        public string BookingSlot { get; set; }

        /// <summary>
        /// AptScheduleDate.
        /// </summary>
        public DateTime? AptScheduleDate { get; set; }

        /// <summary>
        /// Location of Imaging.
        /// </summary>
        public string ImagingLocation { get; set; }

        /// <summary>
        /// Imaging Type 'I'.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Skipped.
        /// </summary>
        public bool Skipped { get; set; }
    }
}