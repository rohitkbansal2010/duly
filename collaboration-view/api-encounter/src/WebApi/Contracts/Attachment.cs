// -----------------------------------------------------------------------
// <copyright file="Attachment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Content in a format defined elsewhere. + Rule: If the Attachment has data, it shall have a contentType")]
    public class Attachment
    {
        [Description("Mime type of the content")]
        public string ContentType { get; set; }

        [Description("Label to display in place of the data")]
        public string Title { get; set; }

        [Description("Number of bytes of content (if url provided)")]
        public int Size { get; set; }

        [Description("Uri where the data can be found")]
        public string Url { get; set; }

        [Description("Data inline, base64ed")]
        public string Data { get; set; }
    }
}