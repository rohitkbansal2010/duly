// <copyright file="ChartDatasetVisibilityIdentifier.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions
{
    internal static class ChartDatasetVisibilityIdentifier
    {
        public static bool BuildVisible(this ObservationType observationType)
        {
            return observationType switch
            {
                ObservationType.BodyHeight => false,
                _ => true
            };
        }
    }
}