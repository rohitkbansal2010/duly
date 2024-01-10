using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.Dashboard
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<Models.Dashboard.PatientAndAppointmentCount>> GetPatientAndAppointmentCountAsync(string locationId);
    }
}
