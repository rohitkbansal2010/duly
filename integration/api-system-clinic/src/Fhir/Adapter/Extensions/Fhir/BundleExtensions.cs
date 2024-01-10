// <copyright file="BundleExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
extern alias stu3;

using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir;
using STU3 = stu3::Hl7.Fhir;

namespace Duly.Clinic.Fhir.Adapter.Extensions.Fhir
{
    public static class BundleExtensions
    {
        private static readonly Type _operationOutcomeType = typeof(OperationOutcome);

        /// <summary>
        /// Extract items of <see cref="STU3.Model.Bundle.EntryComponent"/> from <see cref="STU3.Model.Bundle.Entry"/>.
        /// Exclude resources of the type <see cref="OperationOutcome"/>.
        /// </summary>
        /// <param name="result">Incoming bundle with resources.</param>
        /// <returns>Filtered components.</returns>
        public static IEnumerable<STU3.Model.Bundle.EntryComponent> ExtractEntryComponents(this STU3.Model.Bundle result)
        {
            return result.Entry.Where(entryComponent => entryComponent.Resource.GetType() != _operationOutcomeType);
        }

        /// <summary>
        /// Extract items of <see cref="R4.Model.Bundle.EntryComponent"/> from <see cref="R4.Model.Bundle.Entry"/>.
        /// Exclude resources of the type <see cref="OperationOutcome"/>.
        /// </summary>
        /// <param name="result">Incoming bundle with resources.</param>
        /// <returns>Filtered components.</returns>
        public static IEnumerable<R4.Model.Bundle.EntryComponent> ExtractEntryComponents(this R4.Model.Bundle result)
        {
            return result.Entry.Where(entryComponent => entryComponent.Resource.GetType() != _operationOutcomeType);
        }

        /// <summary>
        /// Extract items of <see cref="T"/> from <see cref="R4.Model.Bundle.Entry"/>.
        /// Exclude resources of other types.
        /// </summary>
        /// <typeparam name="T">Type of desired entities.</typeparam>
        /// <param name="result">Incoming bundle with resources.</param>
        /// <returns>Filtered resource of type <see cref="T"/>.</returns>
        public static IEnumerable<T> ExtractResource<T>(this R4.Model.Bundle result)
        {
            return result.Entry.Select(component => component.Resource).OfType<T>();
        }
    }
}