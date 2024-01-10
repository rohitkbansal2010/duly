// <copyright file="PatientLifeGoalService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using Duly.Common.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IPatientLifeGoalService"/>
    /// </summary>
    internal class PatientLifeGoalService : IPatientLifeGoalService
    {
        private readonly IMapper _mapper;
        private readonly IPatientLifeGoalRepository _repository;
        public PatientLifeGoalService(IMapper mapper, IPatientLifeGoalRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Contracts.CarePlan.PostOrUpdatePatientLifeGoalResponse> PostOrUpdateLifeGoalAsync(PostRequestForLifeGoals request)
        {
            var requestPatientLifeGoal = _mapper.Map<Models.PostRequestForLifeGoals>(request);
            var responsePatientLifeGoal = await _repository.PostOrUpdateLifeGoalAsync(requestPatientLifeGoal);
            var response = _mapper.Map<Contracts.CarePlan.PostOrUpdatePatientLifeGoalResponse>(responsePatientLifeGoal);
            return response;
        }

        public async Task<long> DeletePatientLifeGoalAsync(long patientLifeGoalId)
        {
            var result = await _repository.DeletePatientLifeGoalAsync(patientLifeGoalId);
            return result;
        }

        public async Task<IEnumerable<GetPatientLifeGoalByPatientPlanId>> GetPatientLifeGoalByPatientPlanIdAsync(long id)
        {
            var responsePatientLifeGoad = await _repository.GetPatientLifeGoalByPatientPlanIdAsync(id);
            var result = _mapper.Map<IEnumerable<GetPatientLifeGoalByPatientPlanId>>(responsePatientLifeGoad);
            return result;
        }

        public async Task<long> PostPatientLifeGoalTargetMappingAsync(long patientTargetId, IEnumerable<long> patientLifeGoalIds)
        {
            var responseCustomActions = await _repository.PostPatientLifeGoalTargetMappingAsync(patientTargetId, patientLifeGoalIds);
            return responseCustomActions;
        }

        public async Task<IEnumerable<GetPatientLifeGoalTargetMapping>> GetPatientLifeGoalTargetMappingByPatientIdAsync(long patientTargetId)
        {
            var response = await _repository.GetPatientLifeGoalTargrtMappingByPatientIdAsync(patientTargetId);
            var result = _mapper.Map<IEnumerable<GetPatientLifeGoalTargetMapping>>(response);
            return result;
        }

        public async Task<GetPatientLifeGoalAndActionTrackingResponse> GetPatientLifeGoalAndActionTrackingAsync(long patientPlanId)
        {
            GetPatientLifeGoalAndActionTrackingResponse listOfLifeGoalsAndActions = new GetPatientLifeGoalAndActionTrackingResponse();

            var result = await _repository.GetPatientLifeGoalAndActionTrackingAsync(patientPlanId);
            if (result == null)
            {
                throw new EntityNotFoundException(nameof(result));
            }

            var repoResponse = _mapper.Map<IEnumerable<PatientLifeGoalAndActionTracking>>(result);
            foreach (var groupItemByLifeGoals in repoResponse.GroupBy(x => x.PatientLifeGoalId))
            {
                if (groupItemByLifeGoals.Select(x => x.PatientLifeGoalId).FirstOrDefault() == 0)
                {
                    //OtherActions otherActions = new OtherActions();
                    foreach (var item in groupItemByLifeGoals)
                    {
                        if (!listOfLifeGoalsAndActions.OtherActions.PatientTargets.Any(attribute => attribute.PatientTargetId == item.PatientTargetId))
                        {
                            listOfLifeGoalsAndActions.OtherActions.PatientTargets.Add(new PatientTarget
                            {
                                PatientTargetId = item.PatientTargetId,
                                TargetId = item.PatientTargetId,
                                TargetName = item.TargetName
                            });
                        }

                        listOfLifeGoalsAndActions.OtherActions.PatientActions.Add(new PatientAction
                        {
                            PatientActionId = item.PatientActionId,
                            ActionId = item.ActionId,
                            CustomActionId = item.CustomActionId,
                            ActionName = item.ActionName,
                            Description = item.Description,
                            Progress = item.Progress,
                            Notes = item.Notes
                        });
                    }
                }
                else
                {
                    List<PatientTarget> targets = new List<PatientTarget>();
                    List<PatientAction> actions = new List<PatientAction>();
                    foreach (var item in groupItemByLifeGoals)
                    {

                        if (item.PatientActionId != 0 || item.CustomActionId != 0)
                        {
                            actions.Add(new PatientAction
                            {
                                PatientActionId = item.PatientActionId,
                                ActionId = item.ActionId,
                                CustomActionId = item.CustomActionId,
                                ActionName = item.ActionName,
                                Description = item.Description,
                                Progress = item.Progress,
                                Notes = item.Notes
                            });
                        }

                        if (!targets.Any(attribute => attribute.PatientTargetId == item.PatientTargetId))
                        {
                            targets.Add(new PatientTarget
                            {
                                PatientTargetId = item.PatientTargetId,
                                TargetId = item.PatientTargetId,
                                TargetName = item.TargetName
                            });
                        }


                    }
                    listOfLifeGoalsAndActions.MyActions.Add(new MyActions
                    {
                        PatientLifeGoalId = Convert.ToInt64(groupItemByLifeGoals.Select(x => x.PatientLifeGoalId).FirstOrDefault()),
                        LifeGoalName = groupItemByLifeGoals.Select(x => x.LifeGoalName).FirstOrDefault(),
                        LifeGoalDescription = groupItemByLifeGoals.Select(x => x.LifeGoalDescription).FirstOrDefault(),
                        CategoryName = groupItemByLifeGoals.Select(x => x.CategoryName).FirstOrDefault(),
                        Priority = groupItemByLifeGoals.Select(x => x.Priority).FirstOrDefault(),
                        PatientTargets = targets,
                        PatientActions = actions
                    });
                }
            }

            return listOfLifeGoalsAndActions;
        }
    }
}