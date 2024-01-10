// <copyright file="HumanNameConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;

using AutoMapper;
using Duly.Clinic.Contracts;

using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    public class HumanNameConverter :
        ITypeConverter<R4.HumanName, HumanName>,
        ITypeConverter<STU3.HumanName, HumanName>
    {
        public HumanName Convert(R4.HumanName source, HumanName destination, ResolutionContext context)
        {
            return new HumanName
            {
                FamilyName = source.Family,
                GivenNames = context.Mapper.Map<string[]>(source.Given),
                Prefixes = context.Mapper.Map<string[]>(source.Prefix),
                Suffixes = context.Mapper.Map<string[]>(source.Suffix),
                Use = context.Mapper.Map<NameUse>(source.Use.GetValueOrDefault(R4.HumanName.NameUse.Usual))
            };
        }

        public HumanName Convert(STU3.HumanName source, HumanName destination, ResolutionContext context)
        {
            return new HumanName
            {
                FamilyName = source.Family,
                GivenNames = context.Mapper.Map<string[]>(source.Given),
                Prefixes = context.Mapper.Map<string[]>(source.Prefix),
                Suffixes = context.Mapper.Map<string[]>(source.Suffix),
                Use = context.Mapper.Map<NameUse>(source.Use.GetValueOrDefault(STU3.HumanName.NameUse.Usual))
            };
        }
    }
}