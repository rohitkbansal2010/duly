// <copyright file="AddressModelRow.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class AddressModelRow
    {
        public long AddressId { get; set; }

        public long RequestId { get; set; }

        public string CorrelationToken { get; set; }

        public string TimeZone { get; set; }

        public int SortOrder { get; set; }

        public string AddressPointer { get; set; }
    }
}