// <copyright file="UiConfiguration.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.UiConfig.Adapter.Contracts
{
    /// <summary>
    /// User interface configuration.
    /// </summary>
    public class UiConfiguration
    {
        /// <summary>
        /// Configuration identity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// An application part <see cref="ApplicationPart"/>.
        /// </summary>
        public ApplicationPart ApplicationPart { get; set; }

        /// <summary>
        /// A UI configuration type <see cref="UiConfigurationType"/>.
        /// </summary>
        public UiConfigurationType ConfigType { get; set; }

        /// <summary>
        /// A UI configuration target area type <see cref="UiConfigurationTargetAreaType"/>.
        /// </summary>
        public UiConfigurationTargetAreaType TargetAreaType { get; set; }

        /// <summary>
        /// A UI configuration target type <see cref="UiConfigurationTargetType"/>.
        /// </summary>
        public UiConfigurationTargetType TargetType { get; set; }

        /// <summary>
        /// An Id of a site for which this configuration is intended.
        /// </summary>
        public string SiteId { get; set; }

        /// <summary>
        /// An Id of a Patient for which this configuration is intended.
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// An Id of the parent UiConfiguration (if exists).
        /// </summary>
        public Guid? ParentConfigurationId { get; set; }

        /// <summary>
        /// An Id of the target of the parent UiConfiguration item.
        /// </summary>
        public Guid? ParentTargetId { get; set; }

        /// <summary>
        /// Comma separated tags.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// UI configuration items.
        /// </summary>
        public UiConfigurationItem[] Items { get; set; }

        /// <summary>
        /// Represents the details as a json string to be converted to the proper object,
        /// depending on the combination of the following fields: TargetAreaType and TargetType.
        /// </summary>
        public string Details { get; set; }
    }
}