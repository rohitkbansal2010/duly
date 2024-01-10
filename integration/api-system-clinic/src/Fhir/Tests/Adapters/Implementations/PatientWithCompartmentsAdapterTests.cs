// <copyright file="PatientWithCompartmentsAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Hl7.Fhir.Rest;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class PatientWithCompartmentsAdapterTests
    {
        private const string PatientId = "testId";

        private readonly Mock<IFhirClientR4> _mockedClient = new();
        private readonly Mock<IPatientWithCompartmentsBuilder> _mockedBuilder = new();
        private Mock<IPrivateEpicCall> _privateEpicCall;
        [Test]
        public async Task FindPatientByIdAsync_Null_Test()
        {
            //Arrange
            _privateEpicCall = new Mock<IPrivateEpicCall>();
            IPatientWithCompartmentsAdapter adapter = new PatientWithCompartmentsAdapter(_mockedClient.Object, null, _privateEpicCall.Object);

            //Act
            var result = await adapter.FindPatientByIdAsync(PatientId);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task FindPatientByIdAsync_Success_Test()
        {
            //Arrange
            _privateEpicCall = new Mock<IPrivateEpicCall>();
            var components = SetUpClient();
            SetUpBuilder(components);

            IPatientWithCompartmentsAdapter adapter = new PatientWithCompartmentsAdapter(_mockedClient.Object, _mockedBuilder.Object, _privateEpicCall.Object);

            //Act
            var result = await adapter.FindPatientByIdAsync(PatientId);

            //Assert
            result.Should().NotBeNull();
            result.Resource.Should().Be(components[0].Resource);
        }

        [Test]
        public async Task FindPatientsByIdentifiersAsyncTest()
        {
            //Arrange
            _privateEpicCall = new Mock<IPrivateEpicCall>();
            var identifiers = new[] { "EXTERNAL|7650082", "EXTERNAL|7650074" };
            var components = SetUpClient();

            _mockedBuilder
                .Setup(builder => builder.ExtractPatientsWithCompartments(components))
                .Returns(new PatientWithCompartments[]
                {
                    new()
                    {
                        Resource = (R4.Patient)components[0].Resource
                    }
                });

            IPatientWithCompartmentsAdapter adapter = new PatientWithCompartmentsAdapter(_mockedClient.Object, _mockedBuilder.Object, _privateEpicCall.Object);

            //Act
            var result = await adapter.FindPatientsByIdentifiersAsync(identifiers);

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Should().NotBeNull();
            result.First().Resource.Should().Be(components[0].Resource);
        }

        private R4.Bundle.EntryComponent[] SetUpClient()
        {
            var components = new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.Patient
                    {
                        Id = PatientId
                    }
                }
            };
            _mockedClient
                .Setup(client => client.FindPatientsAsync(It.IsAny<SearchParams>()))
                .Returns(
                    Task.FromResult(components));

            return components;
        }

        private void SetUpBuilder(IReadOnlyList<R4.Bundle.EntryComponent> components)
        {
            _mockedBuilder
                .Setup(builder => builder.ExtractPatientWithCompartments(components))
                .Returns(new PatientWithCompartments
                {
                    Resource = (R4.Patient)components[0].Resource
                });
        }
    }
}