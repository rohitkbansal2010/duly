using System.ComponentModel;

namespace Duly.UI.Flourish.GeneratePDF.Model
{
    public class AppointmentLocationViewModel
    {
        [Description("ID")]
        public int Id { get; set; }
        [Description("Address")]
        public Address Address { get; set; }
    }

    public class Address
    {
        [Description("Line")]
        public string Line { get; set; }
        [Description("City")]
        public string City { get; set; }
        [Description("State")]
        public string State { get; set; }
        [Description("Postal Code")]
        public string PostalCode { get; set; }
    }
}