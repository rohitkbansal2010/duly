// -----------------------------------------------------------------------
// <copyright file="Provider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// A Details about the provider.
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// Identifier by which this Provider Location is known.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// provider_Id.
        /// </summary>

        public string Provider_Id { get; set; }

        /// <summary>
        /// Id of the location.
        /// </summary>
        public string Location_Id { get; set; }

        /// <summary>
        /// name of the Provider.
        /// </summary>
        public string Provider_Name { get; set; }

        /// <summary>
        /// providerDisplayName.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// provider_Photo_URL.
        /// </summary>
        public string Provider_Photo_URL { get; set; }

        /// <summary>
        /// location_Latitude_coordinate.
        /// </summary>
        public string Location_Latitude_coordinate { get; set; }

        /// <summary>
        /// location_Longitude_coordinate.
        /// </summary>
        public string Location_Longitude_coordinate { get; set; }

        /// <summary>
        /// city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///  provider_Specialty.
        /// </summary>
        public string Provider_Specialty { get; set; }

        /// <summary>
        ///  distance.
        /// </summary>

        public double Distance { get; set; }

        /// <summary>
        ///  location_Name.
        /// </summary>
        public string Location_Name { get; set; }

        /// <summary>
        ///  location_Add_1.
        /// </summary>

        public string Location_Add_1 { get; set; }

        /// <summary>
        /// location_State.
        /// </summary>
        public string Location_State { get; set; }

        /// <summary>
        /// location_Zip.
        /// </summary>

        public string Location_Zip { get; set; }

        /// <summary>
        /// Department_Id.
        /// </summary>

        public string Department_Id { get; set; }
    }
}
