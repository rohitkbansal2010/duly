// <copyright file="TestReportConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Extensions;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class TestReportConverter : ITypeConverter<DiagnosticReport, TestReport>
    {
        public TestReport Convert(DiagnosticReport source, TestReport destination, ResolutionContext context)
        {
            var testReport = new TestReport
            {
                Id = source.Id,
                Title = source.Name,
                Date = source.EffectiveDate ?? default,
                HasAbnormalResults = source.Observations.Any(CheckIfObservationIsAbnormal),
            };

            return testReport;
        }

        private static bool CheckIfObservationIsAbnormal(ObservationLabResult observationLabResult)
        {
            return observationLabResult.Components.Any(x => x.CheckIfComponentIsAbnormal());
        }
    }
}
