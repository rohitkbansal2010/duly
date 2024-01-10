// <copyright file="GetSlotsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ICheckOutDetailsService"/>
    /// </summary>
    internal class GetSlotsService : IGetSlotsservice
    {
        private readonly IMapper _mapper;
        private readonly IGetSlotsRepository _repository;
        private readonly IGetSlotDataRepository _getSlotDataRepository;

        public GetSlotsService(
            IMapper mapper,
            IGetSlotsRepository repository,
            IGetSlotDataRepository getSlotDataRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _getSlotDataRepository = getSlotDataRepository;
        }

        public async Task<IEnumerable<ScheduleDate>> GetScheduleDateAsync(
            string visitTypeId,
            string appointmentId,
            DateTimeOffset startDate,
            DateTimeOffset endDate)
        {
            var reqData = await _getSlotDataRepository.GetProvidersByAppointmentIdAsync(appointmentId);
            var filterVisitTypeId = "External|" + visitTypeId;

            var systemScheduleDates = await _repository.GetOpenScheduleDatesAsync(
                reqData.ProviderId,
                reqData.DepartmentId,
                filterVisitTypeId,
                startDate,
                endDate);

            var scheduleDates = _mapper.Map<ScheduleDate[]>(systemScheduleDates);

            return SortScheduleDates(scheduleDates);
        }

        public async Task<IEnumerable<ScheduleDate>> GetReferralScheduleDateAsync(
            string visitTypeId,
            string departmentId,
            string providerId,
            DateTimeOffset startDate,
            DateTimeOffset endDate)
        {
            var filterVisitTypeId = "External|" + visitTypeId;
            var filterDepartmentId = "External|" + departmentId;
            var filterProviderId = "External|" + providerId;

            var systemScheduleDates = await _repository.GetOpenScheduleDatesAsync(
                filterProviderId,
                filterDepartmentId,
                filterVisitTypeId,
                startDate,
                endDate);

            var scheduleDates = _mapper.Map<ScheduleDate[]>(systemScheduleDates);

            return SortScheduleDates(scheduleDates);
        }

        public async Task<List<ImagingScheduleDate>> GetImagingScheduleAsync(ImagingTimeSlot imagingSlot)
        {
            var filterVisitTypeId = "External|4576";
            List<ImagingScheduleDate> listSchedule = new List<ImagingScheduleDate>();

            foreach (var item in imagingSlot.DepartmentAndProvider)
            {
                var filterDepartmentId = "External|" + item.DepartmentId;

                foreach(var pvitem in item.ProviderId)
                {
                    var filterProviderId = "External|" + pvitem;

                    var systemScheduleDates = await _repository.GetOpenScheduleDatesAsync(
                        filterProviderId,
                        filterDepartmentId,
                        filterVisitTypeId,
                        imagingSlot.StartDate,
                        imagingSlot.EndDate);

                    var scheduleDates = _mapper.Map<ScheduleDate[]>(systemScheduleDates);
                    ImagingScheduleDate objImaging = new ImagingScheduleDate();
                    objImaging.ProviderId = pvitem;
                    objImaging.ScheduleDates = scheduleDates;
                    listSchedule.Add(objImaging);
                }
            }

            return listSchedule;
        }

        private static IEnumerable<ScheduleDate> SortScheduleDates(ScheduleDate[] scheduleDates)
        {
            return scheduleDates
                .Select(scheduleDate => new ScheduleDate()
                {
                    Date = scheduleDate.Date,
                    TimeSlots = scheduleDate.TimeSlots.OrderBy(slot => slot.Time).ToArray()
                })
                .OrderBy(s => s.Date);
        }
    }
}
