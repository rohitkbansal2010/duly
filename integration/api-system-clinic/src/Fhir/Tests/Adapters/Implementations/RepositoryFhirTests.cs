// <copyright file="RepositoryFhirTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class RepositoryFhirTests
    {
        private const string EncounterId = "test";
        private Mock<IFhirClientR4> _mockedIFhirClientR4;

        [SetUp]
        public void SetUp()
        {
            _mockedIFhirClientR4 = new Mock<IFhirClientR4>();
        }

        [Test]
        public async Task GetFhirResourceByIdAsync_ReturnNull_Tests()
        {
            //Arrange

            var repositoryFhir = new RepositoryFhir<R4.Encounter>(_mockedIFhirClientR4.Object);
            SetupClientR4();

            //Act
            var result = await repositoryFhir.GetFhirResourceByIdAsync(EncounterId);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetFhirResourceByIdAsync_Test()
        {
            //Arrange

            var repositoryFhir = new RepositoryFhir<R4.Encounter>(_mockedIFhirClientR4.Object);
            var encounter = new R4.Encounter();
            SetupClientR4(encounter);

            //Act
            var result = await repositoryFhir.GetFhirResourceByIdAsync(EncounterId);

            //Assert
            result.Should().Be(encounter);
        }

        private void SetupClientR4(R4.Encounter encounter = null)
        {
            var entry = new List<R4.Bundle.EntryComponent>();
            if (encounter != null)
            {
                entry.Add(new R4.Bundle.EntryComponent
                {
                    Resource = encounter
                });
            }

            _mockedIFhirClientR4
                .Setup(clientR4 => clientR4.SearchByIdAsync<R4.Encounter>(EncounterId))
                .Returns(() => Task.FromResult(
                    new R4.Bundle
                    {
                        Entry = entry
                    }));
        }
    }
}
