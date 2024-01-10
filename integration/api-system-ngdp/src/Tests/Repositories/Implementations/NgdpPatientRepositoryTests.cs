// <copyright file="NgdpPatientRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class NgdpPatientRepositoryTests
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
        public async Task GetPatientByReferralIdAsync_Test()
        {
            //Arrange
            const string referralId = "1";

            var provider = SetupAdapter(referralId);
            var systemAppointments = SetupMapper(provider);

            var repository = new NgdpPatientRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var result = await repository
                .GetPatientByReferralIdAsync(referralId);

            //Assert
            result.Should().BeEquivalentTo(systemAppointments);
        }

        private AdapterModels.RecommendedProvider SetupAdapter(string referralId)
        {
            var provider = new AdapterModels.RecommendedProvider();

            _adapterMocked
                .Setup(adapter => adapter.FindFirstRecommendedProviderByReferralIdAsync(referralId))
                .ReturnsAsync(provider);

            return provider;
        }

        private Contracts.ReferralPatient SetupMapper(AdapterModels.RecommendedProvider provider)
        {
            Contracts.ReferralPatient systemPatient = new();

            _mapperMocked
                .Setup(mapper => mapper.Map<Contracts.ReferralPatient>(provider))
                .Returns(systemPatient);

            return systemPatient;
        }
    }
}
