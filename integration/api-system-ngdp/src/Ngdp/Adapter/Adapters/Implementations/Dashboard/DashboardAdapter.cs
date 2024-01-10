using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces.Dashboard;
using Duly.Ngdp.Adapter.Adapters.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations.Dashboard
{
    class DashboardAdapter : IDashboardAdapter
    {
        private const string FindPatientAndAppointmentCount = "[dulycv].[uspGetTotalPatientandAppoitmentCountByLocations]";
        private readonly IDapperContext _dapperContext;

        public DashboardAdapter(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task<IEnumerable<PatientAndAppointmentCount>> FindPatientAndAppointmentCountAsync(string locationId)
        {
            dynamic parameters = new { locationIds = locationId };
            IEnumerable<PatientAndAppointmentCount> result = await _dapperContext.QueryAsync<PatientAndAppointmentCount>(FindPatientAndAppointmentCount, parameters);
            return result;

        }
    }
}
