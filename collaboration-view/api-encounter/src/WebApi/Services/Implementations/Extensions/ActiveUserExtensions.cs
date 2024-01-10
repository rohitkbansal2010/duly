// <copyright file="ActiveUserExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions
{
    public static class ActiveUserExtensions
    {
        /// <summary>
        /// Try to find matching between <see cref="activeUser"/> in <see cref="parties"/> by <see cref="HumanName.FamilyName"/> and <see cref="HumanName.GivenNames"/>.
        /// </summary>
        /// <param name="parties">Array of searching.</param>
        /// <param name="activeUser">User to search.</param>
        /// <returns>If match return item from <see cref="parties"/>.</returns>
        public static Party FindExistingParty(this Party[] parties, Party activeUser)
        {
            return Array.Find(parties, party =>
                AreHumanNameEquivalent(party.HumanName, activeUser.HumanName));
        }

        /// <summary>
        /// Try to find matching between <see cref="activeUser"/> in <see cref="practitionerGeneralInfos"/> by <see cref="HumanName.FamilyName"/> and <see cref="HumanName.GivenNames"/>.
        /// </summary>
        /// <param name="practitionerGeneralInfos">Array of searching.</param>
        /// <param name="activeUser">User to search.</param>
        /// <returns>If match return item from <see cref="practitionerGeneralInfos"/>.</returns>
        public static PractitionerGeneralInfo FindExistingPractitionerGeneralInfo(this PractitionerGeneralInfo[] practitionerGeneralInfos, PractitionerGeneralInfo activeUser)
        {
            return Array.Find(practitionerGeneralInfos, info =>
                AreHumanNameEquivalent(info.HumanName, activeUser.HumanName));
        }

        /// <summary>
        /// Copy properties:
        ///     <see cref="PractitionerGeneralInfo.Role"/>,
        ///     <see cref="PractitionerGeneralInfo.Id"/>,
        ///     <see cref="HumanName.Prefixes"/>,
        ///     <see cref="HumanName.Suffixes"/>
        /// from <see cref="source"/> to <see cref="target"/>.
        /// </summary>
        /// <param name="target">Where the properties will be copied.</param>
        /// <param name="source">The source of data to copy. If equals null, skip the copying.</param>
        public static void CopyProperties(
            this PractitionerGeneralInfo target,
            PractitionerGeneralInfo source)
        {
            if (source == null)
            {
                return;
            }

            target.Role = source.Role;
            target.Id = source.Id;
            target.HumanName.Prefixes = source.HumanName.Prefixes;
            target.HumanName.Suffixes = source.HumanName.Suffixes;
        }

        /// <summary>
        /// Copy properties:
        ///     <see cref="Party.MemberType"/>,
        ///     <see cref="Party.Id"/>,
        ///     <see cref="HumanName.Prefixes"/>,
        ///     <see cref="HumanName.Suffixes"/>
        /// from <see cref="source"/> to <see cref="target"/>.
        /// </summary>
        /// <param name="target">Where the properties will be copied.</param>
        /// <param name="source">The source of data to copy. If equals null, skip the copying.</param>
        public static void CopyProperties(
            this Party target,
            Party source)
        {
            if (source == null)
            {
                return;
            }

            target.MemberType = source.MemberType;
            target.Id = source.Id;
            target.HumanName.Prefixes = source.HumanName.Prefixes;
            target.HumanName.Suffixes = source.HumanName.Suffixes;
        }

        private static bool AreHumanNameEquivalent(HumanName partyHumanName, HumanName activeUserHumanName)
        {
            return partyHumanName.FamilyName == activeUserHumanName.FamilyName &&
                activeUserHumanName.GivenNames.All(name =>
                    partyHumanName.GivenNames.Contains(name));
        }
    }
}
