// -----------------------------------------------------------------------
// <copyright file="ProviderDetails.cs" company="Duly Health and Care">
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
    /// Details of the provider.
    /// </summary>
    public class ProviderDetails
    {
        /// <summary>
        /// Id of Provider Table.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Id of the Location".
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// ID of the provider.
        /// </summary>
        public string ProviderId { get; set; }

        /// <summary>
        /// Name of the Provider.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Display name of the provider.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// Url of the providers photo.
        /// </summary>
        public string ProviderPhotoURL { get; set; }

        /// <summary>
        /// Latitude coordinates of the location.
        /// </summary>
        public string LocationLatitudeCoordinate { get; set; }

        /// <summary>
        /// Longitudnal coordinate of the location.
        /// </summary>
        public string LocationLongitudeCoordinate { get; set; }

        /// <summary>
        /// city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Speciality of the provider.
        /// </summary>
        public string ProviderSpecialty { get; set; }

        /// <summary>
        /// Distance.
        /// </summary>
        public string Distance { get; set; }

        /// <summary>
        /// Name of the location.
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Address of the location.
        /// </summary>
        public string LocationAdd_1 { get; set; }

        /// <summary>
        /// State of the location.
        /// </summary>
        public string LocationState { get; set; }

        /// <summary>
        /// Zipcode of the location.
        /// </summary>
        public string LocationZip { get; set; }
    }
}