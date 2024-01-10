// <copyright file="ContactPoint.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Ngdp.Contracts
{
    [Description("Telecommunications form for contact point.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Enum in contracts")]
    public enum ContactPointType
    {
        [Description("Telephone")]
        Phone,

        [Description("Email")]
        Email
    }
}