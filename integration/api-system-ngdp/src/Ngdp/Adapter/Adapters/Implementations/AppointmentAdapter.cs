// <copyright file="AppointmentAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAppointmentAdapter"/>
    /// </summary>
    internal class AppointmentAdapter : IAppointmentAdapter
    {
        private const string FindAppointmentsStoredProcedureName = Constants.SchemaName + Constants.NameSeparator + "[uspAppointmentsSelect]";
        private const string FindAppointmentByCsnIdStoredProcedureName = Constants.SchemaName + Constants.NameSeparator + "[uspAppointmentSelectSingleByCsnId]";
        private const string FindAppointmentsForPatientByCsnIdStoredProcedureName = Constants.SchemaName + Constants.NameSeparator + "[uspAppointmentsSelectForPatientByCsnId]";
        private const string FindReferralAppointmentsByReferralIdStoredProcedureName = Constants.SchemaName + Constants.NameSeparator + "[uspReferralAppointmentsSelectByReferralId]";

        private readonly IDapperContext _dapperContext;

        public AppointmentAdapter(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<IEnumerable<Appointment>> FindAppointmentsAsync(AppointmentSearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new ArgumentNullException(nameof(searchCriteria));
            }

            var parameters = searchCriteria.ConvertToParameters();
            return _dapperContext.QueryAsync<Appointment>(FindAppointmentsStoredProcedureName, parameters);
        }

        public async Task<Appointment> FindAppointmentByCsnIdAsync(string csnId)
        {
            dynamic parameters = new { CsnId = csnId };
            IEnumerable<Appointment> result = await _dapperContext.QueryAsync<Appointment>(FindAppointmentByCsnIdStoredProcedureName, parameters);
            return result?.SingleOrDefault();
        }

        public Task<IEnumerable<Appointment>> FindAppointmentsForPatientByCsnIdAsync(AppointmentSearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new ArgumentNullException(nameof(searchCriteria));
            }

            var parameters = searchCriteria.ConvertToParametersForPatient();
            return _dapperContext.QueryAsync<Appointment>(FindAppointmentsForPatientByCsnIdStoredProcedureName, parameters);
        }

        public Task<IEnumerable<ReferralAppointment>> FindReferralAppointmentsByReferralIdAsync(string referralId)
        {
            dynamic parameters = new
            {
                ReferralId = referralId
            };

            return _dapperContext.QueryAsync<ReferralAppointment>(FindReferralAppointmentsByReferralIdStoredProcedureName, parameters);
        }
    }
}
