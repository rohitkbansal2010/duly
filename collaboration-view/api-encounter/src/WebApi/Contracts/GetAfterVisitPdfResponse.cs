// <copyright file="GetAfterVisitPdfResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class GetAfterVisitPdfResponse
    {
        public string AfterVisitPdf { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
