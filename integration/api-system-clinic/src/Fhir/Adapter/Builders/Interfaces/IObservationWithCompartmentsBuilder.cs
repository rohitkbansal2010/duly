// <copyright file="IObservationWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Interfaces
{
    internal interface IObservationWithCompartmentsBuilder
    {
        /// <summary>
        /// Extract Observations with compartments from Bundles.
        /// </summary>
        /// <param name="searchResult">Entries from search.</param>
        /// <returns>Extracted observations.</returns>
        ObservationWithCompartments[] ExtractObservations(IEnumerable<R4.Bundle.EntryComponent> searchResult);
    }
}