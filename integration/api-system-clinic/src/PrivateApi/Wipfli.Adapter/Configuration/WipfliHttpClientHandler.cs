// <copyright file="WipfliHttpClientHandler.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Net.Security;

namespace Wipfli.Adapter.Configuration
{
    internal class WipfliHttpClientHandler : HttpClientHandler
    {
        public WipfliHttpClientHandler(IPrivateApiCertificateProvider provider)
        {
            ClientCertificates.Add(provider.GetCertificate());
            ClientCertificateOptions = ClientCertificateOption.Manual;

            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                policyErrors is SslPolicyErrors.RemoteCertificateNameMismatch or SslPolicyErrors.None;
        }
    }
}
