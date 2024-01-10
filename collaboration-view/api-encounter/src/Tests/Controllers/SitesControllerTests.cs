// -----------------------------------------------------------------------
// <copyright file="SitesControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Controllers;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Infrastructure.Exceptions;
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

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    public class SitesControllerTests
    {
        private const string TestSiteId = "test-site-id";

        private SitesController _controller;
        private Mock<ISiteService> _siteServiceMock;

        [SetUp]
        public void SetUp()
        {
            var loggerMock = new Mock<ILogger<SitesController>>();
            _siteServiceMock = new Mock<ISiteService>();
            _siteServiceMock.Setup(x => x.GetSitesAsync())
                .ReturnsAsync(new List<Site> { new () { Id = TestSiteId } });
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _controller = new SitesController(_siteServiceMock.Object, loggerMock.Object, iWebHostEnvironment.Object);

            _controller.MockObjectValidator();
        }

        [Test]
        public void GetSitesTest()
        {
            //Arrange
            ActionResult<IEnumerable<Site>> result = null;

            //Act
            Func<Task> act = async () => result = await _controller.GetSites();
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<Site>;
            contentResult.Should().NotBeNullOrEmpty();
            contentResult.FirstOrDefault().Id.Should().Be(TestSiteId, "This is the one that is created in setup");
        }

        [Test]
        public void GetSiteTest_OkResult()
        {
            //Arrange
            ActionResult<Site> result = null;
            _siteServiceMock.Setup(x => x.GetSiteAsync(It.IsAny<string>()))
                .ReturnsAsync(new Site { Id = TestSiteId });

            //Act
            Func<Task> act = async () => result = await _controller.GetSiteById(TestSiteId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as Site;
            contentResult.Id.Should().Be(TestSiteId);
        }

        [Test]
        public void GetSiteTest_NotFound()
        {
            //Arrange
            ActionResult<Site> result = null;

            //Act
            Func<Task> act = async () => result = await _controller.GetSiteById(TestSiteId);
            act.Invoke();

            //Assert
            act.Should().ThrowAsync<EntityNotFoundException>();
        }
    }
}
