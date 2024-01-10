// <copyright file="Observations.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class ObservationsExampleProvider : IExamplesProvider<IEnumerable<Observation>>
    {
        public IEnumerable<Observation> GetExamples()
        {
            yield return new Observation
            {
                Date = DateTimeOffset.Now,
                Type = ObservationType.BloodPressure,
                Components = new ObservationComponent[]
                {
                    new ()
                    {
                        Type = ObservationComponentType.Systolic,
                        Measurements = new ObservationMeasurement[]
                        {
                            new ()
                            {
                                Value = 115.37M,
                                Unit = "mmHg"
                            }
                        }
                    },
                    new ()
                    {
                        Type = ObservationComponentType.Diastolic,
                        Measurements = new ObservationMeasurement[]
                        {
                            new ()
                            {
                                Value = 93.73M,
                                Unit = "mmHg"
                            }
                        }
                    },
                }
            };
            yield return new Observation
            {
                Date = new DateTimeOffset(2020, 07, 21, 1, 2, 3, default),
                Type = ObservationType.BodyTemperature,
                Components = new ObservationComponent[]
                {
                    new ()
                    {
                        Measurements = new ObservationMeasurement[]
                        {
                            new ()
                            {
                                Value = 36.6M,
                                Unit = "Cel"
                            },
                            new ()
                            {
                                Value = 97.88M,
                                Unit = "[degF]"
                            },
                        }
                    },
                }
            };
            yield return new Observation
            {
                Date = DateTimeOffset.Now,
                Type = ObservationType.PainLevel,
                Components = new ObservationComponent[]
                {
                    new ()
                    {
                        Measurements = new ObservationMeasurement[]
                        {
                            new ()
                            {
                                Value = 3M,
                            },
                        }
                    },
                }
            };
        }
    }
}
