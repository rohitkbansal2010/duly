// -----------------------------------------------------------------------
// <copyright file="LabLocation.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    public class LabLocation
    {
        /// <summary>
        /// Lab Id.
        /// </summary>
        public int LabId { get; set; }

        /// <summary>
        /// Lab Name.
        /// </summary>
        public string LabName { get; set; }

        /// <summary>
        /// External Lab Yes or No.
        /// </summary>
        public string ExternalLabYn { get; set; }

        /// <summary>
        /// Lab Lb Id.
        /// </summary>
        public string LabLlbId { get; set; }

        /// <summary>
        /// Lab Lb name.
        /// </summary>
        public string LlbName { get; set; }

        /// <summary>
        /// Lab Lb Address Line1 .
        /// </summary>
        public string LlbAddressLn1 { get; set; }

        /// <summary>
        /// Lab Lb Address Line2 .
        /// </summary>
        public string LlbAddressLn2 { get; set; }

        /// <summary>
        /// Lab Location City.
        /// </summary>
        public string LLCity { get; set; }

        /// <summary>
        /// Lab Location state.
        /// </summary>
        public string LLState { get; set; }

        /// <summary>
        /// Lab Location Zip code.
        /// </summary>
        public string LLZip { get; set; }

        /// <summary>
        /// Latitude of the Lab location.
        /// </summary>
        public double LabLatitude { get; set; }

        /// <summary>
        /// Longitude of the Lab location.
        /// </summary>
        public string LabLongitude { get; set; }

        /// <summary>
        /// Distance.
        /// </summary>
        public string Distance { get; set; }

        /// <summary>
        /// Contact Number.
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Working Hours.
        /// </summary>
        public string WorkingHours { get; set; }
    }
}
