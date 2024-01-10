// <copyright file="PatientPhotoByParam.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    public class PatientPhotoByParam
    {
        public string PatientID { get; set; }
        public string PatientIDType { get; set; }
    }
}