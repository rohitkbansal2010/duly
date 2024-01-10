// <copyright file="ProviderAdapterTests.cs" company="Duly Health and Care">
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
    public class ProviderAdapterTests
    {
        private const string FindProviderByLatLng = "GetProviderByLatLng";
        private Mock<ICVDapperContext> _mockedDapperContext;

        [SetUp]
        public void Setup()
        {
            _mockedDapperContext = new Mock<ICVDapperContext>();
        }

        [Test]
        public async Task FindProviderByLatLngAsync_ArgumentNullException_Test()
        {
            //Arrange
            var adapter = new ProviderAdapter(_mockedDapperContext.Object);

            //Act
            Func<Task> action = async () => await adapter.FindProviderByLatLngAsync(null);

            //Assert
            var result = await action.Should().ThrowAsync<ArgumentNullException>();
            result.Which.Message.Should().Be("Value cannot be null. (Parameter 'searchCriteria')");
        }

        [Test]
        public async Task FindProviderByLatLngAsync_Success_Test()
        {
            //Arrange
            var adapter = new ProviderAdapter(_mockedDapperContext.Object);
            var searchCriteria = new ProviderSearchCriteria();
            var providers = SetupDapperContext();

            //Act
            var result = await adapter.FindProviderByLatLngAsync(searchCriteria);

            //Assert
            result.Should().BeEquivalentTo(providers);
        }

        private IEnumerable<ProviderLocation> SetupDapperContext()
        {
            IEnumerable<ProviderLocation> providers = new ProviderLocation[]
            {
                new()
            };

            _mockedDapperContext
                .Setup(context =>
                context.QueryAsync<ProviderLocation>(FindProviderByLatLng, It.IsAny<object>(), default, default))
                .ReturnsAsync(providers);

            return providers;
        }

    }
}
