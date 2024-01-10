// <copyright file="Participants.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class ParticipantsExampleProvider : IExamplesProvider<IEnumerable<Participant>>
    {
        public IEnumerable<Participant> GetExamples()
        {
            var provider = new PractitionerGeneralInfoExampleProvider();
            yield return new Participant
            {
                Person = provider.GetExamples()
            };

            var rpProvider = new RelatedPersonGeneralInfoExampleProvider();
            yield return new Participant
            {
                Person = rpProvider.GetExamples()
            };
        }
    }
}