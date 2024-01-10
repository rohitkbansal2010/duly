// <copyright file="CareTeamParticipantConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace Duly.Clinic.Api.Client
{
    /// <summary>
    /// A converter to correctly deserialize CareTeamParticipant objects.
    /// </summary>
    internal class CareTeamParticipantConverter : JsonConverter<CareTeamParticipant>
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON.
        /// </summary>
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, CareTeamParticipant? value, JsonSerializer serializer)
        {
        }

        public override CareTeamParticipant? ReadJson(JsonReader reader, Type objectType, CareTeamParticipant? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = serializer.Deserialize<JObject>(reader);
            if (jObject == null)
            {
                throw new JsonSerializationException("CareTeamParticipant does not exist");
            }

            if (!jObject.TryGetValue(nameof(existingValue.MemberRole), StringComparison.OrdinalIgnoreCase, out var token))
            {
                throw new JsonSerializationException("CareTeamParticipant must contain MemberRole");
            }

            var memberRole = token.ToObject<MemberRole>();
            CareTeamMember member = memberRole switch
            {
                MemberRole.Practitioner => new PractitionerInCareTeam { Practitioner = new PractitionerGeneralInfo() },
                _ => throw new ArgumentOutOfRangeException(nameof(memberRole))
            };

            var personToken = jObject.GetValue(nameof(existingValue.Member), StringComparison.OrdinalIgnoreCase);
            if (personToken == null)
            {
                throw new JsonSerializationException("Participant must contain Member");
            }

            serializer.Populate(personToken.CreateReader(), member);
            return new CareTeamParticipant { MemberRole = memberRole, Member = member };
        }
    }
}
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
