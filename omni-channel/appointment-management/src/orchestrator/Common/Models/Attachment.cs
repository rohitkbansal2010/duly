// <copyright file="Attachment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Models
{
    /// <summary>
    /// Represents a model of request attachment data.
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Gets or sets an original attachment name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets an unique identifier of attachment at storage provider.
        /// </summary>
        public string ReferenceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets an unique identifier of storage provider schema.
        /// </summary>
        public string ReferenceSchema { get; set; }

        /// <summary>
        /// Gets or sets an attachment MIME type.
        /// </summary>
        [Description("Attachment media type (https://www.iana.org/assignments/media-types/media-types.xhtml).")]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets an attachment sort order.
        /// </summary>
        public int? SortOrder { get; set; }

        /// <summary>
        /// Gets or sets attachment parameters.
        /// </summary>
        [JsonConverter(typeof(PolytypicParametersObjectConverter))]
        public IDictionary<string, string> Parameters { get; set; }
    }
}