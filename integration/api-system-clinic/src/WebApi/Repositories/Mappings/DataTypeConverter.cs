// <copyright file="DataTypeConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using Hl7.Fhir.Model;
using System;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    public class DataTypeConverter : ITypeConverter<DataType, DateTimeOffset>
    {
        public DateTimeOffset Convert(DataType source, DateTimeOffset destination, ResolutionContext context)
        {
            return source switch
            {
                FhirDateTime dateTime => dateTime.BuildDateTimeOffset(),
                _ => throw new ConceptNotMappedException("Could not map DataType")
            };
        }
    }
}