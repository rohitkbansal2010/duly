// -----------------------------------------------------------------------
// <copyright file="PatientPhotoByParam.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Patients photo parameters.
    /// </summary>
    internal class PatientPhotoByParam
    {
        /// <summary>
        /// Patient Id.
        /// </summary>
        public string PatientID { get; set; }

        /// <summary>
        /// Patient Id Type.
        /// </summary>
        public string PatientIDType { get; set; }
    }
}
