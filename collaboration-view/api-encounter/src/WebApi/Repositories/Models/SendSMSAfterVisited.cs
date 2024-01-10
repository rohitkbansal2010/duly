// <copyright file="SendSmsAfterVisited.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    public class SendSmsAfterVisited
    {
        /// <summary>
        /// Gets or sets an unique token of configuration.
        /// </summary>
        public string ConfigurationToken { get; set; }

        /// <summary>
        /// Gets or sets an unique identifier of the request from workflow.
        /// </summary>
        public string CorrelationToken { get; set; }

        /// <summary>
        /// Gets or sets a collection of request parameters.
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; }
    }
}