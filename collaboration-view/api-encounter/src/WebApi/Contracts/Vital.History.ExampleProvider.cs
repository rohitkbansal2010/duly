// -----------------------------------------------------------------------
// <copyright file="Vital.History.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Services.Constants;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class VitalsHistoryExampleProvider : IExamplesProvider<VitalHistory>
    {
        public VitalHistory GetExamples(VitalsCardType vitalsCardType)
        {
            return vitalsCardType switch
            {
                VitalsCardType.BloodPressure => GetBloodPressure(),
                _ => GetExamples()
            };
        }

        public VitalHistory GetExamples()
        {
            return new VitalHistory
            {
                Chart = GetChart()
            };
        }

        private static VitalHistory GetBloodPressure()
        {
            return new VitalHistory
            {
                Chart = new Chart<DateTimeOffset, decimal>
                {
                    Datasets = new[]
                    {
                        GetSystolic(),
                        GetDiastolic()
                    },
                    ChartOptions = new ChartOptions<DateTimeOffset, decimal>
                    {
                        ChartScales = new ChartScales<DateTimeOffset, decimal>
                        {
                            YAxis = new Axis<decimal>
                            {
                                Max = 160,
                                Min = 60,
                                StepSize = 20
                            }
                        }
                    }
                }
            };
        }

        private static Chart<DateTimeOffset, decimal> GetChart()
        {
            return new Chart<DateTimeOffset, decimal>
            {
                Datasets = new[]
                {
                    GetPulse()
                },
                ChartOptions = new ChartOptions<DateTimeOffset, decimal>
                {
                    ChartScales = new ChartScales<DateTimeOffset, decimal>
                    {
                        YAxis = new Axis<decimal>
                        {
                            Max = 100,
                            Min = 40,
                            StepSize = 10
                        }
                    }
                }
            };
        }

        private static ChartDataset<DateTimeOffset, decimal> GetPulse()
        {
            return new ChartDataset<DateTimeOffset, decimal>
            {
                Label = "Heart Rate",
                Data = new ChartData<DateTimeOffset, decimal>
                {
                    Dimension = "BPM",
                    Values = new ChartDataValue<DateTimeOffset, decimal>[]
                    {
                        new()
                        {
                            X = new DateTime(2020, 01, 12),
                            Y = 54
                        },
                        new()
                        {
                            X = new DateTime(2020, 02, 12),
                            Y = 67
                        },
                        new()
                        {
                            X = new DateTime(2020, 03, 12),
                            Y = 75
                        },
                        new()
                        {
                            X = new DateTime(2020, 04, 12),
                            Y = 60
                        },
                        new()
                        {
                            X = new DateTime(2020, 05, 12),
                            Y = 79
                        },
                        new()
                        {
                            X = new DateTime(2020, 06, 12),
                            Y = 72
                        },
                        new()
                        {
                            X = new DateTime(2020, 07, 12),
                            Y = 66
                        },
                        new()
                        {
                            X = new DateTime(2020, 09, 12),
                            Y = 73
                        },
                        new()
                        {
                            X = new DateTime(2020, 10, 12),
                            Y = 65
                        },
                        new()
                        {
                            X = new DateTime(2020, 11, 12),
                            Y = 57
                        }
                    }
                },
                Ranges = new KeyValuePair<string, ExpectedRange<decimal>>[]
                {
                    new("Normal", new ExpectedRange<decimal>
                    {
                        Min = 60,
                        Max = 80
                    })
                },
                Visible = true
            };
        }

        private static ChartDataset<DateTimeOffset, decimal> GetSystolic()
        {
            return new ChartDataset<DateTimeOffset, decimal>
            {
                Label = VitalsConstants.SystolicBloodPressureLabel,
                Data = new ChartData<DateTimeOffset, decimal>
                {
                    Dimension = "mm Hg",
                    Values = new ChartDataValue<DateTimeOffset, decimal>[]
                    {
                        new()
                        {
                            X = new DateTime(2020, 01, 12),
                            Y = 116
                        },
                        new()
                        {
                            X = new DateTime(2020, 02, 12),
                            Y = 126
                        },
                        new()
                        {
                            X = new DateTime(2020, 03, 12),
                            Y = 130
                        },
                        new()
                        {
                            X = new DateTime(2020, 04, 12),
                            Y = 130
                        },
                        new()
                        {
                            X = new DateTime(2020, 05, 12),
                            Y = 112
                        },
                        new()
                        {
                            X = new DateTime(2020, 06, 12),
                            Y = 115
                        },
                        new()
                        {
                            X = new DateTime(2020, 07, 12),
                            Y = 121
                        },
                        new()
                        {
                            X = new DateTime(2020, 09, 12),
                            Y = 118
                        },
                        new()
                        {
                            X = new DateTime(2020, 10, 12),
                            Y = 144
                        },
                        new()
                        {
                            X = new DateTime(2020, 11, 12),
                            Y = 145
                        }
                    }
                },
                Ranges = new KeyValuePair<string, ExpectedRange<decimal>>[]
                {
                    new("Normal", new ExpectedRange<decimal>
                    {
                        Min = 125,
                        Max = 140
                    })
                },
                Visible = true
            };
        }

        private static ChartDataset<DateTimeOffset, decimal> GetDiastolic()
        {
            return new ChartDataset<DateTimeOffset, decimal>
            {
                Label = VitalsConstants.DiastolicBloodPressureLabel,
                Data = new ChartData<DateTimeOffset, decimal>
                {
                    Dimension = "mm Hg",
                    Values = new ChartDataValue<DateTimeOffset, decimal>[]
                    {
                        new()
                        {
                            X = new DateTime(2020, 01, 12),
                            Y = 62
                        },
                        new()
                        {
                            X = new DateTime(2020, 02, 12),
                            Y = 77
                        },
                        new()
                        {
                            X = new DateTime(2020, 03, 12),
                            Y = 68
                        },
                        new()
                        {
                            X = new DateTime(2020, 04, 12),
                            Y = 94
                        },
                        new()
                        {
                            X = new DateTime(2020, 05, 12),
                            Y = 99
                        },
                        new()
                        {
                            X = new DateTime(2020, 06, 12),
                            Y = 82
                        },
                        new()
                        {
                            X = new DateTime(2020, 07, 12),
                            Y = 85
                        },
                        new()
                        {
                            X = new DateTime(2020, 09, 12),
                            Y = 88
                        },
                        new()
                        {
                            X = new DateTime(2020, 10, 12),
                            Y = 85
                        },
                        new()
                        {
                            X = new DateTime(2020, 11, 12),
                            Y = 89
                        }
                    }
                },
                Ranges = new KeyValuePair<string, ExpectedRange<decimal>>[]
                {
                    new("Normal", new ExpectedRange<decimal>
                    {
                        Min = 90,
                        Max = 100
                    })
                },
                Visible = true
            };
        }
    }
}