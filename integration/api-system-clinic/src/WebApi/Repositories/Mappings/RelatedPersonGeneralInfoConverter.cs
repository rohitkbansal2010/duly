// <copyright file="RelatedPersonGeneralInfoConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    public class RelatedPersonGeneralInfoConverter : ITypeConverter<RelatedPersonWithCompartments, RelatedPersonGeneralInfo>
    {
        public RelatedPersonGeneralInfo Convert(RelatedPersonWithCompartments source, RelatedPersonGeneralInfo destination, ResolutionContext context)
        {
            var person = new RelatedPersonGeneralInfo
            {
                Id = source.Resource.Id,
                Names = context.Mapper.Map<HumanName[]>(source.Resource.Name),
                Photos = context.Mapper.Map<Attachment[]>(source.Resource.Photo)
            };

            return person;
        }
    }
}
