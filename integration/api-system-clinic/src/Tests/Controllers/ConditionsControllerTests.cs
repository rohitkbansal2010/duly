// <copyright file="ConditionsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Controllers;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Tests.Controllers
{
    [TestFixture]
    public class ConditionsControllerTests
    {
        private Mock<IConditionRepository> _conditionRepositoryMocked;
        private Mock<ILogger<ConditionsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _conditionRepositoryMocked = new Mock<IConditionRepository>();
            _loggerMocked = new Mock<ILogger<ConditionsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task FindConditionsForPatientTest()
        {
            //Arrange
            const string patientId = "test";
            ConditionClinicalStatus[] clinicalStatusArray = new[]
            {
                ConditionClinicalStatus.Active,
                ConditionClinicalStatus.Inactive,
                ConditionClinicalStatus.Resolved
            };
            var conditions = ConfigureRepository(patientId, clinicalStatusArray);
            var controller = new ConditionsController(_loggerMocked.Object, _conditionRepositoryMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.FindConditionsForPatient(patientId, clinicalStatusArray);

            //Assert
            _conditionRepositoryMocked.Verify(
                x => x.FindProblemsForPatientAsync(
                    patientId,
                    It.Is<ConditionClinicalStatus[]>(
                        p => p.Length == clinicalStatusArray.Length
                             && p[0] == ConditionClinicalStatus.Active
                             && p[1] == ConditionClinicalStatus.Inactive
                             && p[2] == ConditionClinicalStatus.Resolved)),
                Times.Once());

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Condition>)((OkObjectResult)actionResult.Result).Value;

            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(conditions.Count());
            var firstItem = responseData.First();
            firstItem.Id.Should().Be(conditions.First().Id);
        }

        private IEnumerable<Condition> ConfigureRepository(
            string patientId,
            ConditionClinicalStatus[] clinicalStatusArray)
        {
            var conditions = new List<Condition>
            {
                new Condition
                {
                    Id = Guid.NewGuid().ToString()
                },
                new Condition
                {
                    Id = Guid.NewGuid().ToString()
                },
                new Condition
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _conditionRepositoryMocked
                .Setup(repository => repository.FindProblemsForPatientAsync(patientId, clinicalStatusArray))
                .Returns(Task.FromResult(conditions.AsEnumerable()));

            return conditions;
        }
    }
}