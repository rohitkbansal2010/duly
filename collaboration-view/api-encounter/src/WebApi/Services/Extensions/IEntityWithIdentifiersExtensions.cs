// <copyright file="IEntityWithIdentifiersExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Services.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IEntityWithIdentifiers"/>.
    /// </summary>
    public static class IEntityWithIdentifiersExtensions
    {
        /// <summary>
        /// Convert to <see cref="IReadOnlyDictionary{TKey,TValue}"/> the <see cref="source"/> including <see cref="idPrefix"/>.
        /// </summary>
        /// <typeparam name="T">Instance of <see cref="IEntityWithIdentifiers"/>.</typeparam>
        /// <param name="source">Collections to modify.</param>
        /// <param name="idPrefix">Required prefix.</param>
        /// <returns>Collection of items with key.</returns>
        internal static IReadOnlyDictionary<string, T> CollectEntitiesDictionary<T>(this IEnumerable<T> source, string idPrefix)
            where T : IEntityWithIdentifiers
        {
            var sourceDictionary = source
                .Where(s => s.Identifiers != null)
                .SelectMany(
                    s => s.Identifiers.Where(ei => ei.StartsWith(idPrefix)),
                    (e, id) => new
                    {
                        ExternalId = id,
                        Entity = e
                    })
                .Distinct()
                .ToDictionary(ep => ep.ExternalId[idPrefix.Length..], ep => ep.Entity);

            return sourceDictionary;
        }
    }
}
