// -----------------------------------------------------------------------
// <copyright file="PatientPhoto.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Patients photo .
    /// </summary>
    public class PatientPhoto
    {
        /// <summary>
        /// Patient Photo.
        /// </summary>
        public string Photo { get; set; }
        public string FileExtension { get; set; }
        public int FileSize { get; set; }
        public string Title { get; set; }
    }
}