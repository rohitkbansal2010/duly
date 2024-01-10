// <copyright file="UnitsOfTimeConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Repositories.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    internal class UnitsOfTimeConverterTests : MapperConfigurator<SystemContractsToRepositoryModelsProfile>
    {
        [TestCase(UnitsOfTime.S, Models.UnitsOfTime.S)]
        [TestCase(UnitsOfTime.Min, Models.UnitsOfTime.Min)]
        [TestCase(UnitsOfTime.H, Models.UnitsOfTime.H)]
        [TestCase(UnitsOfTime.D, Models.UnitsOfTime.D)]
        [TestCase(UnitsOfTime.Wk, Models.UnitsOfTime.Wk)]
        [TestCase(UnitsOfTime.Mo, Models.UnitsOfTime.Mo)]
        [TestCase(UnitsOfTime.A, Models.UnitsOfTime.A)]
        public void ConvertTest(UnitsOfTime source, Models.UnitsOfTime expected)
        {
            //Arrange

            //Act
            var result = Mapper.Map<Models.UnitsOfTime>(source);

            //Assert
            result.Should().Be(expected);
        }
    }
}
