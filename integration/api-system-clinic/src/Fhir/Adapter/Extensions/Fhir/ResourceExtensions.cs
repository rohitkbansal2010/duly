// <copyright file="ResourceExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Extensions.Fhir
{
    public static class ResourceExtensions
    {
        /// <summary>
        /// Builds simple reference based on typeName.
        /// </summary>
        /// <param name="res">Resource.</param>
        /// <returns>String in Resource/id format.</returns>
        public static string ToReference(this Resource res)
        {
            return $"{res.TypeName}/{res.Id}";
        }
    }
}