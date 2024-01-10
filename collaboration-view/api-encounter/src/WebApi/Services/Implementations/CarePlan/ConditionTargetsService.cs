// <copyright file="ConditionTargetsService.cs" company="Duly Health and Care">
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

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="IConditionTargetsService"/>
    /// </summary>
    public class ConditionTargetsService : IConditionTargetsService
    {
        private readonly IMapper _mapper;
        private readonly IConditionTargetsRepository _repository;

        public ConditionTargetsService(IMapper mapper, IConditionTargetsRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<GetConditionTargetsResponse>> GetConditionTargetsByConditionId(string conditionIds)
        {
            List<GetConditionTargetsResponse> listOfConditionTargets = new List<GetConditionTargetsResponse>();

            var result = await _repository.GetConditionTargetsByConditionId(conditionIds);
            if (result == null)
            {
                throw new EntityNotFoundException(nameof(result));
            }

            var targetResponse = _mapper.Map<IEnumerable<ConditionTargets>>(result);
            foreach (var grpItemByCondition in targetResponse.GroupBy(x => x.ConditionId))
            {
                List<Targets> targets = new List<Targets>();
                foreach (var grpItemByTarget in grpItemByCondition.GroupBy(x => x.TargetId))
                {
                    List<GetTargetCategory> normalValue = new List<GetTargetCategory>();
                    foreach (var item in grpItemByTarget)
                    {
                        normalValue.Add(new GetTargetCategory
                        {
                            CategoryName = item.CategoryName,
                            CategoryValue = new TargetMinMaxValues(item.TargetMinValue, item.TargetMaxValue),
                        });
                    }

                    targets.Add(new Targets
                    {
                        TargetId = Convert.ToInt64(grpItemByTarget.Select(x => x.TargetId).FirstOrDefault()),
                        TargetName = grpItemByTarget.Select(x => x.TargetName).FirstOrDefault(),
                        NormalValue = normalValue,
                        Description = grpItemByTarget.Select(x => x.Description).FirstOrDefault(),
                        MeasurementUnit = grpItemByTarget.Select(x => x.MeasurementUnit).FirstOrDefault(),
                        Active = grpItemByTarget.Select(x => x.Active).FirstOrDefault(),
                    });
                }

                listOfConditionTargets.Add(new GetConditionTargetsResponse
                {
                    ConditionId = Convert.ToInt64(grpItemByCondition.Select(x => x.ConditionId).FirstOrDefault()),
                    Targets = targets
                });
            }

            return listOfConditionTargets;
        }
    }
}