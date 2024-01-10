// <copyright file="RepeatConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias stu3;

using AutoMapper;
using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Linq;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps
    ///     https://www.hl7.org/fhir/datatypes-definitions.html#Timing.repeat
    /// into <see cref="Repeat"/>.
    /// </summary>
    public class RepeatConverter : ITypeConverter<STU3.Timing.RepeatComponent, Repeat>
    {
        public Repeat Convert(STU3.Timing.RepeatComponent source, Repeat destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            return new Repeat
            {
                BoundsPeriod = FindPeriod(source.Bounds, context.Mapper),
                Count = source.Count,
                CountMax = source.CountMax,
                DaysOfWeek = FindDaysOfWeek(source.DayOfWeek, context.Mapper),
                Duration = source.Duration,
                Frequency = source.Frequency,
                Period = source.Period,
                PeriodUnit = FindPeriodUnit(source.PeriodUnit, context.Mapper),
                TimesOfDay = source.TimeOfDay.ToArray(),
                When = FindWhen(source.When, context.Mapper)
            };
        }

        private static Period FindPeriod(Hl7.Fhir.Model.DataType sourceBounds, IRuntimeMapper contextMapper)
        {
            return sourceBounds switch
            {
                null => default,
                Hl7.Fhir.Model.Period period => contextMapper.Map<Period>(period),
                _ => throw new ConceptNotMappedException("Could not map sourceBounds to Period")
            };
        }

        private static UnitsOfTime? FindPeriodUnit(STU3.Timing.UnitsOfTime? sourcePeriodUnit, IRuntimeMapper contextMapper)
        {
            if (!sourcePeriodUnit.HasValue)
                return null;

            return contextMapper.Map<UnitsOfTime>(sourcePeriodUnit.Value);
        }

        private static DaysOfWeek[] FindDaysOfWeek(IEnumerable<STU3.DaysOfWeek?> sourceDayOfWeek, IRuntimeMapper contextMapper)
        {
            return contextMapper.Map<DaysOfWeek[]>(sourceDayOfWeek.Where(week => week.HasValue).Select(week => week.Value));
        }

        private static EventTiming[] FindWhen(IEnumerable<STU3.Timing.EventTiming?> sourceWhen, IRuntimeMapper contextMapper)
        {
            return contextMapper.Map<EventTiming[]>(sourceWhen.Where(timing => timing.HasValue).Select(timing => timing.Value));
        }
    }
}