using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces.Dashboard;
using Duly.Ngdp.Api.Repositories.Interfaces.Dashboard;
using Duly.Ngdp.Contracts.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Implementations.Dashboard
{
    public class NgdpDashboardRepository : IDashboardRepository
    {
        private readonly IDashboardAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpDashboardRepository(IDashboardAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PatientAndAppointmentCount>> GetPatientAndAppointmentCountAsync(string locationId)
        {
            var ngdpPatientAndAppointmentCount = await _adapter.FindPatientAndAppointmentCountAsync(locationId);

            var systemPatientAndAppointmentCount = _mapper.Map<IEnumerable<PatientAndAppointmentCount>>(ngdpPatientAndAppointmentCount);

            return systemPatientAndAppointmentCount;
        }
    }
}
