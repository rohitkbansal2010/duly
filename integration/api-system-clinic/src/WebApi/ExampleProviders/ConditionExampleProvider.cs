// -----------------------------------------------------------------------
// <copyright file="ConditionExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    public class ConditionExampleProvider : IExamplesProvider<IEnumerable<Condition>>
    {
        public IEnumerable<Condition> GetExamples()
        {
            yield return new Condition
            {
                Id = "KY4weoablxy14OSgm4DWeyUmuVx-mWa9WnczY9..NKLE3",
                Name = "Bat ear",
                ClinicalStatus = ConditionClinicalStatus.Active,
                RecordedDate = DateTime.Now
            };
            yield return new Condition
            {
                Id = "KY4weoablxy14OSgm4DWeyUmuVx-mWa9WnczY9..NKLE3",
                Name = "Rheumatoid arthritis of multiple sites with negative rheumatoid factor (HCC)",
                ClinicalStatus = ConditionClinicalStatus.Active,
                RecordedDate = DateTime.Now
            };
            yield return CreateCondtitionWithPeriod();
        }

        private static Condition CreateCondtitionWithPeriod()
        {
            return new Condition
            {
                Id = "KY4weoablxy14OSgm4DWeyUmuVx-mWa9WnczY9..NKLE3",
                Name = "Malignant neoplasm of lower-outer quadrant of left breast of female, estrogen receptor positive (HCC)",
                ClinicalStatus = ConditionClinicalStatus.Resolved,
                RecordedDate = DateTime.Now,
                AbatementPeriod = new Period { End = DateTime.UtcNow }
            };
        }
    }
}