// -----------------------------------------------------------------------
// <copyright file="Immunizations.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Examples")]
    public class ImmunizationsExampleProvider : IExamplesProvider<Immunizations>
    {
        public const string ADMINISTEREDTITLE = Immunizations.AdministeredTitle;
        public const string DUETITLE = "DUE";
        public const string NOTADMINISTEREDTITLE = Immunizations.NotAdministeredTitle;
        public const string POSTPONEDTITLE = "POSTPONED";

        public Immunizations GetExamples()
        {
            var immunizations = new Immunizations
            {
                RecommendedImmunizations = BuildRecommendedImmunizations(),
                PastImmunizations = BuildPastImmunizations()
            };

            return immunizations;
        }

        private static ImmunizationsRecommendedGroup[] BuildRecommendedImmunizations()
        {
            return new[]
            {
                new ImmunizationsRecommendedGroup
                {
                    Title = "TDAP",
                    Vaccinations = new[]
                    {
                        new RecommendedVaccination
                        {
                            Title = "Tdap",
                            Status = RecommendedVaccinationStatus.Completed,
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(2002, 3, 11))
                        },
                        new RecommendedVaccination
                        {
                            Title = "Tdap",
                            Status = RecommendedVaccinationStatus.Completed,
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(2012, 3, 21))
                        },
                        new RecommendedVaccination
                        {
                            Title = "Tdap",
                            Status = RecommendedVaccinationStatus.DueOn,
                            DateTitle = DUETITLE,
                            Date = new DateTimeOffset(new DateTime(2022, 3, 15))
                        }
                    }
                },
                new ImmunizationsRecommendedGroup
                {
                    Title = "FLU",
                    Vaccinations = new[]
                    {
                        new RecommendedVaccination
                        {
                            Title = "Influenza, seasonal, injectable",
                            Status = RecommendedVaccinationStatus.Addressed,
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(2017, 9, 3))
                        },
                        new RecommendedVaccination
                        {
                            Title = "Influenza, seasonal, injectable",
                            Status = RecommendedVaccinationStatus.Addressed,
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(2018, 9, 5))
                        },
                        new RecommendedVaccination
                        {
                            Title = "Influenza, seasonal, injectable",
                            Status = RecommendedVaccinationStatus.Addressed,
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(2019, 9, 7))
                        },
                        new RecommendedVaccination
                        {
                            Title = "Influenza, high dose seasonal",
                            Status = RecommendedVaccinationStatus.Completed,
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(2020, 9, 1))
                        },
                        new RecommendedVaccination
                        {
                            Title = "Influenza, high dose seasonal",
                            Status = RecommendedVaccinationStatus.Postponed,
                            DateTitle = POSTPONEDTITLE
                        }
                    }
                },
                new ImmunizationsRecommendedGroup
                {
                    Title = "Encephalitis",
                    Vaccinations = new[]
                    {
                        new RecommendedVaccination
                        {
                            Title = "IXIARO",
                            Status = RecommendedVaccinationStatus.Completed,
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(2018, 5, 7))
                        },
                        new RecommendedVaccination
                        {
                            Title = "IXIARO",
                            Status = RecommendedVaccinationStatus.Postponed,
                            DateTitle = POSTPONEDTITLE,
                            Date = new DateTimeOffset(new DateTime(2019, 5, 11)),
                            Notes = "Patient refused second dose on 5/11/2019 as she noticed she felt like the first dose gave her anxiety and made her feel dizzy."
                        }
                    }
                },
                new ImmunizationsRecommendedGroup
                {
                    Title = "COVID-19",
                    Vaccinations = new[]
                    {
                        new RecommendedVaccination
                        {
                            Title = "Covid-19 Vaccination Pfizer 30 mcg/0.3 ml",
                            Status = RecommendedVaccinationStatus.Completed,
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(2021, 3, 17))
                        },
                        new RecommendedVaccination
                        {
                            Title = "Covid-19 Vaccination Pfizer 30 mcg/0.3 ml",
                            Status = RecommendedVaccinationStatus.Completed,
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(2021, 5, 17))
                        },
                        new RecommendedVaccination
                        {
                            Title = "Covid-19 Vaccination Pfizer 30 mcg/0.3 ml",
                            Status = RecommendedVaccinationStatus.DueSoon,
                            DateTitle = DUETITLE
                        }
                    }
                },
                new ImmunizationsRecommendedGroup
                {
                    Title = "ADENO",
                    Vaccinations = new[]
                    {
                        new RecommendedVaccination
                        {
                            Title = "Adenovirus types 4 and 7",
                            Status = RecommendedVaccinationStatus.DueSoon,
                            DateTitle = DUETITLE
                        }
                    }
                },
                new ImmunizationsRecommendedGroup
                {
                    Title = "RABIES",
                    Vaccinations = new[]
                    {
                        new RecommendedVaccination
                        {
                            Title = "Rabies - IM fibroblast culture",
                            Status = RecommendedVaccinationStatus.NotDue,
                            DateTitle = DUETITLE
                        }
                    }
                }
            };
        }

        private static ImmunizationsGroup[] BuildPastImmunizations()
        {
            return new[]
            {
                new ImmunizationsGroup
                {
                    Title = "HepA",
                    Vaccinations = new[]
                    {
                        new Vaccination
                        {
                            Title = "Hep A, adult",
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(1985, 7, 5)),
                            Dose = new Dose
                            {
                                Amount = 1.0M,
                                Unit = "mL"
                            }
                        }
                    }
                },
                new ImmunizationsGroup
                {
                    Title = "HepB",
                    Vaccinations = new[]
                    {
                        new Vaccination
                        {
                            Title = "Hep B-dialysis",
                            DateTitle = ADMINISTEREDTITLE,
                            Date = new DateTimeOffset(new DateTime(1998, 2, 15)),
                            Dose = new Dose
                            {
                                Amount = 1.0M,
                                Unit = "mL"
                            }
                        }
                    }
                },
                new ImmunizationsGroup
                {
                    Title = "MENING",
                    Vaccinations = new[]
                    {
                        new Vaccination
                        {
                            Title = "Meningococcal MCV4O",
                            DateTitle = NOTADMINISTEREDTITLE
                        }
                    }
                },
                new ImmunizationsGroup
                {
                    Title = "POLIO",
                    Vaccinations = new[]
                    {
                        new Vaccination
                        {
                            Title = "IPV (e-IPV)",
                            DateTitle = ADMINISTEREDTITLE
                        }
                    }
                }
            };
        }
    }
}