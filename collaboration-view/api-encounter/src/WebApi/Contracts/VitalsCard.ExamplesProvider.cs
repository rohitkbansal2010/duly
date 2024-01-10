// -----------------------------------------------------------------------
// <copyright file="VitalsCard.ExamplesProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class VitalsCardExamplesProvider : IExamplesProvider<IEnumerable<VitalsCard>>
    {
        public IEnumerable<VitalsCard> GetExamples()
        {
            var provider = new VitalsCardExampleProvider();

            yield return provider.BuildExampleFor(VitalsCardType.BloodPressure);
            yield return provider.BuildExampleFor(VitalsCardType.BloodOxygen);
            yield return provider.BuildExampleFor(VitalsCardType.HeartRate);
            yield return provider.BuildExampleFor(VitalsCardType.Temperature);
            yield return provider.BuildExampleFor(VitalsCardType.RespiratoryRate);
            yield return provider.BuildExampleFor(VitalsCardType.PainLevel);
            yield return provider.BuildExampleFor(VitalsCardType.WeightAndHeight);
            yield return provider.BuildExampleFor(VitalsCardType.BodyMassIndex);
        }
    }
}