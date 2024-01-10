// <copyright file="PatientLifeGoalsController.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Constants;
using Duly.Common.ApiExtensions.Controllers;
using Duly.Common.Infrastructure.Entities;
using Duly.Ngdp.Api.Repositories.Interfaces.CarePlan;
using Duly.Ngdp.Contracts.CarePlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Controllers.CarePlan
{
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class PatientLifeGoalsController : DulyControllerBase
    {
        private const string GetPatientLifeGoalsDescription = "Get Patient Life Goal Details.";

        private const string CreateLifeGoalsDescription = "Save Patient Life Goal Details.";

        private const string DeletePatientLifeGoalDescription = "Delete Patient Life Goal";


        private readonly IPatientLifeGoalsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientLifeGoalsController" /> class.
        /// </summary>
        /// <param name="logger">An instance of logger provider.</param>
        /// <param name="environment">An instance of the provider of information about the web hosting environment.</param>
        /// <param name="repository">A repository for working with <see cref=""/>.</param>
        public PatientLifeGoalsController(
                   IPatientLifeGoalsRepository repository,
                   ILogger<PatientLifeGoalsController> logger,
                   IWebHostEnvironment environment)
                   : base(logger, environment)
        {
            _repository = repository;
        }

        [HttpGet("[controller]/{patientPlanId}")]
        [SwaggerOperation(
        Summary = nameof(GetPatientLifeGoals),
        Description = GetPatientLifeGoalsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, GetPatientLifeGoalsDescription, typeof(IEnumerable<GetPatientLifeGoals>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<IEnumerable<GetPatientLifeGoals>>> GetPatientLifeGoals(
        [Required, FromRoute, SwaggerParameter("Patient Plan Id")] long patientPlanId)
        {
            var result = await _repository.GetPatientLifeGoalsAsync(patientPlanId);
            return Ok(result);
        }

        [HttpPost("[controller]")]
        [SwaggerOperation(
             Summary = nameof(PostOrUpdatePatientLifeGoal),
             Description = CreateLifeGoalsDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, CreateLifeGoalsDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<List<PatientLifeGoalResponse>>> PostOrUpdatePatientLifeGoal([FromBody] IEnumerable<PatientLifeGoals> request)
        {
            var data = await _repository.PostOrUpdateLifeGoalAsync(request);

            return Ok(data);
        }

        [HttpPost("DeletePatient/{id}")]
        [SwaggerOperation(
           Summary = nameof(DeletePatientLifeGoal),
           Description = DeletePatientLifeGoalDescription)]
        [SwaggerResponse(StatusCodes.Status200OK, DeletePatientLifeGoalDescription)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Errors.Status400BadRequestText, typeof(FaultResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Errors.Status404NotFoundText, Type = typeof(FaultResponse))]
        public async Task<ActionResult<long>> DeletePatientLifeGoal([FromRoute, SwaggerParameter("Patient LifeGoal Id")] long id)
        {
            var data = await _repository.DeletePatientLifeGoalAsync(id);

            return Ok(data);
        }
    }
}
