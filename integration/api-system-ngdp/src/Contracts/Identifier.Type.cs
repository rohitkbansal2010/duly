// <copyright file="Identifier.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Ngdp.Contracts
{
    [Description("Description of identifier")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Enum in contracts")]
    public enum IdentifierType
    {
        [Description("EXTERNAL")]
        External,
        [Description("UNSPECIFIED")]
        Unspecified
    }
}