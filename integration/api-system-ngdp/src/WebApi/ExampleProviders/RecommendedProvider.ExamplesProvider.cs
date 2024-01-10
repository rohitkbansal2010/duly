// <copyright file="RecommendedProvider.ExamplesProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Duly.Ngdp.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "ExampleProviders")]
    public class RecommendedProviderExamplesProvider : IExamplesProvider<IEnumerable<RecommendedProvider>>
    {
        public IEnumerable<RecommendedProvider> GetExamples()
        {
            yield return new RecommendedProvider
            {
                Identifier = "16620533_7650077_10031_9794_112368",
                Specialty = BuildSpecialty("10031"),
                Department = BuildDepartment("23231", "Cardiology - Airlite St, Elgin"),
                Visit = BuildVisit("1124", "Diagnostic Procedure Visit"),
                Location = BuildLocation("232", "ELMHURST-DMG", 30, BuildElginAddress()),
                Provider = BuildProvider("112368", "Syed Hasan, MD"),
                Referral = BuildReferral("16620533"),
                ReferredProvider = BuildReferredProvider("9794")
            };
            yield return new RecommendedProvider
            {
                Identifier = "16620533_7650077_10031_9794_10251",
                Specialty = BuildSpecialty("10031"),
                Department = BuildDepartment("12031", "Cardiology - Spalding Dr, Naperville"),
                Visit = BuildVisit("5507", "NEW PT - CARDIOLOGY CONSULT"),
                Location = BuildLocation("120", "EDWARD PHYSICIAN MEDICAL CTR", 10, BuildNapervilleAddress()),
                Provider = BuildProvider("10251", "Mark R Ottolin"),
                Referral = BuildReferral("16620533"),
                ReferredProvider = BuildReferredProvider("9794")
            };
        }

        private static Identifier BuildExternalIdentifier(string id)
        {
            return new Identifier
            {
                Id = id,
                Type = IdentifierType.External
            };
        }

        private static Specialty BuildSpecialty(string id)
        {
            return new Specialty
            {
                Name = "CARDIOLOGY",
                Identifier = BuildExternalIdentifier(id)
            };
        }

        private static Department BuildDepartment(string id, string name)
        {
            //23231 12031 42631
            return new Department
            {
                Identifier = BuildExternalIdentifier(id),
                Name = name,
                ContactPoints = new ContactPoint[]
                {
                    new()
                    {
                        Value = "847-888-2320",
                        Type = ContactPointType.Phone
                    }
                }
            };
        }

        private static Visit BuildVisit(string visitTypeId, string name)
        {
            return new Visit
            {
                Type = new VisitType
                {
                    Name = name,
                    Identifier = BuildExternalIdentifier(visitTypeId)
                }
            };
        }

        private static Location BuildLocation(string id, string name, double distanceFromPatientHome, Address address)
        {
            return new Location
            {
                //232 120 426 407
                Identifier = BuildExternalIdentifier(id),
                Name = name,
                Address = address,
                DistanceFromPatientHome = distanceFromPatientHome
            };
        }

        private static Address BuildElginAddress()
        {
            return new Address
            {
                City = "ELGIN",
                State = "IL",
                PostalCode = "60123",
                Lines = new[]
                {
                    "87 N AIRLITE ST",
                    "SUITE 100"
                }
            };
        }

        private static Address BuildNapervilleAddress()
        {
            return new Address
            {
                City = "NAPERVILLE",
                State = "IL",
                PostalCode = null,
                Lines = new[]
                {
                    "100 SPALDING DR",
                    "SUITE 400"
                }
            };
        }

        private static Provider BuildProvider(string id, string name)
        {
            return new Provider
            {
                Identifier = BuildExternalIdentifier(id),
                Specialty = "Cardiology",
                Name = name,
                IsSlotAvailableInNext2Weeks = false
            };
        }

        private static Referral BuildReferral(string id)
        {
            return new Referral
            {
                Identifier = BuildExternalIdentifier(id)
            };
        }

        private static ReferredProvider BuildReferredProvider(string id)
        {
            return new ReferredProvider
            {
                Identifier = BuildExternalIdentifier(id)
            };
        }
    }
}