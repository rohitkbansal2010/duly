// <copyright file="PatientLifeGoalServiceTests.cs" company="Duly Health and Care">
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
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.CarePlan
{
    [TestFixture]
    public class PatientLifeGoalServiceTests
    {
        private Mock<IPatientLifeGoalRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        private PostRequestForLifeGoals _postRequestForLifeGoals = new PostRequestForLifeGoals()
        {
            DeletedLifeGoalIds = new List<long>() { 1, 2 },
            PatientLifeGoal = new List<PatientLifeGoal>()
            {
                new PatientLifeGoal
                {
                    CategoryName = "TestCategoryName",
                    LifeGoalDescription = "TestLifeGoalDescription",
                    LifeGoalName = "TestLifeGoalName",
                    PatientLifeGoalId = 99
                }
            },
            PatientPlanId = 1
        };

        private Models.PostOrUpdatePatientLifeGoalResponse _postOrUpdatePatientLifeGoalResponse = new Models.PostOrUpdatePatientLifeGoalResponse()
        {
            PatientLifeGoals = new List<Models.PatientLifeGoal>()
            {
                new Models.PatientLifeGoal
                {
                    CategoryName = "TestCategoryName",
                    LifeGoalDescription = "TestLifeGoalDescription",
                    LifeGoalName = "TestLifeGoalName",
                    PatientLifeGoalId = 99
                }
            },
            DeletedLifeGoalIds = "1,2",
            PatientPlanId = 1
        };

        private IEnumerable<Models.GetPatientLifeGoalByPatientPlanIdModel> _getPatientLifeGoalByPatientPlanIdResponse = new List<Models.GetPatientLifeGoalByPatientPlanIdModel>()
        {
            new Models.GetPatientLifeGoalByPatientPlanIdModel()
            {
                CategoryName = "TestCategoryName",
                LifeGoalDescription = "TestLifeGoalDescription",
                LifeGoalName = "TestLifeGoalName",
                PatientLifeGoalId = 99
            }
        };

        private IEnumerable<PatientLifeGoalTargetMapping> _postPatientLifeGoalTargetMappingRequest = new List<PatientLifeGoalTargetMapping>()
        {
           new PatientLifeGoalTargetMapping()
           {
               PatientLifeGoalId = 99,
               PatientTargetId = 5
           }
        };

        private IEnumerable<Models.GetPatientLifeGoalTargetMapping> _getPatientLifeGoalTargetMappingResponse = new List<Models.GetPatientLifeGoalTargetMapping>()
        {
           new Models.GetPatientLifeGoalTargetMapping()
           {
                LifeGoalDescription = "TestLifeGoalDescription",
                LifeGoalName = "TestLifeGoalName",
                PatientLifeGoalId = 99
           }
        };

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPatientLifeGoalRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task PostOrUpdateLifeGoalAsyncSuccessTest()
        {
            //Arrange
            var mappedRequestPatientLifeGoal = BuildRequestPatientLifeGoal(_postRequestForLifeGoals);
            var mappedResponsePatientLifeGoal = BuildResponsePatientLifeGoal(_postOrUpdatePatientLifeGoalResponse);

            _mapperMock
            .Setup(mapper => mapper.Map<Models.PostRequestForLifeGoals>(_postRequestForLifeGoals))
            .Returns(mappedRequestPatientLifeGoal);

            _repositoryMock
              .Setup(repo => repo.PostOrUpdateLifeGoalAsync(mappedRequestPatientLifeGoal, null))
              .Returns(Task.FromResult(_postOrUpdatePatientLifeGoalResponse));

            _mapperMock
            .Setup(mapper => mapper.Map<PostOrUpdatePatientLifeGoalResponse>(_postOrUpdatePatientLifeGoalResponse))
            .Returns(mappedResponsePatientLifeGoal);

            var serviceMock = new PatientLifeGoalService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.PostOrUpdateLifeGoalAsync(_postRequestForLifeGoals);

            //Assert
            _repositoryMock.Verify(x => x.PostOrUpdateLifeGoalAsync(mappedRequestPatientLifeGoal, null), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<PostOrUpdatePatientLifeGoalResponse>();
        }

        [Test]
        public async Task DeletePatientLifeGoalAsyncSuccessTest()
        {
            //Arrange
            long patientLifeGoalId = 99;

            _repositoryMock
              .Setup(repo => repo.DeletePatientLifeGoalAsync(patientLifeGoalId))
              .Returns(Task.FromResult<long>(1));

            var serviceMock = new PatientLifeGoalService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.DeletePatientLifeGoalAsync(patientLifeGoalId);

            //Assert
            _repositoryMock.Verify(x => x.DeletePatientLifeGoalAsync(patientLifeGoalId), Times.Once());

            results.Should().Be(1);
        }

        [Test]
        public async Task GetPatientLifeGoalByPatientPlanIdAsyncSuccessTest()
        {
            //Arrange
            var mappedResponsePatientLifeGoal = BuildResponseGetPatientLifeGoalByPatientPlanId(_getPatientLifeGoalByPatientPlanIdResponse);

            long id = 99;
            _repositoryMock
              .Setup(repo => repo.GetPatientLifeGoalByPatientPlanIdAsync(id))
              .Returns(Task.FromResult(_getPatientLifeGoalByPatientPlanIdResponse));

            _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<GetPatientLifeGoalByPatientPlanId>>(_getPatientLifeGoalByPatientPlanIdResponse))
            .Returns(mappedResponsePatientLifeGoal);

            var serviceMock = new PatientLifeGoalService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.GetPatientLifeGoalByPatientPlanIdAsync(id);

            //Assert
            _repositoryMock.Verify(x => x.GetPatientLifeGoalByPatientPlanIdAsync(id), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<List<GetPatientLifeGoalByPatientPlanId>>();
        }

        [Test]
        public async Task PostPatientLifeGoalTargetMappingAsyncSuccessTest()
        {
            //Arrange
            long patientTargetId = 5;
            IEnumerable<long> patientLifeGoalIds = new List<long>() { 99 };

            _repositoryMock
              .Setup(repo => repo.PostPatientLifeGoalTargetMappingAsync(patientTargetId, patientLifeGoalIds, null))
              .Returns(Task.FromResult<long>(1));

            var serviceMock = new PatientLifeGoalService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.PostPatientLifeGoalTargetMappingAsync(patientTargetId, patientLifeGoalIds);

            //Assert
            _repositoryMock.Verify(x => x.PostPatientLifeGoalTargetMappingAsync(patientTargetId, patientLifeGoalIds, null), Times.Once());

            results.Should().Be(1);
        }

        [Test]
        public async Task GetPatientLifeGoalTargetMappingByPatientIdAsyncSuccessTest()
        {
            //Arrange
            var mappedResponse = BuildResponseGetPatientLifeGoalTargetMappingByPatientId(_getPatientLifeGoalTargetMappingResponse);

            _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<GetPatientLifeGoalTargetMapping>>(_getPatientLifeGoalTargetMappingResponse))
            .Returns(mappedResponse);

            long patientTargetId = 99;
            _repositoryMock
              .Setup(repo => repo.GetPatientLifeGoalTargrtMappingByPatientIdAsync(patientTargetId))
              .Returns(Task.FromResult(_getPatientLifeGoalTargetMappingResponse));

            var serviceMock = new PatientLifeGoalService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            var results = await serviceMock.GetPatientLifeGoalTargetMappingByPatientIdAsync(patientTargetId);

            //Assert
            _repositoryMock.Verify(x => x.GetPatientLifeGoalTargrtMappingByPatientIdAsync(patientTargetId), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<List<GetPatientLifeGoalTargetMapping>>();
        }

        private Models.PostRequestForLifeGoals BuildRequestPatientLifeGoal(PostRequestForLifeGoals request)
        {
            var patientLifeGoal = request.PatientLifeGoal.ToArray();
            Models.PostRequestForLifeGoals patientPlan = new Models.PostRequestForLifeGoals()
            {
                PatientLifeGoal = new List<Models.PatientLifeGoal>()
                {
                   new Models.PatientLifeGoal
                   {
                       CategoryName = patientLifeGoal[0].CategoryName,
                       LifeGoalDescription = patientLifeGoal[0].LifeGoalDescription,
                       LifeGoalName = patientLifeGoal[0].LifeGoalName,
                       PatientLifeGoalId = patientLifeGoal[0].PatientLifeGoalId
                   }
                },
                DeletedLifeGoalIds = request.DeletedLifeGoalIds,
                PatientPlanId = 1
            };

            return patientPlan;
        }

        private PostOrUpdatePatientLifeGoalResponse BuildResponsePatientLifeGoal(Models.PostOrUpdatePatientLifeGoalResponse response)
        {
            var patientLifeGoal = response.PatientLifeGoals.ToArray();
            PostOrUpdatePatientLifeGoalResponse mappedPatientPlanResponse = new PostOrUpdatePatientLifeGoalResponse()
            {
                PatientLifeGoals = new List<PatientLifeGoal>()
                {
                   new PatientLifeGoal
                   {
                       CategoryName = patientLifeGoal[0].CategoryName,
                       LifeGoalDescription = patientLifeGoal[0].LifeGoalDescription,
                       LifeGoalName = patientLifeGoal[0].LifeGoalName,
                       PatientLifeGoalId = patientLifeGoal[0].PatientLifeGoalId
                   }
                },
                DeletedLifeGoalIds = response.DeletedLifeGoalIds
            };

            return mappedPatientPlanResponse;
        }

        private IEnumerable<GetPatientLifeGoalByPatientPlanId> BuildResponseGetPatientLifeGoalByPatientPlanId(IEnumerable<Models.GetPatientLifeGoalByPatientPlanIdModel> request)
        {
            var getPatientLifeGoalByPatientPlanId = request.ToArray();
            IEnumerable<GetPatientLifeGoalByPatientPlanId> mappedPatientPlanResponse = new List<GetPatientLifeGoalByPatientPlanId>()
            {
                new GetPatientLifeGoalByPatientPlanId()
                {
                   CategoryName = getPatientLifeGoalByPatientPlanId[0].CategoryName,
                   LifeGoalDescription = getPatientLifeGoalByPatientPlanId[0].LifeGoalDescription,
                   LifeGoalName = getPatientLifeGoalByPatientPlanId[0].LifeGoalName,
                   PatientLifeGoalId = getPatientLifeGoalByPatientPlanId[0].PatientLifeGoalId
                }
            };

            return mappedPatientPlanResponse;
        }

        private IEnumerable<Models.PatientLifeGoalTargetMapping> BuildRequestPostPatientLifeGoalTargetMapping(IEnumerable<PatientLifeGoalTargetMapping> request)
        {
            var patientLifeGoalTargetMapping = request.ToArray();
            IEnumerable<Models.PatientLifeGoalTargetMapping> mappedResponse = new List<Models.PatientLifeGoalTargetMapping>()
            {
                new Models.PatientLifeGoalTargetMapping()
                {
                  PatientLifeGoalId = patientLifeGoalTargetMapping[0].PatientLifeGoalId,
                  PatientTargetId = patientLifeGoalTargetMapping[0].PatientTargetId
                }
            };

            return mappedResponse;
        }

        private IEnumerable<GetPatientLifeGoalTargetMapping> BuildResponseGetPatientLifeGoalTargetMappingByPatientId(IEnumerable<Models.GetPatientLifeGoalTargetMapping> request)
        {
            var getPatientLifeGoalTargetMapping = request.ToArray();
            IEnumerable<GetPatientLifeGoalTargetMapping> mappedResponse = new List<GetPatientLifeGoalTargetMapping>()
            {
                new GetPatientLifeGoalTargetMapping()
                {
                  PatientLifeGoalId = getPatientLifeGoalTargetMapping[0].PatientLifeGoalId,
                  LifeGoalName = getPatientLifeGoalTargetMapping[0].LifeGoalName,
                  LifeGoalDescription = getPatientLifeGoalTargetMapping[0].LifeGoalDescription
                }
            };

            return mappedResponse;
        }
    }
}