// <copyright file="RelatedPersonWithCompartmentsBuilder.cs" company="Duly Health and Care">
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
    internal class RelatedPersonWithCompartmentsBuilder : IRelatedPersonWithCompartmentsBuilder
    {
        public RelatedPersonWithCompartments[] ExtractRelatedPersons(IEnumerable<R4.Bundle.EntryComponent> searchResult)
        {
            var relatedPersons = searchResult.Select(component => component.Resource).OfType<R4.RelatedPerson>();
            return relatedPersons.Select(person => new RelatedPersonWithCompartments
            {
                Resource = person
            }).ToArray();
        }
    }
}