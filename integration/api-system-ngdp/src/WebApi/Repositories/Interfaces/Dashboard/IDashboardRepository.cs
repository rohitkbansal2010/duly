using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duly.Ngdp.Contracts.Dashboard;

namespace Duly.Ngdp.Api.Repositories.Interfaces.Dashboard
{
    public interface IDashboardRepository
    {

        /// <summary>
        /// Returns Patient count and appointment count.
        /// </summary>
        /// <param name="locationId">locationId</param>
        /// <returns>Returns List of locations</returns>
        Task<IEnumerable<PatientAndAppointmentCount>> GetPatientAndAppointmentCountAsync(string locationId);
    }
}
