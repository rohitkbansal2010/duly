// <copyright file="ImmunizationAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Implementations;
using Duly.Ngdp.Adapter.Adapters.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ngdp.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class ImmunizationAdapterTests
    {
        private const string FindImmunizationsStoredProcedureName = "[dulycv].[uspImmunizationsSelectByPatientId]";

        private Mock<IDapperContext> _mockedDapperContext;

        [SetUp]
        public void Setup()
        {
            _mockedDapperContext = new Mock<IDapperContext>();
        }

        [Test]
        public async Task FindImmunizationsAsync_ArgumentNullException_Test()
        {
            //Arrange
            var adapter = new ImmunizationAdapter(_mockedDapperContext.Object);

            //Act
            Func<Task> action = async () => await adapter.FindImmunizationsAsync(null);

            //Assert
            var result = await action.Should().ThrowAsync<ArgumentNullException>();
            result.Which.Message.Should().Be("Value cannot be null. (Parameter 'searchCriteria')");
        }

        [Test]
        public async Task FindImmunizationsAsync_Success_Test()
        {
            //Arrange
            var adapter = new ImmunizationAdapter(_mockedDapperContext.Object);
            var searchCriteria = new ImmunizationSearchCriteria();
            var immunizations = SetupDapperContext();

            //Act
            var result = await adapter.FindImmunizationsAsync(searchCriteria);

            //Assert
            result.Should().BeEquivalentTo(immunizations);
        }

        private IEnumerable<Immunization> SetupDapperContext()
        {
            IEnumerable<Immunization> immunizations = new Immunization[]
            {
                new()
            };

            _mockedDapperContext
                .Setup(context =>
                    context.QueryAsync<Immunization>(FindImmunizationsStoredProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(immunizations);

            return immunizations;
        }
    }
}