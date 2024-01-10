// <copyright file="Provider.SearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    public class ProviderSearchCriteria
    {
        /// <summary>
        /// Latitude of the Location.
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// Longitude of the Location.
        /// </summary>
        public string Lng { get; set; }

        /// <summary>
        /// ProviderType.
        /// </summary>
        public string ProviderType { get; set; }

        public dynamic ConvertToParameters()
        {
            var parameters = new
            {
                vLatitude=Lat,
                vLongitude =Lng,
                providerType= ProviderType

            };

            return parameters;
        }
    }
}
