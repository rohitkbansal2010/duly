// <copyright file="ProviderLocation.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Contracts
{
    [Description("ProviderLocation")]
    public class ProviderLocation
    {
        [Description("ID of the ProviderLocation")]
        public int Id { get; set; }

        [Description("ID of the Provider")]
        public string Provider_Id { get; set; }

        [Description("Name of the Provider")]
        public string Provider_Name { get; set; }

        [Description("ProviderDisplayName")]
        public string ProviderDisplayName { get; set; }

        [Description("Provider_Photo_URL")]
        public string Provider_Photo_URL { get; set; }

        [Description("Location_Id")]
        public string Location_Id { get; set; }

        [Description("location_Latitude_coordinate")]
        public string Location_Latitude_coordinate { get; set; }

        [Description("location_Longitude_coordinate")]
        public string Location_Longitude_coordinate { get; set; }

        [Description("city")]
        public string City { get; set; }

        [Description("Provider_Specialty")]
        public string Provider_Specialty { get; set; }

        [Description("distance")]
        public double Distance { get; set; }

        [Description("Location_Name")]
        public string Location_Name { get; set; }

        [Description("Location_Add_1")]
        public string Location_Add_1 { get; set; }

        [Description("Location_State")]
        public string Location_State { get; set; }

        [Description("Location_Zip")]
        public string Location_Zip { get; set; }

        [Description("Department_Id")]
        public string Department_Id { get; set; }
    }
}
