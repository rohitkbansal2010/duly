// <copyright file="LifeGoalDetailsConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using System;
using System.Collections.Generic;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings.CarePlan
{
    public class LifeGoalDetailsConverter : ITypeConverter<Contracts.CarePlan.PostRequestForLifeGoals, Models.PostRequestForLifeGoals>
    {
        public PostRequestForLifeGoals Convert(Contracts.CarePlan.PostRequestForLifeGoals source, PostRequestForLifeGoals destination, ResolutionContext context)
        {
            return new Models.PostRequestForLifeGoals
            {
                PatientPlanId = source.PatientPlanId,
                PatientLifeGoal = (source.PatientLifeGoal != null) ? BuildLifeGoals(source) : null,
                DeletedLifeGoalIds = source.DeletedLifeGoalIds
            };
        }

        private static IEnumerable<PatientLifeGoal> BuildLifeGoals(Contracts.CarePlan.PostRequestForLifeGoals source)
        {
            var listOfLifeGoals = new List<Models.PatientLifeGoal>();
            foreach (var item in source.PatientLifeGoal)
            {
                var lifegoal = new Models.PatientLifeGoal();
                lifegoal.PatientLifeGoalId = item.PatientLifeGoalId;
                lifegoal.LifeGoalName = item.LifeGoalName;
                lifegoal.LifeGoalDescription = item.LifeGoalDescription;
                lifegoal.CategoryName = item.CategoryName;
                lifegoal.Priority = item.Priority;
                listOfLifeGoals.Add(lifegoal);
            }

            return listOfLifeGoals;
        }
    }
}