// <copyright file="IUserService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.User"/> entity.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Returns active <see cref="Contracts.User"/>.
        /// </summary>
        /// <returns>A <see cref="Contracts.User"/> instance.</returns>
        public Task<Contracts.User> GetActiveUserAsync();
    }
}
