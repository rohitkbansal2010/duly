// <copyright file="ProviderLocation.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    public class ProviderLocation
    {
        /// <summary>
        /// Id of lab ProviderLocation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of lab Provider.
        /// </summary>
        public string Provider_Id { get; set; }

        /// <summary>
        /// Name of  Provider.
        /// </summary>
        public string Provider_Name { get; set; }

        /// <summary>
        /// ProviderDisplayName.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// Provider_Photo_URL.
        /// </summary>
        public string Provider_Photo_URL { get; set; }

        /// <summary>
        /// Id of the Location.
        /// </summary>
        public string Location_Id { get; set; }

        /// <summary>
        /// location_Latitude_coordinate.
        /// </summary>
        public string Location_Latitude_coordinate { get; set; }

        /// <summary>
        /// Location_Longitude_coordinate.
        /// </summary>
        public string Location_Longitude_coordinate { get; set; }

        /// <summary>
        /// City of The Location.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Speciality of the Provider.
        /// </summary>
        public string Provider_Specialty { get; set; }

        /// <summary>
        /// Distance.
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Name of The Location.
        /// </summary>
        public string Location_Name { get; set; }

        /// <summary>
        /// Location_Add_1.
        /// </summary>
        public string Location_Add_1 { get; set; }

        /// <summary>
        /// Location_State.
        /// </summary>
        public string Location_State { get; set; }

        /// <summary>
        /// Location_Zip.
        /// </summary>
        public string Location_Zip { get; set; }

        /// <summary>
        /// Department_Id.
        /// </summary>
        public string Department_ID { get; set; }

    }
}
