// <copyright file="AdapterUiConfigurationWithChildrenConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Resource.Api.Contracts;
using System;
using System.Linq;
using AdapterContracts = Duly.UiConfig.Adapter.Contracts;

namespace Duly.CollaborationView.Resource.Api.Repositories.Mappings
{
    internal class AdapterUiConfigurationWithChildrenConverter : ITypeConverter<AdapterContracts.UiConfigurationWithChildren, UiConfiguration>
    {
        private const string WithoutConfigurationError = "Could not map UiConfigurationWithChildren with Configuration == null.";
        private const string WithoutItemTargetError = "Could not map UiConfigurationItem with ItemTarget == null.";

        public UiConfiguration Convert(
            AdapterContracts.UiConfigurationWithChildren source,
            UiConfiguration destination,
            ResolutionContext context)
        {
            if (source.Configuration == null)
            {
                throw new RepositoryNotMappedException(WithoutConfigurationError);
            }

            switch (source.Configuration.TargetAreaType)
            {
                case AdapterContracts.UiConfigurationTargetAreaType.Navigation:
                    if (source.Configuration.TargetType == AdapterContracts.UiConfigurationTargetType.Modules)
                    {
                        return ConvertToNavigationModulesUiConfiguration(source, context);
                    }
                    else
                    {
                        throw new RepositoryNotMappedException(
                            $"Could not map UiConfigurationWithChildren with TargetType:[{source.Configuration.TargetType}] to corresponding contract.");
                    }

                case AdapterContracts.UiConfigurationTargetAreaType.Layout:
                default:
                {
                    throw new RepositoryNotMappedException(
                        $"Could not map UiConfigurationWithChildren with TargetAreaType:[{source.Configuration.TargetAreaType}] to corresponding contract.");
                }
            }
        }

        private static NavigationModulesUiConfiguration ConvertToNavigationModulesUiConfiguration(AdapterContracts.UiConfigurationWithChildren source, ResolutionContext context)
        {
            var configuration = new NavigationModulesUiConfiguration
            {
                Id = source.Configuration.Id.ToString(),
                TargetAreaType = UiConfigurationTargetAreaType.Navigation,
                Details = new NavigationModulesDetails
                {
                    Modules = BuildModules(source, context)
                },
                Tags = source.Configuration.Tags
                    ?.Replace(" ", string.Empty)
                    .Split(",")
            };

            return configuration;
        }

        private static NavigationModulesItem[] BuildModules(AdapterContracts.UiConfigurationWithChildren source, ResolutionContext context)
        {
            return source.Configuration.Items
                .Select(x => ConvertToNavigationModulesItem(x, source, context))
                .ToArray();
        }

        private static NavigationModulesItem ConvertToNavigationModulesItem(
            AdapterContracts.UiConfigurationItem item,
            AdapterContracts.UiConfigurationWithChildren source,
            ResolutionContext context)
        {
            if (item.ItemTarget == null)
            {
                throw new RepositoryNotMappedException(WithoutItemTargetError);
            }

            var navigationModulesItem = new NavigationModulesItem
            {
                Alias = item.ItemTarget.Alias,
                Widgets = BuildWidgets(item.ItemTarget.Id, source, context)
            };

            return navigationModulesItem;
        }

        private static NavigationWidgetsItem[] BuildWidgets(
            Guid id,
            AdapterContracts.UiConfigurationWithChildren source,
            ResolutionContext context)
        {
            var items = source.Children
                ?.FirstOrDefault(x => x.ParentTargetId == id)
                ?.Items;

            return items == null
                ? Array.Empty<NavigationWidgetsItem>()
                : items.Select(ConvertToNavigationWidgetsItem)
                    .ToArray();
        }

        private static NavigationWidgetsItem ConvertToNavigationWidgetsItem(
            AdapterContracts.UiConfigurationItem item)
        {
            if (item.ItemTarget == null)
            {
                throw new RepositoryNotMappedException(WithoutItemTargetError);
            }

            return new NavigationWidgetsItem
            {
                Alias = item.ItemTarget.Alias
            };
        }
    }
}