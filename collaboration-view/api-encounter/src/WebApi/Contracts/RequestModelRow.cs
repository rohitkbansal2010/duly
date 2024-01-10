// <copyright file="RequestModelRow.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class RequestModelRow
    {
        public long RequestId { get; set; }

        public string CorrelationToken { get; set; }

        public string RequestStatusId { get; set; }

        public int AddressesCount { get; set; }

        public int? DeliveryTimeoutInMinutes { get; set; }
    }
}