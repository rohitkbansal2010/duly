// <copyright file="NgdpAuthorizationMessageHandler.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    public class NgdpAuthorizationMessageHandler : BaseAuthorizationMessageHandler<NgdpApiClientOptions>
    {
        public NgdpAuthorizationMessageHandler(ITokenAcquisition tokenAcquisition, IOptionsMonitor<NgdpApiClientOptions> optionsMonitor)
            : base(tokenAcquisition, optionsMonitor)
        {
        }
    }
}