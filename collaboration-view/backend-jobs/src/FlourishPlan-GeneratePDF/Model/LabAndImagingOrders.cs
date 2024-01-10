using System.Collections.Generic;
using System.ComponentModel;

namespace PDFGeneratorApp.ViewModel
{
    public class LabAndImagingOrders
    {
        [Description("Names of the lab orders")]
        public List<Orders> TestOrder { get; set; }

        [Description("Count of Lab orders")]
        public int? OrderCount { get; set; }
    }

    public class Orders
    {
        [Description("Names of the lab orders")]
        public string OrderName { get; set; }

        [Description("Authored On")]
        public string AuthoredOn { get; set; }

        [Description("Code")]
        public string Code { get; set; }
    }
}