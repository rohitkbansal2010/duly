// -----------------------------------------------------------------------
// <copyright file="Site.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Sites Data.
    /// </summary>
    public class Site
    {
        /// <summary>
        /// Site Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Address line.
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State of U.S.A..
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Postal code, ZIP code.
        /// </summary>
        public string PostalCode { get; set; }
    }
}