// <copyright file="PostPatientConditionsResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    public class PostPatientConditionsResponse
    {
        /// <summary>
        /// Status Code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Message.
        /// </summary>
        public string Message { get; set; }
    }
}