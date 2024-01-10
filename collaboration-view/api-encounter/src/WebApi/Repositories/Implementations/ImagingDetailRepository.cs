// -----------------------------------------------------------------------
// <copyright file="ImagingDetailRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// --------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IImagingDetailRepository"/>
    /// </summary>
    public class ImagingDetailRepository : IImagingDetailRepository
    {
        private const string InsertImagingStoredProcedureName = "[InsertLabDetails]";

        private readonly IDapperContext _dapperContext;
        public ImagingDetailRepository(
        IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<int> PostImagingDetailAsync(ImagingDetails request)
        {
            dynamic labDetailParameter = new
            {
                Id = request.ID,
                Type = request.Type,
                ImagingType = request.ImagingType,
                Appointment_ID = request.Appointment_ID,
                Patient_ID = request.Patient_ID,
                Provider_ID = request.Provider_ID,
                Location_ID = request.Location_ID,
                BookingSlot = request.BookingSlot,
                AptScheduleDate = request.AptScheduleDate,
                ImagingLocation = request.ImagingLocation,
                Skipped = request.Skipped
            };
            return _dapperContext.ExecuteScalarAsync<int>(InsertImagingStoredProcedureName, labDetailParameter);
        }
    }
}