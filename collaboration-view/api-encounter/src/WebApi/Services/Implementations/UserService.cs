// <copyright file="UserService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Security.Interfaces;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IUserService"/>
    /// </summary>
    internal class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IActiveDirectoryAccountIdentityService _activeDirectoryAccountIdentityService;

        public UserService(
            IMapper mapper,
            IActiveDirectoryAccountIdentityService activeDirectoryAccountIdentityService)
        {
            _mapper = mapper;
            _activeDirectoryAccountIdentityService = activeDirectoryAccountIdentityService;
        }

        public async Task<User> GetActiveUserAsync()
        {
            var activeUser = await _activeDirectoryAccountIdentityService.GetUserAsync();
            return _mapper.Map<User>(activeUser);
        }
    }
}
