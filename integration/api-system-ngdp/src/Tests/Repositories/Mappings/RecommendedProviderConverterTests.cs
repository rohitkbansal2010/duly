// <copyright file="RecommendedProviderConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Api.Configurations;
using Duly.Ngdp.Api.Repositories.Mappings;
using Duly.Ngdp.Api.Tests.Common;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Duly.Ngdp.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class RecommendedProviderConverterTests : MapperConfigurator<NgdpToSystemApiContractsProfile>
    {
        protected override object ConstructService(Type serviceType)
        {
            if (serviceType == typeof(RecommendedProviderConverter))
            {
                return new RecommendedProviderConverter();
            }

            throw new InvalidOperationException($"Can not create instance of type {serviceType}");
        }

        [TestCase(1234, "External", IdentifierType.External)]
        [TestCase(124155612, "something else", IdentifierType.Unspecified)]
        public void Convert_ToRecommendedProvider_Identifiers_Test(int id, string idType, IdentifierType expectedType)
        {
            //Arrange
            var source = new Adapter.Adapters.Models.RecommendedProvider
            {
                ReferralId = id,
                DepartmentId = id,
                LocationId = id,
                ProviderId = id.ToString(),
                ReferredToProviderId = id.ToString(),
                SpecialtyId = id.ToString(),
                VisitTypeId = id,
                ReferralIdType = idType,
                DepartmentIdType = idType,
                LocationIdType = idType,
                ProviderIdType = idType,
                ReferredToProviderIdType = idType,
                SpecialtyIdType = idType,
                VisitTypeIdType = idType
            };

            //Act
            var result = Mapper.Map<RecommendedProvider>(source);

            //Assert

            result.Should().NotBeNull();
            result.Referral.Identifier.Id.Should().Be(source.ReferralId.ToString());
            result.Referral.Identifier.Type.Should().Be(expectedType);
            result.Department.Identifier.Id.Should().Be(source.DepartmentId.ToString());
            result.Department.Identifier.Type.Should().Be(expectedType);
            result.Location.Identifier.Id.Should().Be(source.LocationId.ToString());
            result.Location.Identifier.Type.Should().Be(expectedType);
            result.Provider.Identifier.Id.Should().Be(source.ProviderId);
            result.Provider.Identifier.Type.Should().Be(expectedType);
            result.ReferredProvider.Identifier.Id.Should().Be(source.ReferredToProviderId);
            result.ReferredProvider.Identifier.Type.Should().Be(expectedType);
            result.Specialty.Identifier.Id.Should().Be(source.SpecialtyId);
            result.Specialty.Identifier.Type.Should().Be(expectedType);
            result.Visit.Type.Identifier.Id.Should().Be(source.VisitTypeId.ToString());
            result.Visit.Type.Identifier.Type.Should().Be(expectedType);
        }

        [Test]
        public void Convert_ToRecommendedProvider_Department_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.RecommendedProvider
            {
                DepartmentName = "test-name",
                DepartmentPhone = "+123-456"
            };

            //Act
            var result = Mapper.Map<RecommendedProvider>(source);

            //Assert

            result.Should().NotBeNull();
            result.Department.Name.Should().Be(source.DepartmentName);
            result.Department.ContactPoints[0].Type.Should().Be(ContactPointType.Phone);
            result.Department.ContactPoints[0].Value.Should().Be(source.DepartmentPhone);
        }

        [Test]
        public void Convert_ToRecommendedProvider_ProviderLocation_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.RecommendedProvider
            {
                LocationName = "test-name",
                LocationCity = "test-city",
                LocationState = "test-state",
                LocationZip = "12345",
                LocationAddressFirstLine = "first line 1",
                LocationAddressSecondLine = "second line 2",
                LocationDistanceFromPatientHome = 1
            };

            //Act
            var result = Mapper.Map<RecommendedProvider>(source);

            //Assert

            result.Should().NotBeNull();
            result.Location.Name.Should().Be(source.LocationName);
            result.Location.Address.City.Should().Be(source.LocationCity);
            result.Location.Address.State.Should().Be(source.LocationState);
            result.Location.Address.PostalCode.Should().Be(source.LocationZip);
            result.Location.Address.Lines.Should().BeEquivalentTo( new[] { source.LocationAddressFirstLine, source.LocationAddressSecondLine });
            result.Location.DistanceFromPatientHome.Should().Be(source.LocationDistanceFromPatientHome);
        }

        [TestCase("Y", true)]
        [TestCase("y", true)]
        [TestCase("N", false)]
        [TestCase("n", false)]
        [TestCase("asdf", false)]
        public void Convert_ToRecommendedProvider_Provider_Test(string slotsAvailable, bool isSlotAvailableExpected)
        {
            //Arrange
            var source = new Adapter.Adapters.Models.RecommendedProvider
            {
                ProviderName = "test-name",
                ProviderSpecialty = "test-spec",
                IsSlotAvailableInNextTwoWeeks = slotsAvailable
            };

            //Act
            var result = Mapper.Map<RecommendedProvider>(source);

            //Assert

            result.Should().NotBeNull();
            result.Provider.Name.Should().Be(source.ProviderName);
            result.Provider.Specialty.Should().Be(source.ProviderSpecialty);
            result.Provider.IsSlotAvailableInNext2Weeks.Should().Be(isSlotAvailableExpected);
        }

        [Test]
        public void Convert_ToReferralPatient_Test()
        {
            //Arrange
            var source = new Adapter.Adapters.Models.RecommendedProvider
            {
                PatientId = "101",
                PatientCity = "ELGIN",
                PatientState = "IL",
                PatientZip = "60123",
                PatientAddressFirstLine = "87 N AIRLITE ST",
                PatientAddressSecondLine = "SUITE 100",
                PatientLastName = "Fitzgerald",
                PatientFirstName = "Michael E",
                PatientDateOfBirth = new DateTime(1950, 03, 14),
                PatientPhone = "847-888-2320"
            };

            //Act
            var result = Mapper.Map<ReferralPatient>(source);

            //Assert
            result.Should().NotBeNull();
            result.Patient.Id.Should().Be(source.PatientId);
            result.Address.City.Should().Be(source.PatientCity);
            result.Address.State.Should().Be(source.PatientState);
            result.Address.PostalCode.Should().Be(source.PatientZip);
            result.Address.Lines[0].Should().Be(source.PatientAddressFirstLine);
            result.Address.Lines[1].Should().Be(source.PatientAddressSecondLine);
            result.Name.FamilyName.Should().Be(source.PatientLastName);
            result.Name.GivenNames.First().Should().Be(source.PatientFirstName);
            result.DateOfBirth.Should().Be(source.PatientDateOfBirth);
            result.ContactPoints.First().Value.Should().Be(source.PatientPhone);
            result.ContactPoints.First().Type.Should().Be(ContactPointType.Phone);
        }
    }
}
