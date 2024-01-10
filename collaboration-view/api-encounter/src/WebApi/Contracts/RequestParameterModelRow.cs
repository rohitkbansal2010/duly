// <copyright file="RequestParameterModelRow.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class RequestParameterModelRow
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public class ByNameComparer : IEqualityComparer<RequestParameterModelRow>
        {
            public bool Equals(RequestParameterModelRow x, RequestParameterModelRow y) =>
                x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase);

            public int GetHashCode(RequestParameterModelRow obj) =>
                obj.Name.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }
    }
}