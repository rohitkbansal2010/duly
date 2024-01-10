// <copyright file="PrivateApiCertificateProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Wipfli.Adapter.Configuration
{
    public class PrivateApiCertificateProvider : IPrivateApiCertificateProvider
    {
        private PrivateApiOptions _options;
        private Lazy<X509Certificate2> _lazyCertificate;

        public PrivateApiCertificateProvider(IOptionsMonitor<PrivateApiOptions> options)
        {
            _options = options.CurrentValue;

            options.OnChange(Listener);

            _lazyCertificate = new Lazy<X509Certificate2>(Init);
        }

        private void Listener(PrivateApiOptions opts, string s)
        {
            if (opts.CertificateKeyVaultUrl == _options.CertificateKeyVaultUrl &&
                opts.CertificateName == _options.CertificateName) return;

            _options = opts;

            if (_lazyCertificate.IsValueCreated)
                _lazyCertificate.Value.Dispose();

            _lazyCertificate = new Lazy<X509Certificate2>(Init);
        }

        public X509Certificate2 GetCertificate()
        {
            return _lazyCertificate.Value;
        }

        private X509Certificate2 Init()
        {
            var privateApiOptions = _options;

            string keyVaultUrl = privateApiOptions.CertificateKeyVaultUrl;
            var client = new CertificateClient(vaultUri: new Uri(keyVaultUrl), credential: new DefaultAzureCredential());
            var certificate = client.DownloadCertificate(privateApiOptions.CertificateName);

            return certificate.Value;
        }
    }
}