// <copyright file="RepeatConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias stu3;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class RepeatConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void Convert_BoundsIsNotPeriod_Test()
        {
            //Arrange
            var source = new STU3.Timing.RepeatComponent
            {
                Bounds = new STU3.Duration()
            };

            //Act
            Action action = () => Mapper.Map<Repeat>(source);

            //Assert
            var exception = action.Should().Throw<ConceptNotMappedException>();
            exception.Which.Message.Should().Be("Could not map sourceBounds to Period");
        }

        [Test]
        public void ConvertTest()
        {
            //Arrange
            var source = new STU3.Timing.RepeatComponent
            {
                Bounds = new Hl7.Fhir.Model.Period(),
                DayOfWeek = new[]
                {
                    (STU3.DaysOfWeek?)null,
                    STU3.DaysOfWeek.Sat
                },
                When = new[]
                {
                    (STU3.Timing.EventTiming?)null,
                    STU3.Timing.EventTiming.AFT
                },
                Count = 23,
                CountMax = 234,
                Duration = decimal.MinusOne * 2,
                Frequency = 2,
                Period = decimal.One * 3,

                TimeOfDay = new[]
                {
                    "One",
                    "Two"
                },
                PeriodUnit = STU3.Timing.UnitsOfTime.Mo
            };

            //Act
            var result = Mapper.Map<Repeat>(source);

            //Assert
            result.Count.Should().Be(source.Count);
            result.CountMax.Should().Be(source.CountMax);
            result.Duration.Should().Be(source.Duration);
            result.Frequency.Should().Be(source.Frequency);
            result.Period.Should().Be(source.Period);
            result.TimesOfDay.First().Should().Be(source.TimeOfDay.First());
            result.BoundsPeriod.Should().NotBeNull();
            result.DaysOfWeek.First().Should().Be(DaysOfWeek.Sat);
            result.When.First().Should().Be(EventTiming.AFT);
            result.PeriodUnit.Should().Be(UnitsOfTime.Mo);
        }

        [Test]
        public void Convert_Empty_Test()
        {
            //Arrange
            var source = new STU3.Timing.RepeatComponent();

            //Act
            var result = Mapper.Map<Repeat>(source);

            //Assert
            result.BoundsPeriod.Should().BeNull();
        }
    }
}