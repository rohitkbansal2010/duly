// <copyright file="IdentityExtension.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Wipfli.Adapter.Client;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    /// <summary>
    /// Extensions for <see cref="Identity"/>.
    /// </summary>
    public static class IdentityExtension
    {
        /// <summary>
        /// Extract a <see cref="Identity"/> from string.
        /// From format: identifier.Type|identifier.ID.
        /// </summary>
        /// <param name="identifier">Identifier to extract.</param>
        /// <returns>An instance of <see cref="Identity"/>.</returns>
        public static Identity SplitIdentifier(this string identifier)
        {
            var splitIdentifier = identifier.Split("|");
            return new Identity
            {
                ID = splitIdentifier[1],
                Type = splitIdentifier[0]
            };
        }
    }
}
