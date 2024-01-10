// <copyright file="AzureActiveDirectoryUserAccountIdentityConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.Common.Security.Entities;
using System;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    /// <summary>
    /// Maps <see cref="AzureActiveDirectoryUserAccountIdentity"/> into <see cref="PractitionerGeneralInfo"/>.
    /// </summary>
    internal class AzureActiveDirectoryUserAccountIdentityConverter :
        ITypeConverter<AzureActiveDirectoryUserAccountIdentity, PractitionerGeneralInfo>,
        ITypeConverter<AzureActiveDirectoryUserAccountIdentity, Party>,
        ITypeConverter<AzureActiveDirectoryUserAccountIdentity, User>
    {
        public PractitionerGeneralInfo Convert(AzureActiveDirectoryUserAccountIdentity source, PractitionerGeneralInfo destination, ResolutionContext context)
        {
            return new PractitionerGeneralInfo
            {
                Id = source.Id,
                HumanName = BuildHumanName(source),
                Photo = BuildAttachment(source),
                Role = PractitionerRole.Unknown
            };
        }

        public Party Convert(AzureActiveDirectoryUserAccountIdentity source, Party destination, ResolutionContext context)
        {
            return new Party
            {
                Id = source.Id,
                HumanName = BuildHumanName(source),
                Photo = BuildAttachment(source),
                MemberType = MemberType.Unknown,
                Role = PractitionerRole.Unknown
            };
        }

        public User Convert(AzureActiveDirectoryUserAccountIdentity source, User destination, ResolutionContext context)
        {
            return new User
            {
                Id = source.Id,
                Name = BuildHumanName(source),
                Photo = BuildAttachment(source),
            };
        }

        private static HumanName BuildHumanName(AzureActiveDirectoryUserAccountIdentity source)
        {
            return new HumanName
            {
                FamilyName = source.LastName,
                GivenNames = source.FirstName == null ? Array.Empty<string>() : new[] { source.FirstName },
                Suffixes = Array.Empty<string>(),
                Prefixes = Array.Empty<string>()
            };
        }

        private static Attachment BuildAttachment(AzureActiveDirectoryUserAccountIdentity source)
        {
            return source.Photo == null
                ? null
                : new Attachment
                {
                    ContentType = source.ContentType,
                    Data = source.Photo
                };
        }
    }
}