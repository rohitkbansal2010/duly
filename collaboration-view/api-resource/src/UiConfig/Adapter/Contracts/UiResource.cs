// <copyright file="UiResource.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.UiConfig.Adapter.Contracts
{
    /// <summary>
    /// User interface resource.
    /// </summary>
    public class UiResource
    {
        /// <summary>
        /// Resource identity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Resource alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// A UI resource type <see cref="UiResourceType"/>.
        /// </summary>
        public UiResourceType ResourceType { get; set; }

        /// <summary>
        /// Represents the details as a json string to be converted to the proper object, depending on the ResourceType.
        /// </summary>
        public string Details { get; set; }
    }
}