// <copyright file="DbStringToBoolExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Api.Repositories.Mappings.Converters
{
    public static class DbStringToBoolExtensions
    {
        public static bool ConvertDbStringToBool(this string value)
        {
            return value?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }
    }
}
