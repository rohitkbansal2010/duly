// <copyright file="RecommendedProviderConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using System;
using System.Linq;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="AdapterModels.RecommendedProvider"/> into <see cref="RecommendedProvider"/>.
    /// </summary>
    public class RecommendedProviderConverter
        : ITypeConverter<AdapterModels.RecommendedProvider, RecommendedProvider>,
        ITypeConverter<AdapterModels.RecommendedProvider, ReferralPatient>
    {
        public RecommendedProvider Convert(AdapterModels.RecommendedProvider source, RecommendedProvider destination, ResolutionContext context)
        {
            return new RecommendedProvider
            {
                Identifier = source.Identifier,
                Referral = BuildReferral(source.ReferralId, source.ReferralIdType),
                Department = BuildDepartment(source),
                Location = BuildProviderLocation(source),
                Provider = BuildProvider(source),
                ReferredProvider = BuildReferredProvider(source.ReferredToProviderId, source.ReferredToProviderIdType),
                Specialty = BuildSpecialty(source),
                Visit = BuildVisit(source)
            };
        }

        public ReferralPatient Convert(AdapterModels.RecommendedProvider source, ReferralPatient destination, ResolutionContext context)
        {
            return new ReferralPatient
            {
                Patient = BuildPatient(source),
                Address = BuildPatientAddress(source),
                Name = BuildPatientName(source),
                DateOfBirth = BuildPatientDateOfBirth(source),
                ContactPoints = BuildPatientContactPoints(source)
            };
        }

        private static Identifier BuildIdentifier(string id, string idType)
        {
            return new Identifier
            {
                Id = id,
                Type = BuildIdentifierType(idType)
            };
        }

        private static IdentifierType BuildIdentifierType(string type)
        {
            return type switch
            {
                "External" => IdentifierType.External,
                _ => IdentifierType.Unspecified
            };
        }

        private static Specialty BuildSpecialty(AdapterModels.RecommendedProvider source)
        {
            return new Specialty
            {
                Name = source.SpecialtyName,
                Identifier = BuildIdentifier(source.SpecialtyId, source.SpecialtyIdType)
            };
        }

        private static Department BuildDepartment(AdapterModels.RecommendedProvider source)
        {
            return new Department
            {
                Identifier = BuildIdentifier(source.DepartmentId.ToString(), source.DepartmentIdType),
                Name = source.DepartmentName,
                ContactPoints = BuildPhoneContact(source.DepartmentPhone)
            };
        }

        private static ContactPoint[] BuildPatientContactPoints(AdapterModels.RecommendedProvider source)
        {
            return BuildPhoneContact(source.PatientPhone);
        }

        private static Visit BuildVisit(AdapterModels.RecommendedProvider source)
        {
            return new Visit
            {
                Type = new VisitType
                {
                    Name = source.VisitTypeName,
                    Identifier = BuildIdentifier(source.VisitTypeId.ToString(), source.VisitTypeIdType)
                }
            };
        }

        private static Location BuildProviderLocation(AdapterModels.RecommendedProvider source)
        {
            return new Location
            {
                Identifier = BuildIdentifier(source.LocationId.ToString(), source.LocationIdType),
                Name = source.LocationName,
                Address = BuildAddress(
                    source.LocationCity,
                    source.LocationState,
                    source.LocationZip,
                    source.LocationAddressFirstLine,
                    source.LocationAddressSecondLine),
                DistanceFromPatientHome = source.LocationDistanceFromPatientHome
            };
        }

        private static Address BuildPatientAddress(AdapterModels.RecommendedProvider source)
        {
            return BuildAddress(
                source.PatientCity,
                source.PatientState,
                source.PatientZip,
                source.PatientAddressFirstLine,
                source.PatientAddressSecondLine);
        }

        private static Address BuildAddress(string city, string state, string postalCode, params string[] addressLines)
        {
            return new Address
            {
                City = city,
                State = state,
                PostalCode = postalCode,
                Lines = addressLines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray()
            };
        }

        private static Provider BuildProvider(AdapterModels.RecommendedProvider source)
        {
            return new Provider
            {
                Identifier = BuildIdentifier(source.ProviderId, source.ProviderIdType),
                Specialty = source.ProviderSpecialty,
                Name = source.ProviderName,
                IsSlotAvailableInNext2Weeks = source.IsSlotAvailableInNextTwoWeeks.ConvertDbStringToBool()
            };
        }

        private static Patient BuildPatient(AdapterModels.RecommendedProvider source)
        {
            return new Patient
            {
                Id = source.PatientId
            };
        }

        private static Referral BuildReferral(decimal? id, string idType)
        {
            return new Referral
            {
                Identifier = BuildIdentifier(id.ToString(), idType)
            };
        }

        private static ReferredProvider BuildReferredProvider(string id, string idType)
        {
            return id == null ? null : new ReferredProvider
            {
                Identifier = BuildIdentifier(id, idType)
            };
        }

        private static HumanName BuildPatientName(AdapterModels.RecommendedProvider source)
        {
            return new HumanName
            {
                FamilyName = source.PatientLastName,
                GivenNames = source.PatientFirstName is null ? Array.Empty<string>() : new[] { source.PatientFirstName }
            };
        }

        private static DateTime BuildPatientDateOfBirth(AdapterModels.RecommendedProvider source)
        {
            return source.PatientDateOfBirth.GetValueOrDefault();
        }

        private static ContactPoint[] BuildPhoneContact(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return Array.Empty<ContactPoint>();
            }

            return new ContactPoint[]
            {
                new()
                {
                    Value = phone,
                    Type = ContactPointType.Phone
                }
            };
        }
    }
}
