// <copyright file="PractitionerWithRolesBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;
using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Support;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Builders.Implementations
{
    [TestFixture]
    public class PractitionerWithRolesBuilderTests
    {
        private const string PractitionerId = "practitionerId";

        private Mock<IFhirClientR4> _mockedClientR4;
        private Mock<IFhirClientSTU3> _mockedClientSTU3;
        private Mock<ILogger<PractitionerWithRolesBuilder>> _mockedlogger;

        [SetUp]
        public void SetUp()
        {
            _mockedClientR4 = new Mock<IFhirClientR4>();
            _mockedClientSTU3 = new Mock<IFhirClientSTU3>();
            _mockedlogger = new Mock<ILogger<PractitionerWithRolesBuilder>>();
        }

        [Test]
        public void ExtractPractitionerWithRoles_R4_Test()
        {
            //Arrange
            var components = SetR4Components(PractitionerId);
            IPractitionerWithRolesBuilder builder = new PractitionerWithRolesBuilder(_mockedClientR4.Object, _mockedClientSTU3.Object, _mockedlogger.Object);

            //Act
            var result = builder.ExtractPractitionerWithRoles(components);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
            result.First().Resource.Id.Should().Be(PractitionerId);
            result.First().Roles.Should().NotBeEmpty();
            result.First().Roles.Should().HaveCount(1);
            result.First().Roles.First().Practitioner.Reference.Should()
                .Be(nameof(R4.Practitioner) + "/" + PractitionerId);
        }

        [Test]
        public async Task RetrievePractitionerWithRolesAsync_R4_Test()
        {
            //Arrange
            var components = SetR4Components(PractitionerId);
            SetupR4FetchPractitionerRolesAsync(PractitionerId);

            IPractitionerWithRolesBuilder builder = new PractitionerWithRolesBuilder(_mockedClientR4.Object, _mockedClientSTU3.Object, _mockedlogger.Object);

            //Act
            var result = await builder.RetrievePractitionerWithRolesAsync(components, true);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Resource.Id.Should().Be(PractitionerId);
            result.First().Roles.Should().NotBeEmpty();
            result.First().Roles.Should().HaveCount(1);
            result.First().Roles.First().Practitioner.Reference.Should()
                .Be(nameof(R4.Practitioner) + "/" + PractitionerId);
        }

        [Test]
        public async Task RetrievePractitionerWithRolesSafeAsync_R4_Test()
        {
            //Arrange
            var components = SetR4Components(PractitionerId);
            SetupR4FetchPractitionerRolesToThrowAsync();

            IPractitionerWithRolesBuilder builder = new PractitionerWithRolesBuilder(_mockedClientR4.Object, _mockedClientSTU3.Object, _mockedlogger.Object);

            //Act
            var result = await builder.RetrievePractitionerWithRolesSafeAsync(components, true);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Resource.Id.Should().Be(PractitionerId);
            _mockedlogger.Invocations[0].Arguments[3].Should().BeOfType<ResourceReferenceNotFoundException>();
            result.First().Roles.Should().BeEmpty();
        }

        [Test]
        public async Task RetrievePractitionerWithRolesAsync_STU3_Test()
        {
            //Arrange
            var components = SetSTU3Components(PractitionerId);
            SetupSTU3FetchPractitionerRolesAsync(PractitionerId);
            IPractitionerWithRolesBuilder builder = new PractitionerWithRolesBuilder(_mockedClientR4.Object, _mockedClientSTU3.Object, _mockedlogger.Object);

            //Act
            var result = await builder.RetrievePractitionerWithRolesAsync(components);
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Resource.Id.Should().Be(PractitionerId);
            result.First().Roles.Should().NotBeEmpty();
            result.First().Roles.Should().HaveCount(1);
            result.First().Roles.First().Practitioner.Reference.Should()
                .Be(nameof(R4.Practitioner) + "/" + PractitionerId);
        }

        [Test]
        public async Task RetrievePractitionerWithRolesAsync_WithoutPractitionersRoles_STU3_Test()
        {
            //Arrange
            var components = Array.Empty<STU3.Bundle.EntryComponent>();

            IPractitionerWithRolesBuilder builder = new PractitionerWithRolesBuilder(_mockedClientR4.Object, _mockedClientSTU3.Object, _mockedlogger.Object);

            //Act
            var result = await builder.RetrievePractitionerWithRolesAsync(components);

            //Assert
            result.Should().BeEmpty();
        }

        [Test]
        public async Task RetrievePractitionerWithRolesAsync_WithoutPractitionersRoles_R4_Test()
        {
            //Arrange
            var components = Array.Empty<R4.Bundle.EntryComponent>();

            IPractitionerWithRolesBuilder builder = new PractitionerWithRolesBuilder(_mockedClientR4.Object, _mockedClientSTU3.Object, _mockedlogger.Object);

            //Act
            var result = await builder.RetrievePractitionerWithRolesAsync(components);

            //Assert
            result.Should().BeEmpty();
        }

        private static R4.Bundle.EntryComponent[] SetR4Components(string practitionerId)
        {
            return new R4.Bundle.EntryComponent[]
            {
                new ()
                {
                    Resource = new R4.Practitioner
                    {
                        Id = practitionerId,
                        Active = true
                    }
                },
                new ()
                {
                    Resource = new R4.Practitioner
                    {
                        Id = Guid.NewGuid().ToString(),
                        Active = false
                    }
                },
                new ()
                {
                    Resource = new R4.PractitionerRole
                    {
                        Practitioner = new ResourceReference { Reference = nameof(R4.Practitioner) + "/" + practitionerId }
                    }
                }
            };
        }

        private static STU3.Bundle.EntryComponent[] SetSTU3Components(string practitionerId)
        {
            return new STU3.Bundle.EntryComponent[]
            {
                new ()
                {
                    Resource = new STU3.Practitioner
                    {
                        Id = practitionerId,
                        Active = true
                    }
                },
                new ()
                {
                    Resource = new STU3.PractitionerRole
                    {
                        Practitioner = new ResourceReference { Reference = nameof(STU3.Practitioner) + "/" + practitionerId }
                    }
                }
            };
        }

        private void SetupR4FetchPractitionerRolesAsync(string practitionerId)
        {
            var practitionerRoles = new R4.Bundle.EntryComponent[]
            {
                new ()
                {
                    Resource = new R4.PractitionerRole
                    {
                        Practitioner = new ResourceReference { Reference = nameof(R4.Practitioner) + "/" + practitionerId }
                    }
                }
            };

            _mockedClientR4
                .Setup(client => client.FindPractitionerRolesAsync(It.IsAny<SearchParams>()))
                .Returns(Task.FromResult(practitionerRoles));
        }

        private void SetupR4FetchPractitionerRolesToThrowAsync()
        {
            _mockedClientR4
                .Setup(client => client.FindPractitionerRolesAsync(It.IsAny<SearchParams>()))
                .Throws(new ResourceReferenceNotFoundException("url", "message"));
        }

        private void SetupSTU3FetchPractitionerRolesAsync(string practitionerId)
        {
            var practitionerRoles = new STU3.Bundle.EntryComponent[]
            {
                new ()
                {
                    Resource = new STU3.PractitionerRole
                    {
                        Practitioner = new ResourceReference { Reference = nameof(STU3.Practitioner) + "/" + practitionerId }
                    }
                }
            };

            _mockedClientSTU3
                .Setup(client => client.FindPractitionerRolesAsync(It.IsAny<SearchParams>()))
                .Returns(Task.FromResult(practitionerRoles));
        }
    }
}
