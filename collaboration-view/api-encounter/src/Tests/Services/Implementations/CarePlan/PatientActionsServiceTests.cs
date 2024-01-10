// <copyright file="PatientActionsServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class PatientActionsServiceTests
    {
        private Mock<IPatientActionsRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private IDbTransaction _transaction = null;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPatientActionsRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task PostPatientActionsSuccessTests()
        {
            //Arrange
          IEnumerable<PatientActions> _patientActions = new PatientActions[]
          {
            new()
            {
               PatientTargetId = 2,
               ActionId = 1,
               PatientActionId = 9,
               CustomActionId = 8,
               Deleted = false
            }
          };

          IEnumerable<Api.Repositories.Models.CarePlan.PatientActions> _mappedPatientActions = new Api.Repositories.Models.CarePlan.PatientActions[]
          {
            new()
            {
                PatientTargetId = 2,
                ActionId = 1,
                PatientActionId = 9,
                CustomActionId = 8,
                Deleted = false
            }
          };

          long res = 2;

          _repositoryMock
                .Setup(repo => repo.PostPatientActionsAsync(_mappedPatientActions, _transaction))
                .Returns(Task.FromResult(res));

          _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<Api.Repositories.Models.CarePlan.PatientActions>>(_patientActions))
                .Returns(_mappedPatientActions);

          var serviceMock = new PatientActionsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
          var results = await serviceMock.PostPatientActionsAsync(_patientActions);

            //Assert
          _repositoryMock.Verify(x => x.PostPatientActionsAsync(_mappedPatientActions, _transaction), Times.Once());

          results.Should().NotBe(0);
        }

        public async Task GetPatientActionsByPatientTargetIdAsyncSuccessTests()
        {
            //Arrange

            IEnumerable<Api.Repositories.Models.CarePlan.GetPatientActions> _patientActions = new Api.Repositories.Models.CarePlan.GetPatientActions[]
            {
                new()
                {
                    PatientActionId = 2,
                    ActionId = 1,
                    ActionName = "Test action",
                    Description = "Test description",
                    IsSelected = false,
                    CustomActionId = 6
                }
            };

            IEnumerable<GetPatientActions> _mappedPatientActions = new GetPatientActions[]
            {
                new()
                {
                    PatientActionId = 2,
                    ActionId = 1,
                    ActionName = "Test action",
                    Description = "Test description",
                    IsSelected = false,
                    CustomActionId = 6
                }
            };

            long res = 2;

            _repositoryMock
                  .Setup(repo => repo.GetPatientActionsByPatientTargetIdAsync(res))
                  .Returns(Task.FromResult(_patientActions));

            _mapperMock
                  .Setup(mapper => mapper.Map<IEnumerable<GetPatientActions>>(_patientActions))
                  .Returns(_mappedPatientActions);

            var serviceMock = new PatientActionsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.GetPatientActionsByPatientTargetIdAsync(res);

            //Assert
            _repositoryMock.Verify(x => x.GetPatientActionsByPatientTargetIdAsync(res), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<List<PatientActions>>();
        }

        public async Task UpdateActionProgressAsyncSuccessTests()
        {
            //Arrange
            long PatientActionId = 5;
            var _PatientActionUpdate = new UpdateActionProgress()
            {
              Notes = "Test notes",
              Progress = 8
            };

            var _MappedPatientActionUpdate = new Api.Repositories.Models.CarePlan.UpdateActionProgress()
            {
                Notes = "Test notes",
                Progress = 8
            };

            long res = 2;

            _repositoryMock
                  .Setup(repo => repo.UpdateActionProgressAsync(_MappedPatientActionUpdate, PatientActionId))
                  .Returns(Task.FromResult(res));

            _mapperMock
                  .Setup(mapper => mapper.Map<Api.Repositories.Models.CarePlan.UpdateActionProgress>(_PatientActionUpdate))
                  .Returns(_MappedPatientActionUpdate);

            var serviceMock = new PatientActionsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.UpdateActionProgressAsync(_PatientActionUpdate, PatientActionId);

            //Assert
            _repositoryMock.Verify(x => x.GetPatientActionsByPatientTargetIdAsync(res), Times.Once());

            results.Should().NotBe(0);
        }
    }
}