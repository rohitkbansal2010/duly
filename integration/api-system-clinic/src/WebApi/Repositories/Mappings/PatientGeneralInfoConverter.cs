// <copyright file="PatientGeneralInfoConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using AutoMapper;
using Duly.Clinic.Contracts;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    public class PatientGeneralInfoConverter : ITypeConverter<R4.Patient, PatientGeneralInfo>
    {
        public PatientGeneralInfo Convert(R4.Patient source, PatientGeneralInfo destination, ResolutionContext context)
        {
            var patient = new PatientGeneralInfo
            {
                Id = source.Id,
                Names = context.Mapper.Map<HumanName[]>(source.Name)
            };
            return patient;
        }
    }
}