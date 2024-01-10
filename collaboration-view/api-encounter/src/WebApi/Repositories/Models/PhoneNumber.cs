// -----------------------------------------------------------------------
// <copyright file="PhoneNumber.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Address of patient.
    /// </summary>
    internal class PhoneNumber
    {
        /// <summary>
        /// Phone Number.
        /// </summary>
        public string PhoneNum { get; set; }

        /// <summary>
        /// Usage.
        /// </summary>
        public string Use { get; set; }
    }
}
