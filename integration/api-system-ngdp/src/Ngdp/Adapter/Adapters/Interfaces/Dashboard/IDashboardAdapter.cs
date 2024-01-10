using Duly.Ngdp.Adapter.Adapters.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.Dashboard
{
   public interface IDashboardAdapter
    {
            /// <summary>
            /// Returns Patient count and appointment count.
            /// </summary>
            /// <param name="locationId">locationId</param>
            /// <returns>Returns List of locations</returns>
            Task<IEnumerable<PatientAndAppointmentCount>> FindPatientAndAppointmentCountAsync(string locationId);
    }
}
