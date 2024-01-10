// <copyright file="CreationResultResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut
{
    public class CreationResultResponse
    {
        public int RecordID { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }

        public string StatusCode { get; set; }
    }
}
