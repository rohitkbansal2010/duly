// <copyright file="TestReportResultConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Extensions;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class TestReportResultConverter : ITypeConverter<ObservationLabResult, TestReportResult>
    {
        public TestReportResult Convert(ObservationLabResult source, TestReportResult destination, ResolutionContext context)
        {
            var selectedComponent = source.Components.First();
            var selectedReferenceRange = selectedComponent.ReferenceRange?.FirstOrDefault();
            var selectedMeasurement = selectedComponent.Measurements?.FirstOrDefault();
            var interpretations = selectedComponent.Interpretations?.Select(x => x.Text).ToArray();

            var testReportResult = new TestReportResult
            {
                Id = source.Id,
                ComponentName = source.ComponentName,
                IsAbnormalResult = selectedComponent.CheckIfComponentIsAbnormal(),
                Measurement = context.Mapper.Map<TestReportResultMeasurement>(selectedMeasurement),
                ValueText = selectedComponent.ValueText,
                ReferenceRange = context.Mapper.Map<TestReportResultReferenceRange>(selectedReferenceRange),
                Notes = selectedComponent.Notes,
                Interpretations = interpretations
            };

            return testReportResult;
        }
    }
}
