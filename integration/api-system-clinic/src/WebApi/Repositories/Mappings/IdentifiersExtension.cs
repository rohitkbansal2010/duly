// <copyright file="IdentifiersExtension.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Hl7.Fhir.Model;
using System.Collections.Generic;
using System.Linq;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Extensions for <see cref="Identifier"/>.
    /// </summary>
    public static class IdentifiersExtension
    {
        /// <summary>
        /// Build a string from <see cref="Identifier"/>.
        /// Format: identifier.Type.Text|identifier.Value.
        /// </summary>
        /// <param name="identifiers">Identifiers to convert.</param>
        /// <returns>An array of strings format: identifier.Type.Text|identifier.Value.</returns>
        public static string[] ConvertIdentifiers(this List<Identifier> identifiers)
        {
            return identifiers
                .Where(identifier => identifier.Type != null)
                .Select(identifier => $"{identifier.Type.Text}|{identifier.Value}").ToArray();
        }
    }
}