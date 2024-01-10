// <copyright file="IPrivateApiCertificateProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Security.Cryptography.X509Certificates;

namespace Wipfli.Adapter.Configuration
{
    public interface IPrivateApiCertificateProvider
    {
        X509Certificate2 GetCertificate();
    }
}