// -----------------------------------------------------------------------
// <copyright file="UiConfiguration.NavigationModulesUiConfigurationExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.CollaborationView.Resource.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class NavigationModulesUiConfigurationExampleProvider : IExamplesProvider<NavigationModulesUiConfiguration>
    {
        public NavigationModulesUiConfiguration GetExamples()
        {
            return BuildExample(ApplicationPart.CurrentAppointment);
        }

        public NavigationModulesUiConfiguration BuildExample(
            ApplicationPart appPart,
            string siteId = null,
            string patientId = null)
        {
            var config = new NavigationModulesUiConfiguration
            {
                Id = "ExampleNavigationModulesConfig",
                TargetAreaType = UiConfigurationTargetAreaType.Navigation
            };

            if (appPart == ApplicationPart.Calendar)
            {
                config.Details = new NavigationModulesDetails
                {
                    Modules = new[]
                    {
                        new NavigationModulesItem
                        {
                            Alias = UiConfigurationModules.CalendarModuleAlias,
                            Widgets = new[]
                            {
                                new NavigationWidgetsItem
                                {
                                    Alias = UiConfigurationWidgets.TodayAppointmentsWidgetAlias
                                }
                            }
                        }
                    }
                };
            }
            else
            {
                config.Details = new NavigationModulesDetails
                {
                    Modules = new[]
                    {
                        new NavigationModulesItem
                        {
                            Alias = UiConfigurationModules.OverviewModuleAlias,
                            Widgets = new[]
                            {
                                new NavigationWidgetsItem
                                {
                                    Alias = UiConfigurationWidgets.QuestionsWidgetAlias
                                },
                                new NavigationWidgetsItem
                                {
                                    Alias = UiConfigurationWidgets.VitalsWidgetAlias
                                },
                                new NavigationWidgetsItem
                                {
                                    Alias = UiConfigurationWidgets.GoalsWidgetAlias
                                },
                                new NavigationWidgetsItem
                                {
                                    Alias = UiConfigurationWidgets.ConditionsWidgetAlias
                                },
                                new NavigationWidgetsItem
                                {
                                    Alias = UiConfigurationWidgets.AppointmentsWidgetAlias
                                },
                                new NavigationWidgetsItem
                                {
                                    Alias = UiConfigurationWidgets.MedicationsWidgetAlias
                                },
                                new NavigationWidgetsItem
                                {
                                    Alias = UiConfigurationWidgets.AllergiesWidgetAlias
                                },
                                new NavigationWidgetsItem
                                {
                                    Alias = UiConfigurationWidgets.ImmunizationsWidgetAlias
                                }
                            }
                        },
                        new NavigationModulesItem
                        {
                            Alias = UiConfigurationModules.CarePlanModuleAlias,
                            Widgets = Array.Empty<NavigationWidgetsItem>()
                        },
                        new NavigationModulesItem
                        {
                            Alias = UiConfigurationModules.EducationModuleAlias,
                            Widgets = Array.Empty<NavigationWidgetsItem>()
                        },
                        new NavigationModulesItem
                        {
                            Alias = UiConfigurationModules.ResultsModuleAlias,
                            Widgets = Array.Empty<NavigationWidgetsItem>()
                        },
                        new NavigationModulesItem
                        {
                            Alias = UiConfigurationModules.TelehealthModuleAlias,
                            Widgets = Array.Empty<NavigationWidgetsItem>()
                        }
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(patientId))
            {
                config.Id += "ForPatient";
            }
            else if (!string.IsNullOrWhiteSpace(siteId))
            {
                config.Id += "ForSite";
            }

            return config;
        }
    }
}