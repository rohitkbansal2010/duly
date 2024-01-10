// <copyright file="ResourceContextTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Resource.Api.Contexts;
using Duly.CollaborationView.Resource.Api.Contexts.Implementations;
using Duly.Common.Infrastructure.Constants;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;

namespace Duly.Resource.Api.Tests.Contexts.Implementations
{
    [TestFixture]
    public class ResourceContextTests
    {
        private HttpContext _fakeHttpContext;
        private Mock<IHttpContextAccessor> _accessorMock;

        [SetUp]
        public void SetUp()
        {
            ConfigureContextAccessor();
        }

        [Test]
        public void GetXCorrelationIdTest_WithXCorrelationIdHeader()
        {
            //Arrange
            var requestXCorrelationId = Guid.NewGuid();
            _fakeHttpContext.Response.Headers.Add(ParameterNames.XCorrelationIdHeader, requestXCorrelationId.ToString());
            var resourceContextMocked = new ResourceContext(_accessorMock.Object);

            //Act
            var results = resourceContextMocked.GetXCorrelationId();

            //Assert
            results.Should().Be(requestXCorrelationId);
        }

        [Test]
        public void GetXCorrelationIdTest_ThrowExceptionWithoutXCorrelationIdHeader()
        {
            //Arrange
            var resourceContextMocked = new ResourceContext(_accessorMock.Object);

            //Act
            Action action = () => resourceContextMocked.GetXCorrelationId();

            //Assert
            var result = action.Should().ThrowExactly<ResourceContextException>();
            result.Which.Message.Should().Be("The request does not contain 'x-correlation-id'.");
        }

        private void ConfigureContextAccessor()
        {
            _accessorMock = new Mock<IHttpContextAccessor>();

            _fakeHttpContext = new DefaultHttpContext().HttpContext;
            _accessorMock
                .Setup(accessor => accessor.HttpContext)
                .Returns(_fakeHttpContext);
        }
    }
}