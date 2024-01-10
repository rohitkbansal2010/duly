// <copyright file="IClientAssertionCreator.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Auth
{
    /// <summary>
    /// Does all the prep for auth with epic.
    /// </summary>
    public interface IClientAssertionCreator
    {
        /// <summary>
        /// Builds and creates Client Assertion.
        /// </summary>
        /// <returns>Client Assertion as String.</returns>
        Task<string> GetClientAssertionAsync();

        /// <summary>
        /// Retrieves token from Epic using client assertion.
        /// </summary>
        /// <param name="client_assertion_value"> Client assertion as string. </param>
        /// <returns>Token issued by epic. </returns>
        Task<DulyEpicToken> AuthenticationAsync(string client_assertion_value);
    }
}