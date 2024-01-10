// <copyright file="PeriodConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    public class PeriodConverter : ITypeConverter<Hl7.Fhir.Model.Period, Period>
    {
        public Period Convert(Hl7.Fhir.Model.Period source, Period destination, ResolutionContext context)
        {
            return new Period
            {
                Start = source.StartElement?.Value == null ? null : source.StartElement.BuildDateTimeOffset(),
                End = source.EndElement?.Value == null ? null : source.EndElement.BuildDateTimeOffset()
            };
        }
    }
}