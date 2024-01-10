// <copyright file="PostAfterVisitPdfResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class PostAfterVisitPdfResponse
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public int AfterVisitPdfId { get; set; }
    }
}
