// <copyright file="TimeSlotConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    public class TimeSlotConverter : ITypeConverter<Hl7.Fhir.Model.Period, TimeSlot>
    {
        public TimeSlot Convert(Hl7.Fhir.Model.Period source, TimeSlot destination, ResolutionContext context)
        {
            return new TimeSlot
            {
                StartTime = source.StartElement.BuildDateTimeOffset(),
                EndTime = source.EndElement.BuildDateTimeOffset()
            };
        }
    }
}