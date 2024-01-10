using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Models.Dashboard
{
   public class PatientAndAppointmentCount
    {
        /// <summary>
        /// Location Id.
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// Location Name.
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Total patient count in particualar location.
        /// </summary>
        public string TotalPatientCount { get; set; }

        /// <summary>
        /// Current Appointment.
        /// </summary>
        public string TodaysAppointment { get; set; }
    }
}
