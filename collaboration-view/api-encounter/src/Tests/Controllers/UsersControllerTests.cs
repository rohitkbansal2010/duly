// <copyright file="UsersControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Controllers;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private const string UserId = "test-user-id";

        private Mock<ILogger<UsersController>> _loggerMock;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<UsersController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public void GetActiveUserTest()
        {
            //Arrange
            ActionResult<User> result = null;
            var controller = new UsersController(
                SetupUserServiceForGetActiveUserAsync(),
                _loggerMock.Object,
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.GetActiveUser();
            act.Invoke();

            //Assert
            act.Should()
                .NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult?.Value as User;

            contentResult.Should().NotBeNull();
            contentResult.Id.Should().Be(UserId);
            contentResult.Name.Should().NotBeNull();
            contentResult.Name.FamilyName.Should().Be("Wood");
        }

        private static IUserService SetupUserServiceForGetActiveUserAsync()
        {
            var serviceMock = new Mock<IUserService>();

            serviceMock
                .Setup(x => x.GetActiveUserAsync())
                .Returns(Task.FromResult(new User
                {
                    Id = UserId,
                    Name = new HumanName
                    {
                        FamilyName = "Wood",
                        GivenNames = new[] { "John", "Henry" }
                    },
                }));

            return serviceMock.Object;
        }
    }
}
