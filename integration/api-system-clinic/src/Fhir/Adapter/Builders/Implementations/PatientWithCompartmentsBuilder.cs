// <copyright file="PatientWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPatientWithCompartmentsBuilder"/>
    /// </summary>
    internal class PatientWithCompartmentsBuilder : IPatientWithCompartmentsBuilder
    {
        public PatientWithCompartments ExtractPatientWithCompartments(IEnumerable<R4.Bundle.EntryComponent> searchResult)
        {
            var patient = searchResult.Select(component => component.Resource).OfType<R4.Patient>().First();
            return new PatientWithCompartments
            {
                Resource = patient
            };
        }

        public PatientWithCompartments[] ExtractPatientsWithCompartments(IEnumerable<R4.Bundle.EntryComponent> searchResult)
        {
            var patients = searchResult.Select(component => component.Resource).OfType<R4.Patient>();
            return patients.Select(patient => new PatientWithCompartments
            {
                Resource = patient
            }).ToArray();
        }
    }
}