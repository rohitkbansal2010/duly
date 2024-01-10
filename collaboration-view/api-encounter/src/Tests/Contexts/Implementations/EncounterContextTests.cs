﻿// <copyright file="EncounterContextTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contexts;
using Duly.CollaborationView.Encounter.Api.Contexts.Implementations;
using Duly.Common.Infrastructure.Constants;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;

namespace Duly.CollaborationView.Encounter.Api.Tests.Contexts.Implementations
{
    [TestFixture]
    public class EncounterContextTests
    {
        private HttpContext _fakeHttpContext;
        private Mock<IHttpContextAccessor> _accessorMocked;

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
            var encounterContextMocked = new EncounterContext(_accessorMocked.Object);

            //Act
            var results = encounterContextMocked.GetXCorrelationId();

            //Assert
            results.Should().Be(requestXCorrelationId);
        }

        [Test]
        public void GetXCorrelationIdTest_ThrowExceptionWithoutXCorrelationIdHeader()
        {
            //Arrange
            var encounterContextMocked = new EncounterContext(_accessorMocked.Object);

            //Act
            Action action = () => encounterContextMocked.GetXCorrelationId();

            //Assert
            var result = action.Should().ThrowExactly<EncounterContextException>();
            result.Which.Message.Should().Be("The request does not contain 'x-correlation-id'.");
        }

        private void ConfigureContextAccessor()
        {
            _accessorMocked = new Mock<IHttpContextAccessor>();

            _fakeHttpContext = new DefaultHttpContext().HttpContext;
            _accessorMocked
                .Setup(accessor => accessor.HttpContext)
                .Returns(_fakeHttpContext);
        }
    }
}