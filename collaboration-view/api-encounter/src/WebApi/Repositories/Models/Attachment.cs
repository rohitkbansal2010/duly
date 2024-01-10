// -----------------------------------------------------------------------
// <copyright file="Attachment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    internal class Attachment
    {
        /// <summary>
        /// Mime type of the content.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Label to display in place of the data".
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Number of bytes of content (if url provided).
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Uri where the data can be found.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Data inline, base64ed.
        /// </summary>
        public string Data { get; set; }
    }
}