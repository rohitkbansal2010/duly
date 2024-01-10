// <copyright file="CareTeamParticipantsExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    public class CareTeamParticipantsExampleProvider : IExamplesProvider<IEnumerable<CareTeamParticipant>>
    {
        public IEnumerable<CareTeamParticipant> GetExamples()
        {
            var provider = new PractitionerGeneralInfoExampleProvider();
            yield return new CareTeamParticipant
            {
                Member = new PractitionerInCareTeam { Practitioner = provider.GetExamples() }
            };
        }
    }
}