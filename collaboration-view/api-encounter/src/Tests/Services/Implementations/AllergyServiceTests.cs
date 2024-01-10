// <copyright file="AllergyServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class AllergyServiceTests
    {
        [Test]
        public async Task GetAllergiesForPatientAsyncTest()
        {
            //Arrange
            var patientId = Guid.NewGuid().ToString();
            var allergyId1 = Guid.NewGuid().ToString();
            var allergyId2 = Guid.NewGuid().ToString();
            IEnumerable<AllergyIntolerance> allergyIntolerances = new AllergyIntolerance[]
            {
                new() { Id = allergyId1, RecordedDate = new(2000, 05, 15, 0, 0, 0, default) },
                new() { Id = allergyId2, RecordedDate = new(2020, 10, 30, 0, 0, 0, default) },
            };
            IEnumerable<Allergy> allergies = new Allergy[]
            {
                new() { Id = allergyId2 },
                new() { Id = allergyId1 },
            };

            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IAllergyIntoleranceRepository>();

            repositoryMock
                .Setup(x => x.GetAllergyIntolerancesForPatientAsync(patientId, AllergyIntoleranceClinicalStatus.Active))
                .ReturnsAsync(allergyIntolerances);

            mapperMock
                .Setup(x => x.Map<IEnumerable<Allergy>>(allergyIntolerances))
                .Returns(allergies);

            var service = new AllergyService(mapperMock.Object, repositoryMock.Object);

            //Act
            var result = await service.GetAllergiesForPatientAsync(patientId);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(allergyId2);
        }
    }
}
