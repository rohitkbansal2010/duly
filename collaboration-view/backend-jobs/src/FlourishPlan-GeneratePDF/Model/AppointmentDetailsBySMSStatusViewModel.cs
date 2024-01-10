using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.UI.Flourish.GeneratePDF.Model
{
    public class AppointmentDetailsBySMSStatusViewModel
    {
        [Description("ID")]
        public long Id { get; set; }
        [Description("Appointment Id")]
        public string AppointmentId { get; set; }
        [Description("Patient Id")]
        public string PatientId { get; set; }
        [Description("Provider Id")]
        public string ProviderId { get; set; }
        [Description("Appointment Time")]
        public string AppointmentTime { get; set; }
        [Description("Site Id")]
        public string  SiteId { get; set; }
        [Description("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
