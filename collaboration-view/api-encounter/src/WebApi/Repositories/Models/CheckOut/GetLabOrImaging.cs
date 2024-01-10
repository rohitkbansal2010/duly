// -----------------------------------------------------------------------
// <copyright file="GetLabOrImaging.cs" company="Duly Health and Care">
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
    public class GetLabOrImaging
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
        public DateTimeOffset CreatedDate { get; set; }

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
        /// Appointment Schedule Date.
        /// </summary>
        public DateTimeOffset AptScheduleDate { get; set; }

        /// <summary>
        /// Imaging Location.
        /// </summary>
        public string ImagingLocation { get; set; }

        /// <summary>
        /// Imaging Type.
        /// </summary>
        public string ImagingType { get; set; }

        /// <summary>
        /// Appointment Schedule Date.
        /// </summary>
        public bool Skipped { get; set; }
    }
}