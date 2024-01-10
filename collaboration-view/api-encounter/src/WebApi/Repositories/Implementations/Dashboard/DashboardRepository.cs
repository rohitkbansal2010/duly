using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.Dashboard;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.Dashboard;
using Duly.Ngdp.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IMapper _mapper;
        private readonly IClient _client;

        public DashboardRepository(
                 IEncounterContext encounterContext,
                 IMapper mapper,
                 IClient client)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _client = client;
        }

        public async Task<IEnumerable<Models.Dashboard.PatientAndAppointmentCount>> GetPatientAndAppointmentCountAsync(string locationId)
        {
            var responseNgdpPatientAndAppointmentCount = await _client.GetPatientAndAppointmentCountAsync(_encounterContext.GetXCorrelationId(), locationId);
            var repositoryNgdpPatientAndAppointmentCount = _mapper.Map<IEnumerable<Models.Dashboard.PatientAndAppointmentCount>>(responseNgdpPatientAndAppointmentCount);
            return repositoryNgdpPatientAndAppointmentCount;
        }
    }
}
