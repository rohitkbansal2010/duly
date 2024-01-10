// <copyright file="IPrivateEpicCall.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Extensions
{
    public interface IPrivateEpicCall
    {
        /// <summary>
        /// Returns patient photo.
        /// </summary>
        /// <param name="request">Search parameters.</param>
        public Task<GetPatientPhotoRoot> GetPatientPhotoAsync(PatientPhotoByParam request);
    }
}