// <copyright file="GeneralInfoConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    /// <summary>
    /// Maps <see cref="Models.PractitionerGeneralInfo"/> into <see cref="PractitionerGeneralInfo"/>.
    /// Maps <see cref="Models.PractitionerGeneralInfo"/>  into <see cref="Party"/>.
    /// Maps <see cref="PractitionerGeneralInfo"/> into <see cref="Party"/>.
    /// </summary>
    internal class GeneralInfoConverter :
        ITypeConverter<Models.PractitionerGeneralInfo, PractitionerGeneralInfo>,
        ITypeConverter<Models.PractitionerGeneralInfo, Party>,
        ITypeConverter<PractitionerGeneralInfo, Party>
    {
        private const string PrimaryCarePhysicianNamePrefixTemplate = "Dr.";
        private const string PrimaryCarePhysicianRoleTitleTemplate = "physician";

        public PractitionerGeneralInfo Convert( Models.PractitionerGeneralInfo source, PractitionerGeneralInfo destination, ResolutionContext context)
        {
            var practitionerGeneralInfo = new PractitionerGeneralInfo
            {
                Id = source.Id,
                Photo = context.Mapper.Map<Attachment>(SelectAttachment(source.Photos)),
                HumanName = ResolvePractitionerHumanName(source.Names, source.Roles, context.Mapper),
                Role = ConvertPractitionerRoles(source.Roles),
                Speciality = BuildSpecialityProvider(source)
            };

            return practitionerGeneralInfo;
        }

   
        public Party Convert(Models.PractitionerGeneralInfo source, Party destination, ResolutionContext context)
        {
            var party = new Party
            {
                Id = source.Id,
                Photo = context.Mapper.Map<Attachment>(SelectAttachment(source.Photos)),
                HumanName = ResolvePractitionerHumanName(source.Names, source.Roles, context.Mapper),
                MemberType = MemberType.Doctor,
                Role = ConvertPractitionerRoles(source.Roles),
                Speciality = source.Speciality
            };

            return party;
        }

        public Party Convert(PractitionerGeneralInfo source, Party destination, ResolutionContext context)
        {
            var party = new Party
            {
                Id = source.Id,
                Photo = source.Photo,
                HumanName = source.HumanName,
                MemberType = MemberType.Doctor,
                Role = source.Role,
                Speciality = source.Speciality
            };
            return party;
        }

        private static HumanName ResolvePractitionerHumanName(
            Models.HumanName[] names,
            IEnumerable<Models.Role> practitionerRoles,
            IRuntimeMapper contextMapper)
        {
            var practitionerHumanName = HumanNamesSelector.SelectHumanNameByUse(names);

            var resolvePractitionerHumanName = new HumanName
            {
                FamilyName = practitionerHumanName.FamilyName,
                GivenNames = contextMapper.Map<string[]>(practitionerHumanName.GivenNames),
                Suffixes = contextMapper.Map<string[]>(practitionerHumanName.Suffixes)
            };

            if (!IsPrimaryCarePhysician(practitionerRoles))
            {
                return resolvePractitionerHumanName;
            }

            var doctorPrefix = practitionerHumanName.Prefixes?.FirstOrDefault();
            var prefix = string.IsNullOrWhiteSpace(doctorPrefix) ? PrimaryCarePhysicianNamePrefixTemplate : doctorPrefix;
            resolvePractitionerHumanName.Prefixes = new[] { prefix };

            return resolvePractitionerHumanName;
        }

        private static PractitionerRole ConvertPractitionerRoles(IEnumerable<Models.Role> practitionerRoles)
        {
            return IsPrimaryCarePhysician(practitionerRoles) ? PractitionerRole.PrimaryCarePhysician : PractitionerRole.MedicalAssistant;
        }

        private static bool IsPrimaryCarePhysician(IEnumerable<Models.Role> practitionerRoles)
        {
            return practitionerRoles != null &&
                   practitionerRoles.Any(x =>
                       x.Title.Contains(PrimaryCarePhysicianRoleTitleTemplate, StringComparison.OrdinalIgnoreCase));
        }

        private static Models.Attachment SelectAttachment(Models.Attachment[] sourcePhotos)
        {
            return sourcePhotos?.FirstOrDefault();
        }

        private List<string> BuildSpecialityProvider(Models.PractitionerGeneralInfo practitionerRoles)
        {
            var Speciality = new List<string>();
            foreach (var item in practitionerRoles.Speciality)
            {
                Speciality.Add(item);
            }

            return Speciality;
        }
    }
}