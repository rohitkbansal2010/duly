// <copyright file="CodingsComparer.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Hl7.Fhir.Model;
using System.Collections.Generic;

namespace Duly.Clinic.Fhir.Adapter.Extensions
{
    public class CodingsComparer : IEqualityComparer<Coding>
    {
        public bool Equals(Coding x, Coding y)
        {
            if (x == null || y == null)
                return false;

            return x.System == y.System && x.Code == y.Code;
        }

        public int GetHashCode(Coding obj)
        {
            return obj.Code.GetHashCode() ^ obj.System.GetHashCode();
        }
    }
}
