// <copyright file="AdapterUiConfigurationWithChildrenConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Resource.Api.Contracts;
using Duly.CollaborationView.Resource.Api.Repositories.Mappings;
using Duly.Resource.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using AdapterContracts = Duly.UiConfig.Adapter.Contracts;

namespace Duly.Resource.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    internal class AdapterUiConfigurationWithChildrenConverterTests : MapperConfigurator<AdapterContractsToProcessContractsProfile>
    {
        [Test]
        public void ConvertTest4Navigation()
        {
            //Arrange
            var siteId = "Test-Site-Id";
            var patientId = "Test-Patient-Id";
            var applicationPart = AdapterContracts.ApplicationPart.CurrentAppointment;

            var source = BuildUiConfigurationWithChildrenForNavigation(
                applicationPart,
                siteId,
                patientId);

            //Act
            var result = Mapper.Map<UiConfiguration>(source);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NavigationModulesUiConfiguration>();

            var navigationModulesUiConfiguration = (NavigationModulesUiConfiguration)result;
            navigationModulesUiConfiguration.Id.Should().Be(source.Configuration.Id.ToString());

            navigationModulesUiConfiguration.TargetAreaType.Should().Be(UiConfigurationTargetAreaType.Navigation);

            navigationModulesUiConfiguration.Tags.Should().HaveCount(2);
            navigationModulesUiConfiguration.Tags[0].Should().Be("tag1");
            navigationModulesUiConfiguration.Tags[1].Should().Be("tag2");

            navigationModulesUiConfiguration.Details.Should().BeOfType<NavigationModulesDetails>();
            navigationModulesUiConfiguration.Details.Modules.Should().NotBeNullOrEmpty();

            var modulesItem = navigationModulesUiConfiguration.Details.Modules.First();
            modulesItem.Alias.Should().Be("ModuleOverview");
            modulesItem.Widgets.Should().HaveCount(1);
            modulesItem.Widgets[0].Alias.Should().Be("WidgetQuestions");
        }

        [Test]
        public void ConvertTest4Navigation_WithoutConfiguration()
        {
            //Arrange
            var siteId = "Test-Site-Id";
            var patientId = "Test-Patient-Id";
            var applicationPart = AdapterContracts.ApplicationPart.CurrentAppointment;

            var source = BuildUiConfigurationWithChildrenForNavigation(
                applicationPart,
                siteId,
                patientId);
            source.Configuration = null;

            //Act
            Action action = () => Mapper.Map<UiConfiguration>(source);

            //Assert
            var result = action.Should().ThrowExactly<RepositoryNotMappedException>();
            result.Which.Message
                .Should().Be("Could not map UiConfigurationWithChildren with Configuration == null.");
        }

        [Test]
        public void ConvertTest4Navigation_WithIncorrectTargetType()
        {
            //Arrange
            var siteId = "Test-Site-Id";
            var patientId = "Test-Patient-Id";
            var applicationPart = AdapterContracts.ApplicationPart.CurrentAppointment;

            var source = BuildUiConfigurationWithChildrenForNavigation(
                applicationPart,
                siteId,
                patientId);
            source.Configuration.TargetType = AdapterContracts.UiConfigurationTargetType.Widgets;

            //Act
            Action action = () => Mapper.Map<UiConfiguration>(source);

            //Assert
            var result = action.Should().ThrowExactly<RepositoryNotMappedException>();
            result.Which.Message
                .Should().Be($"Could not map UiConfigurationWithChildren with TargetType:[{source.Configuration.TargetType.ToString()}] to corresponding contract.");
        }

        [Test]
        public void ConvertTest4Navigation_WithoutItemTargetInConfigurationItem()
        {
            //Arrange
            var siteId = "Test-Site-Id";
            var patientId = "Test-Patient-Id";
            var applicationPart = AdapterContracts.ApplicationPart.CurrentAppointment;

            var source = BuildUiConfigurationWithChildrenForNavigation(
                applicationPart,
                siteId,
                patientId);
            source.Configuration.Items[0].ItemTarget = null;

            //Act
            Action action = () => Mapper.Map<UiConfiguration>(source);

            //Assert
            var result = action.Should().ThrowExactly<RepositoryNotMappedException>();
            result.Which.Message
                .Should().Be("Could not map UiConfigurationItem with ItemTarget == null.");
        }

        [Test]
        public void ConvertTest4Navigation_WithoutItemTargetInChildrenConfigurationItem()
        {
            //Arrange
            var siteId = "Test-Site-Id";
            var patientId = "Test-Patient-Id";
            var applicationPart = AdapterContracts.ApplicationPart.CurrentAppointment;

            var source = BuildUiConfigurationWithChildrenForNavigation(
                applicationPart,
                siteId,
                patientId);
            source.Children[0].Items[0].ItemTarget = null;

            //Act
            Action action = () => Mapper.Map<UiConfiguration>(source);

            //Assert
            var result = action.Should().ThrowExactly<RepositoryNotMappedException>();
            result.Which.Message
                .Should().Be("Could not map UiConfigurationItem with ItemTarget == null.");
        }

        [Test]
        public void ConvertTest4Layout()
        {
            //Arrange
            var siteId = "Test-Site-Id";
            var patientId = "Test-Patient-Id";
            var applicationPart = AdapterContracts.ApplicationPart.CurrentAppointment;

            var source = BuildUiConfigurationWithChildrenForLayout(
                applicationPart,
                siteId,
                patientId);

            //Act
            Action action = () => Mapper.Map<UiConfiguration>(source);

            //Assert
            var result = action.Should().ThrowExactly<RepositoryNotMappedException>();
            result.Which.Message
                .Should().Be($"Could not map UiConfigurationWithChildren with TargetAreaType:[{source.Configuration.TargetAreaType.ToString()}] to corresponding contract.");
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

        private AdapterContracts.UiConfigurationWithChildren BuildUiConfigurationWithChildrenForNavigation(
            AdapterContracts.ApplicationPart applicationPart,
            string siteId,
            string patientId)
        {
            var uiConfigurationId = Guid.NewGuid();
            var moduleOverviewId = Guid.NewGuid();
            var widgetQuestionsId = Guid.NewGuid();

            return new AdapterContracts.UiConfigurationWithChildren
            {
                Configuration = new AdapterContracts.UiConfiguration
                {
                    Id = uiConfigurationId,
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
                                Id = moduleOverviewId,
                                Alias = "ModuleOverview",
                                ResourceType = AdapterContracts.UiResourceType.Module
                            }
                        }
                    },
                    Tags = "tag1, tag2"
                },
                Children = new[]
                {
                    new AdapterContracts.UiConfiguration
                    {
                        Id = Guid.NewGuid(),
                        ParentConfigurationId = uiConfigurationId,
                        ParentTargetId = moduleOverviewId,
                        ApplicationPart = applicationPart,
                        TargetAreaType = AdapterContracts.UiConfigurationTargetAreaType.Navigation,
                        ConfigType = CalculateConfigType(siteId, patientId),
                        SiteId = siteId,
                        PatientId = patientId,
                        TargetType = AdapterContracts.UiConfigurationTargetType.Widgets,
                        Items = new[]
                        {
                            new AdapterContracts.UiConfigurationItem
                            {
                                ItemTargetType = AdapterContracts.UiConfigurationItemTargetType.Widget,
                                Index = 1,
                                ItemTarget = new AdapterContracts.UiResource
                                {
                                    Id = widgetQuestionsId,
                                    Alias = "WidgetQuestions",
                                    ResourceType = AdapterContracts.UiResourceType.Widget
                                }
                            }
                        }
                    }
                }
            };
        }

        private AdapterContracts.UiConfigurationWithChildren BuildUiConfigurationWithChildrenForLayout(
            AdapterContracts.ApplicationPart applicationPart,
            string siteId,
            string patientId)
        {
            var uiConfigurationId = Guid.NewGuid();

            return new AdapterContracts.UiConfigurationWithChildren
            {
                Configuration = new AdapterContracts.UiConfiguration
                {
                    Id = uiConfigurationId,
                    ApplicationPart = applicationPart,
                    TargetAreaType = AdapterContracts.UiConfigurationTargetAreaType.Layout,
                    ConfigType = CalculateConfigType(siteId, patientId),
                    SiteId = siteId,
                    PatientId = patientId,
                    TargetType = AdapterContracts.UiConfigurationTargetType.Widgets,
                    Items = Array.Empty<AdapterContracts.UiConfigurationItem>(),
                    Tags = "tag1, tag2"
                },
                Children = Array.Empty<AdapterContracts.UiConfiguration>()
            };
        }
    }
}
