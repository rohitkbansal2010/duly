// <copyright file="PractitionerService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Security.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPractitionerService"/>
    /// </summary>
    internal class PractitionerService : IPractitionerService
    {
        private readonly IPractitionerRepository _repository;
        private readonly IActiveDirectoryAccountIdentityService _activeDirectoryAccountIdentityService;
        private readonly IMapper _mapper;

        public PractitionerService(
            IMapper mapper,
            IPractitionerRepository repository,
            IActiveDirectoryAccountIdentityService activeDirectoryAccountIdentityService)
        {
            _mapper = mapper;
            _repository = repository;
            _activeDirectoryAccountIdentityService = activeDirectoryAccountIdentityService;
        }

        public async Task<IEnumerable<PractitionerGeneralInfo>> GetPractitionersBySiteIdAsync(string siteId)
        {
            var activeUserTask = _activeDirectoryAccountIdentityService.GetUserAsync();

            var practitionersTask = _repository.GetPractitionersBySiteIdAsync(siteId);

            await Task.WhenAll(activeUserTask, practitionersTask);

            var activePractitionerGeneralInfo = _mapper.Map<PractitionerGeneralInfo>(activeUserTask.Result);
            var practitionerGeneralInfos = _mapper.Map<PractitionerGeneralInfo[]>(practitionersTask.Result);

            return CombineContent(activePractitionerGeneralInfo, practitionerGeneralInfos);
        }

        private static IEnumerable<PractitionerGeneralInfo> CombineContent(
            PractitionerGeneralInfo activePractitionerGeneralInfo,
            PractitionerGeneralInfo[] practitionerGeneralInfos)
        {
            var existingPractitioner =
                practitionerGeneralInfos.FindExistingPractitionerGeneralInfo(activePractitionerGeneralInfo);

            activePractitionerGeneralInfo.CopyProperties(existingPractitioner);

            yield return activePractitionerGeneralInfo;

            foreach (var practitionerGeneralInfo in practitionerGeneralInfos)
            {
                if(existingPractitioner != practitionerGeneralInfo)
                    yield return practitionerGeneralInfo;
            }
        }
    }
}
