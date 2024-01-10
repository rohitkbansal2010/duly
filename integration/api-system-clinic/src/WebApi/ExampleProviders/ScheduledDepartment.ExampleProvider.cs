// <copyright file="ScheduledDepartment.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class ScheduledDepartmentExampleProvider : IExamplesProvider<ScheduledDepartment>
    {
        public ScheduledDepartment GetExamples()
        {
            return new()
            {
                Identifiers = new[]
                {
                    "Internal|25069",
                    "External|25069"
                },
                Name = "Family Medicine - Willow Ave, Wheaton",
                Specialty = new()
                {
                    Title = "GE FAMILY PRACTICE",
                    Abbreviation = "GEFP",
                    Number = "10062",
                    ExternalName = string.Empty
                },
                OfficialTimeZone = new()
                {
                    Title = "America/Chicago",
                    Number = "5",
                    Abbreviation = "America/CHI"
                },
                Phones = new Phone[]
                {
                    new()
                    {
                        Number = "630-510-6900",
                        Type = "General"
                    },
                    new()
                    {
                        Number = string.Empty,
                        Type = "Scheduling"
                    }
                },
                Address = new()
                {
                    City = "WHEATON",
                    PostalCode = "60187-5476",
                    HouseNumber = string.Empty,
                    StreetAddress = new[]
                    {
                        "150 E WILLOW AVE",
                        "SUITE 300"
                    },
                    Country = new()
                    {
                        Title = "United States",
                        Number = "1",
                        Abbreviation = "USA"
                    },
                    County = new()
                    {
                        Title = "DuPage",
                        Number = "2",
                        Abbreviation = "DUPAGE"
                    },
                    District = new()
                    {
                        Title = string.Empty,
                        Number = string.Empty,
                        Abbreviation = string.Empty
                    },
                    State = new()
                    {
                        Title = "Illinois",
                        Number = "14",
                        Abbreviation = "IL"
                    }
                },
                LocationInstructions = new[]
                {
                    "150 E WILLOW AVE",
                    "SUITE 300",
                    "WHEATON, IL 60187",
                    string.Empty,
                    "Located on the corner of Main St and Willow Ave. (Where the old Jewel Store used to be). Parking located behind the building."
                }
            };
        }
    }
}
