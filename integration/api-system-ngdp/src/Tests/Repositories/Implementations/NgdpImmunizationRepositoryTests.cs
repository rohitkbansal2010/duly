// <copyright file="NgdpImmunizationRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Implementations;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class NgdpImmunizationRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IImmunizationAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IImmunizationAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetImmunizationsForSpecificPatientAsync_Test()
        {
            //Arrange
            const string patientId = "309";
            var includedDueStatuses = new[] { DueStatus.DueOn, DueStatus.DueSoon };

            var immunizations = SetupAdapter(patientId, includedDueStatuses);
            var systemImmunizations = SetupMapper(immunizations);

            var repository = new NgdpImmunizationRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var result = await repository.GetImmunizationsForSpecificPatientAsync(patientId, includedDueStatuses);

            //Assert
            result.Should().BeEquivalentTo(systemImmunizations);
        }

        private IEnumerable<AdapterModels.Immunization> SetupAdapter(string patientId, DueStatus[] includedDueStatuses)
        {
            IEnumerable<AdapterModels.Immunization> immunizations = new AdapterModels.Immunization[]
            {
                new()
            };

            _adapterMocked
                .Setup(adapter => adapter.FindImmunizationsAsync(
                    It.Is<AdapterModels.ImmunizationSearchCriteria>(criteria =>
                        criteria.PatientId == patientId
                        && criteria.IncludedDueStatusesIds.Length == includedDueStatuses.Length)))
                .ReturnsAsync(immunizations);

            return immunizations;
        }

        private IEnumerable<Immunization> SetupMapper(IEnumerable<AdapterModels.Immunization> appointments)
        {
            IEnumerable<Immunization> systemImmunizations = new Immunization[]
            {
                new()
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Immunization>>(appointments))
                .Returns(systemImmunizations);

            return systemImmunizations;
        }
    }
}