// <copyright file="ConfigurationRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Resource.Api.Contracts;
using Duly.CollaborationView.Resource.Api.Repositories.Implementations;
using Duly.UiConfig.Adapter.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdapterContracts = Duly.UiConfig.Adapter.Contracts;

namespace Duly.Resource.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class ConfigurationRepositoryTests
    {
        private readonly Guid _uiConfigurationId = Guid.NewGuid();
        private readonly Guid _moduleOverviewId = Guid.NewGuid();

        private Mock<IMapper> _mapperMock;
        private Mock<IUiConfigAdapter> _uiConfigAdapterMock;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();
            _uiConfigAdapterMock = new Mock<IUiConfigAdapter>();
        }

        [Test]
        public async Task GetConfigurationsAsyncTest()
        {
            //Arrange
            var siteId = "Test-Site-Id";
            var patientId = "Test-Patient-Id";
            var applicationPart = ApplicationPart.CurrentAppointment;
            UiConfigurationTargetAreaType? targetAreaType = UiConfigurationTargetAreaType.Navigation;

            var adapterConfigs = BuildNavigationAdapterConfigs(
                AdapterContracts.ApplicationPart.CurrentAppointment,
                siteId,
                patientId);
            ConfigureUiConfigAdapter(adapterConfigs);
            ConfigureMapper(adapterConfigs);

            var repository = new ConfigurationRepository(
                _mapperMock.Object,
                _uiConfigAdapterMock.Object);

            //Act
            var results = await repository.GetConfigurationsAsync(
                applicationPart,
                siteId,
                patientId,
                targetAreaType);

            //Assert
            _uiConfigAdapterMock.Verify(
                x => x.GetConfigurationsAsync(
                    It.Is<AdapterContracts.UiConfigurationSearchCriteria>(
                        criteria => criteria.ApplicationPart == AdapterContracts.ApplicationPart.CurrentAppointment
                                      && criteria.SiteId == siteId
                                      && criteria.PatientId == patientId
                                      && criteria.TargetAreaType == AdapterContracts.UiConfigurationTargetAreaType.Navigation)),
                Times.Once());

            _mapperMock.Verify(x => x.Map<IEnumerable<UiConfiguration>>(adapterConfigs), Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().AllBeOfType<NavigationModulesUiConfiguration>();

            var uiConfiguration = (NavigationModulesUiConfiguration)results.First();
            uiConfiguration.Id.Should().Be(_uiConfigurationId.ToString());
            uiConfiguration.TargetAreaType.Should().Be(UiConfigurationTargetAreaType.Navigation);
            uiConfiguration.Details.Should().NotBeNull();
            uiConfiguration.Details.Modules.Should().NotBeNullOrEmpty();
            uiConfiguration.Details.Modules.First().Alias.Should().Be("ModuleOverview");
            uiConfiguration.Details.Modules.First().Widgets.Should().NotBeNull();
        }

        private IEnumerable<AdapterContracts.UiConfigurationWithChildren> BuildNavigationAdapterConfigs(
            AdapterContracts.ApplicationPart applicationPart,
            string siteId,
            string patientId)
        {
            return new[]
            {
                new AdapterContracts.UiConfigurationWithChildren
                {
                    Configuration = new AdapterContracts.UiConfiguration
                    {
                        Id = _uiConfigurationId,
                        ApplicationPart = applicationPart,
                        TargetAreaType = AdapterContracts.UiConfigurationTargetAreaType.Navigation,
                        ConfigType = CalculateConfigType(siteId, patientId),
                        SiteId = siteId,
                        PatientId = patientId,
                        TargetType = AdapterContracts.UiConfigurationTargetType.Modules,
                        Items = new[]
                        {
                            new AdapterContracts.UiConfigurationItem
                            {
                                ItemTargetType = AdapterContracts.UiConfigurationItemTargetType.Module,
                                Index = 1,
                                ItemTarget = new AdapterContracts.UiResource
                                {
                                    Id = _moduleOverviewId,
                                    Alias = "ModuleOverview",
                                    ResourceType = AdapterContracts.UiResourceType.Module
                                }
                            }
                        }
                    },
                    Children = Array.Empty<AdapterContracts.UiConfiguration>()
                }
            };
        }

        private AdapterContracts.UiConfigurationType CalculateConfigType(string siteId, string patientId)
        {
            if (!string.IsNullOrWhiteSpace(siteId) && !string.IsNullOrWhiteSpace(patientId))
            {
                return AdapterContracts.UiConfigurationType.PatientConfiguration;
            }
            else if (!string.IsNullOrWhiteSpace(siteId))
            {
                return AdapterContracts.UiConfigurationType.ClinicConfiguration;
            }
            else
            {
                return AdapterContracts.UiConfigurationType.GlobalConfiguration;
            }
        }

        private void ConfigureUiConfigAdapter(IEnumerable<AdapterContracts.UiConfigurationWithChildren> adapterConfigs)
        {
            _uiConfigAdapterMock
                .Setup(adapter => adapter.GetConfigurationsAsync(It.IsAny<AdapterContracts.UiConfigurationSearchCriteria>()))
                .Returns(Task.FromResult(adapterConfigs));
        }

        private void ConfigureMapper(IEnumerable<AdapterContracts.UiConfigurationWithChildren> adapterNavigationConfigs)
        {
            var firstAdapterConfig = adapterNavigationConfigs.First();
            IEnumerable<NavigationModulesUiConfiguration> uiConfigurations = new NavigationModulesUiConfiguration[]
            {
                new ()
                {
                    Id = _uiConfigurationId.ToString(),
                    TargetAreaType = UiConfigurationTargetAreaType.Navigation,
                    Details = new NavigationModulesDetails
                    {
                        Modules = new[]
                        {
                            new NavigationModulesItem
                            {
                                Alias = firstAdapterConfig.Configuration.Items.First().ItemTarget.Alias,
                                Widgets = Array.Empty<NavigationWidgetsItem>()
                            }
                        }
                    }
                }
            };

            _mapperMock
                .Setup(mapper => mapper.Map<AdapterContracts.ApplicationPart>(It.IsAny<ApplicationPart>()))
                .Returns(AdapterContracts.ApplicationPart.CurrentAppointment);

            _mapperMock
                .Setup(mapper => mapper.Map<AdapterContracts.UiConfigurationTargetAreaType>(It.IsAny<UiConfigurationTargetAreaType>()))
                .Returns(AdapterContracts.UiConfigurationTargetAreaType.Navigation);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<UiConfiguration>>(adapterNavigationConfigs))
                .Returns(uiConfigurations);
        }
    }
}
