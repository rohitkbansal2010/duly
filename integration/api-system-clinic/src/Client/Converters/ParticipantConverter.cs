// <copyright file="ParticipantConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace Duly.Clinic.Api.Client
{
    /// <summary>
    /// A converter to correctly deserialize Participant objects.
    /// </summary>
    internal class ParticipantConverter : JsonConverter<Participant>
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON.
        /// </summary>
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, Participant? value, JsonSerializer serializer)
        {
        }

        public override Participant? ReadJson(JsonReader reader, Type objectType, Participant? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = serializer.Deserialize<JObject>(reader);
            if (jObject == null)
            {
                throw new JsonSerializationException("Participant does not exist");
            }

            if (!jObject.TryGetValue(nameof(existingValue.MemberType), StringComparison.OrdinalIgnoreCase, out var token))
            {
                throw new JsonSerializationException("Participant must contain MemberType");
            }

            var memberType = token.ToObject<MemberType>();
            HumanGeneralInfoWithPhoto person = memberType switch
            {
                MemberType.Practitioner => new PractitionerGeneralInfo(),
                MemberType.RelatedPerson => new RelatedPersonGeneralInfo(),
                _ => throw new ArgumentOutOfRangeException(nameof(memberType))
            };

            var personToken = jObject.GetValue(nameof(existingValue.Person), StringComparison.OrdinalIgnoreCase);
            if (personToken == null)
            {
                throw new JsonSerializationException("Participant must contain person");
            }

            serializer.Populate(personToken.CreateReader(), person);
            return new Participant { MemberType = memberType, Person = person };
        }
    }
}
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
