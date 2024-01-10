// <copyright file="RecommendedProviderAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter;
using Duly.Ngdp.Adapter.Adapters.Implementations;
using Duly.Ngdp.Adapter.Adapters.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ngdp.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class RecommendedProviderAdapterTests
    {
        private const string FindRecommendedProvidersByReferralIdProcedureName = Constants.SchemaName + Constants.NameSeparator + "[uspReferralRecommendedProvidersSelect]";

        private Mock<IDapperContext> _mockedDapperContext;

        [SetUp]
        public void Setup()
        {
            _mockedDapperContext = new Mock<IDapperContext>();
        }

        [Test]
        public async Task FindRecommendedProvidersByReferralIdAsync_Test()
        {
            //Arrange
            const int referralId = 123456677;
            var adapter = new RecommendedProviderAdapter(_mockedDapperContext.Object);
            var providers = SetupDapperContext(referralId);

            //Act
            var result = await adapter.FindRecommendedProvidersByReferralIdAsync(referralId.ToString());

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(providers);
        }

        private IEnumerable<RecommendedProvider> SetupDapperContext(decimal refId)
        {
            IEnumerable<RecommendedProvider> providers = new RecommendedProvider[]
            {
                new()
                {
                    ReferralId = refId
                }
            };

            _mockedDapperContext
                .Setup(context =>
                    context.QueryAsync<RecommendedProvider>(FindRecommendedProvidersByReferralIdProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(providers);

            return providers;
        }
    }
}
