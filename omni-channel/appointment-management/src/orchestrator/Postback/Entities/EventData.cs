// <copyright file="EventData.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.OmniChannel.Orchestrator.Appointment.Postback.Entities
{
    public class EventData
    {
        public string DeliveryId { get; set; }

        public string RequestId { get; set; }

        public string RequestCorrelationToken { get; set; }

        public string AddressCorrelationToken { get; set; }

        public string ConfigurationToken { get; set; }

        public DateTime ActivityTime { get; set; }

        public string ActivityText { get; set; }

        public string AttemptCompletionCode { get; set; }

        public string RequestCompletionStatus { get; set; }

        public string OriginatorReferenceId { get; set; }

        public string RequestAddressValue { get; set; }

        public dynamic Meta { get; set; }
    }
}
