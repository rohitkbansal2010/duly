// <copyright file="JwtBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Duly.Clinic.Fhir.Adapter.Auth
{
    /// <summary>
    /// Builds Jwt Token for signing and hashes it with with sha384.
    /// </summary>
    public static class JwtBuilder
    {
        private const string ClientAssertionType = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";

        /// <summary>
        /// expiry time is 300 seconds from time of creation.
        /// </summary>
        private const int ExpiryTime = 300;

        private static readonly JwtHeader _jwtHeader = new()
        {
            { JwtRegisteredClaimNames.Typ, "JWT" },
            { "alg", Algorithm.ToString() }
        };

        public static SignatureAlgorithm Algorithm => SignatureAlgorithm.RS384;

        public static string BuildToken(string clientId, string fhirTokenUrl)
        {
            var utcNow = DateTime.UtcNow;
            var expires = utcNow.AddSeconds(ExpiryTime);

            var epochNow = EpochTime.GetIntDate(utcNow);
            var epochExpires = EpochTime.GetIntDate(expires);

            var jwtPayload = new JwtPayload
            {
                { JwtRegisteredClaimNames.Iss, clientId },
                { JwtRegisteredClaimNames.Sub, clientId },
                { JwtRegisteredClaimNames.Aud, fhirTokenUrl },
                { JwtRegisteredClaimNames.Nbf, epochNow },
                { JwtRegisteredClaimNames.Iat, epochNow },
                { JwtRegisteredClaimNames.Exp, epochExpires },
                { JwtRegisteredClaimNames.Jti, Guid.NewGuid() }
            };

            var token = new JwtSecurityToken(_jwtHeader, jwtPayload);

            return Combine(token.EncodedHeader, token.EncodedPayload);
        }

        public static byte[] HashToken(string token)
        {
            var bytesToSign = Encoding.UTF8.GetBytes(token);
            using SHA384Managed sha384Managed = new();
            return sha384Managed.ComputeHash(bytesToSign);
        }

        public static string Combine(string firstPart, string secondPart)
        {
            return $"{firstPart}.{secondPart}";
        }

        public static object BuildPayload(string client_assertion_value)
        {
            var payload = new
            {
                grant_type = "client_credentials",
                client_assertion = client_assertion_value,
                client_assertion_type = ClientAssertionType
            };

            return payload;
        }
    }
}
