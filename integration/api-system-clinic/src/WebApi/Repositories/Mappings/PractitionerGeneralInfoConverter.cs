// <copyright file="PractitionerGeneralInfoConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;

using AutoMapper;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Hl7.Fhir.Model;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps http://hl7.org/fhir/StructureDefinition/Practitioners with http://hl7.org/fhir/StructureDefinition/PractitionerRole
    /// into <see cref="PractitionerGeneralInfo"/>.
    /// </summary>
    public class PractitionerGeneralInfoConverter :
        ITypeConverter<PractitionerWithRoles, PractitionerGeneralInfo>,
        ITypeConverter<PractitionerWithRolesSTU3, PractitionerGeneralInfo>
    {
        public PractitionerGeneralInfo Convert(PractitionerWithRoles source, PractitionerGeneralInfo destination, ResolutionContext context)
        {
            var practitionerGeneralInfo = new PractitionerGeneralInfo
            {
                Id = source.Resource.Id,
                Names = context.Mapper.Map<HumanName[]>(source.Resource.Name),
                Photos = context.Mapper.Map<Attachment[]>(source.Resource.Photo),
                Roles = ConvertRoles(source.Roles),
                Identifiers = source.Resource.Identifier.ConvertIdentifiers(),
                Speciality = BuildSpeciality(source.Roles)
            };

            return practitionerGeneralInfo;
        }

        public PractitionerGeneralInfo Convert(PractitionerWithRolesSTU3 source, PractitionerGeneralInfo destination, ResolutionContext context)
        {
            var practitionerGeneralInfo = new PractitionerGeneralInfo
            {
                Id = source.Resource.Id,
                Names = context.Mapper.Map<HumanName[]>(source.Resource.Name),
                Photos = context.Mapper.Map<Attachment[]>(source.Resource.Photo),
                Roles = ConvertRoles(source.Roles)
            };

            return practitionerGeneralInfo;
        }

        private static Role[] ConvertRoles(IEnumerable<R4.PractitionerRole> practitionerRoles)
        {
            var roles = new List<Role>();
            foreach (var practitionerRole in practitionerRoles)
            {
                roles.AddRange(ExtractRoles(practitionerRole.Code));
            }

            return CheckRoles(roles);
        }

        private static Role[] ConvertRoles(IEnumerable<STU3.PractitionerRole> practitionerRoles)
        {
            var roles = new List<Role>();
            foreach (var practitionerRole in practitionerRoles)
            {
                roles.AddRange(ExtractRoles(practitionerRole.Code));
            }

            return CheckRoles(roles);
        }

        private static IEnumerable<Role> ExtractRoles(IEnumerable<CodeableConcept> codeableConcepts)
        {
            return codeableConcepts
                .Where(codeableConcept => !string.IsNullOrEmpty(codeableConcept.Text))
                .Select(codeableConcept => new Role { Title = codeableConcept.Text });
        }

        private static Role[] CheckRoles(List<Role> roles)
        {
            return roles.Count == 0 ? null : roles.ToArray();
        }

        private List<string> BuildSpeciality(IEnumerable<R4.PractitionerRole> practitionerRoles)
        {
            var Speciality = new List<string>();
            foreach (var item in practitionerRoles)
            {
                foreach (var specialty in item.Specialty)
                {
                    Speciality.Add(specialty.Text);
                }
            }

            return Speciality;
        }
    }
}