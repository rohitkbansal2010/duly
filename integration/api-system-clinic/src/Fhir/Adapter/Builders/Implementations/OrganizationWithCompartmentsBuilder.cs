// <copyright file="OrganizationWithCompartmentsBuilder.cs" company="Duly Health and Care">
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
    internal class OrganizationWithCompartmentsBuilder : IOrganizationWithCompartmentsBuilder
    {
        public OrganizationWithCompartments[] ExtractOrganizations(IEnumerable<R4.Bundle.EntryComponent> searchResult)
        {
            var organizations = searchResult.Select(component => component.Resource).OfType<R4.Organization>().ToArray();
            return organizations.Select(organization => new OrganizationWithCompartments
            {
                Resource = organization
            }).ToArray();
        }
    }
}