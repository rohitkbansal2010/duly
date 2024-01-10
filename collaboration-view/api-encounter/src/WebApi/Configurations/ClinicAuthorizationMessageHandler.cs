// <copyright file="ClinicAuthorizationMessageHandler.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    public class ClinicAuthorizationMessageHandler : BaseAuthorizationMessageHandler<ClinicApiClientOptions>
    {
        public ClinicAuthorizationMessageHandler(ITokenAcquisition tokenAcquisition, IOptionsMonitor<ClinicApiClientOptions> optionsMonitor)
            : base(tokenAcquisition, optionsMonitor)
        {
        }
    }
}