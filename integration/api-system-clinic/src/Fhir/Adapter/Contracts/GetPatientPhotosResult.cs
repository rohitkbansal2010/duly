// <copyright file="GetPatientPhotosResult.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    public class GetPatientPhotosResult
    {
        public bool IsPhotoPresent { get; set; }
        public List<PatientPhotos> PatientPhotos { get; set; }
    }
}