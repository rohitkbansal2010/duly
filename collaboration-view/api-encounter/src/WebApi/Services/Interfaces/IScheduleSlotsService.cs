// <copyright file="IScheduleSlotsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.CheckOutDetails"/> entity.
    /// </summary>
    public interface IScheduleSlotsservice
    {
        /// <summary>
        /// Create an appointment based on <see cref="referralId"/> and model <see cref="appointmentSchedulingRequest"/>>.
        /// </summary>
        /// <param name="appointmentSchedulingRequest">Model describing the appointment to be scheduled.</param>
        /// <param name="appointmentId">appointment ID.</param>
        /// <returns>Result of the scheduling: <see cref="ScheduleAppointmentResult"/>.</returns>
        Task<ScheduleAppointmentResult> ScheduleAppointmentForPatientAsync(AppointmentSchedulingModel appointmentSchedulingRequest, string appointmentId);

        /// <summary>
        /// Create an appointment based on <see cref="referralId"/> and model <see cref="appointmentSchedulingRequest"/>>.
        /// </summary>
        /// <param name="appointmentSchedulingRequest">Model describing the appointment to be scheduled.</param>
        /// <param name="patientId">patient ID.</param>
        /// <param name="providerId">provider ID.</param>
        /// <param name="departmentId">department ID.</param>
        /// <param name="visitTypeId">visit type ID.</param>
        /// <returns>Result of the scheduling: <see cref="ScheduleAppointmentResult"/>.</returns>
        Task<ScheduleAppointmentResult> ScheduleReferralAppointmentForPatientAsync(AppointmentSchedulingModel appointmentSchedulingRequest, string patientId, string providerId, string departmentId, string visitTypeId);

        /// <summary>
        /// Create an appointment based on  model <see cref="ScheduleAppointmentModel"/>>.
        /// </summary>
        /// <param name="model">Model describing the appointment to be scheduled.</param>
        /// <returns>Result of the scheduling: <see cref="ScheduleAppointmentResult"/>.</returns>
        Task<ScheduleAppointmentResult> ScheduleImagingAppointmentForPatientAsync(Models.CheckOut.ScheduleAppointmentModel model);
    }
}
