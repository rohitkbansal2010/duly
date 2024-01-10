// <copyright file="NgdpRecommendedProviderRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Implementations;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class NgdpRecommendedProviderRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IRecommendedProviderAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IRecommendedProviderAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetAppointmentByCsnIdAsync_Test()
        {
            //Arrange
            const decimal referralId = 123566;

            var providers = SetupAdapter(referralId);
            var recommendedProvider = SetupMapper(providers);

            var repository = new NgdpRecommendedProviderRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var result = await repository.GetRecommendedProvidersByReferralIdAsync(referralId.ToString());

            //Assert
            result.Should().BeEquivalentTo(recommendedProvider);
        }

        private IEnumerable<AdapterModels.RecommendedProvider> SetupAdapter(decimal referralId)
        {
            IEnumerable<AdapterModels.RecommendedProvider> providers = new AdapterModels.RecommendedProvider[]
            {
                new()
                {
                    ReferralId = referralId
                }
            };

            _adapterMocked
                .Setup(adapter => adapter.FindRecommendedProvidersByReferralIdAsync(referralId.ToString()))
                .ReturnsAsync(providers);

            return providers;
        }

        private IEnumerable<RecommendedProvider> SetupMapper(IEnumerable<AdapterModels.RecommendedProvider> providers)
        {
            IEnumerable<RecommendedProvider> recommendedProviders = new[]
            {
                new RecommendedProvider()
                {
                    Referral = new Referral()
                    {
                        Identifier = new Identifier()
                        {
                            Id = providers.First().ReferralId.ToString()
                        }
                    }
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<RecommendedProvider>>(providers))
                .Returns(recommendedProviders);

            return recommendedProviders;
        }
    }
}
