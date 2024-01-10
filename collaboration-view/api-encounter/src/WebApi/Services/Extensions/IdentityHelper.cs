// <copyright file="IdentityHelper.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Services.Extensions
{
    public static class IdentityHelper
    {
        public const string ExternalIdPrefix = "EXTERNAL|";

        public static string ToIdWithExternalPrefix(this string id)
        {
            return $"{ExternalIdPrefix}{id}";
        }

        public static string FindIdWithExternalPrefix(this string[] ids)
        {
            return Array.Find(ids, s => s.StartsWith(ExternalIdPrefix));
        }

        public static string SplitIdWithExternalPrefix(this string externalIdWithPrefix)
        {
            return string.IsNullOrEmpty(externalIdWithPrefix) ? externalIdWithPrefix : externalIdWithPrefix.Replace(ExternalIdPrefix, string.Empty);
        }
    }
}
