using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Contracts.Dashboard
{
    public class PatientAndAppointmentCount
    {
        [Description("Location Id")]
        public string LocationId { get; set; }
        [Description("Location Name")]
        public string LocationName { get; set; }
        [Description("Total patient count in particualar location")]
        public string TotalPatientCount { get; set; }
        [Description("Current Appointment")]
        public string TodaysAppointment { get; set; }
        //[Description("Location Zipcode")]
        //public string ZipCode { get; set; }
    }
}
