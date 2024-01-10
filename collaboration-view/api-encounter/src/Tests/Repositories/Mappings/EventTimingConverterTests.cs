// <copyright file="EventTimingConverterTests.cs" company="Duly Health and Care">
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
    internal class EventTimingConverterTests : MapperConfigurator<SystemContractsToRepositoryModelsProfile>
    {
        [TestCase(EventTiming.MORN, Models.EventTiming.MORN)]
        [TestCase(EventTiming.MORN_early, Models.EventTiming.MORN_early)]
        [TestCase(EventTiming.MORN_late, Models.EventTiming.MORN_late)]
        [TestCase(EventTiming.NOON, Models.EventTiming.NOON)]
        [TestCase(EventTiming.AFT, Models.EventTiming.AFT)]
        [TestCase(EventTiming.AFT_early, Models.EventTiming.AFT_early)]
        [TestCase(EventTiming.AFT_late, Models.EventTiming.AFT_late)]
        [TestCase(EventTiming.EVE, Models.EventTiming.EVE)]
        [TestCase(EventTiming.EVE_early, Models.EventTiming.EVE_early)]
        [TestCase(EventTiming.EVE_late, Models.EventTiming.EVE_late)]
        [TestCase(EventTiming.NIGHT, Models.EventTiming.NIGHT)]
        [TestCase(EventTiming.PHS, Models.EventTiming.PHS)]
        [TestCase(EventTiming.HS, Models.EventTiming.HS)]
        [TestCase(EventTiming.WAKE, Models.EventTiming.WAKE)]
        [TestCase(EventTiming.C, Models.EventTiming.C)]
        [TestCase(EventTiming.CM, Models.EventTiming.CM)]
        [TestCase(EventTiming.CD, Models.EventTiming.CD)]
        [TestCase(EventTiming.CV, Models.EventTiming.CV)]
        [TestCase(EventTiming.AC, Models.EventTiming.AC)]
        [TestCase(EventTiming.ACM, Models.EventTiming.ACM)]
        [TestCase(EventTiming.ACD, Models.EventTiming.ACD)]
        [TestCase(EventTiming.ACV, Models.EventTiming.ACV)]
        [TestCase(EventTiming.PC, Models.EventTiming.PC)]
        [TestCase(EventTiming.PCM, Models.EventTiming.PCM)]
        [TestCase(EventTiming.PCD, Models.EventTiming.PCD)]
        [TestCase(EventTiming.PCV, Models.EventTiming.PCV)]
        public void ConvertTest(EventTiming source, Models.EventTiming expected)
        {
            //Arrange

            //Act
            var result = Mapper.Map<Models.EventTiming>(source);

            //Assert
            result.Should().Be(expected);
        }
    }
}
