using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.Dashboard
{
    public class PatientAndAppointmentCount
    {
       /// <summary>
       /// Patient Id.
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
