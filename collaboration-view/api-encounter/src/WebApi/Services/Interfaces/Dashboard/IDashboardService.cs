using Duly.CollaborationView.Encounter.Api.Contracts.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces.Dashboard
{
   public interface IDashboardService
    {
        /// <summary>
        /// Returns Patient count and appointment count.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns>Returns List of locations</returns>
        Task<IEnumerable<PatientAndAppointmentCount>> GetPatientAndAppointmentCountAsync(string locationId);

    }
}
