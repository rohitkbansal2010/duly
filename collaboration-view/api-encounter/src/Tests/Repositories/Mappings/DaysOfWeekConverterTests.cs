// <copyright file="DaysOfWeekConverterTests.cs" company="Duly Health and Care">
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
    internal class DaysOfWeekConverterTests : MapperConfigurator<SystemContractsToRepositoryModelsProfile>
    {
        [TestCase(DaysOfWeek.Sun, Models.DaysOfWeek.Sun)]
        [TestCase(DaysOfWeek.Mon, Models.DaysOfWeek.Mon)]
        [TestCase(DaysOfWeek.Tue, Models.DaysOfWeek.Tue)]
        [TestCase(DaysOfWeek.Wed, Models.DaysOfWeek.Wed)]
        [TestCase(DaysOfWeek.Thu, Models.DaysOfWeek.Thu)]
        [TestCase(DaysOfWeek.Fri, Models.DaysOfWeek.Fri)]
        [TestCase(DaysOfWeek.Sat, Models.DaysOfWeek.Sat)]
        public void ConvertTest(DaysOfWeek source, Models.DaysOfWeek expected)
        {
            //Arrange

            //Act
            var result = Mapper.Map<Models.DaysOfWeek>(source);

            //Assert
            result.Should().Be(expected);
        }
    }
}
