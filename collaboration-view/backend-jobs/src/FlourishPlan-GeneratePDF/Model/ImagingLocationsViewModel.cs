using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PDFGeneratorApp.ViewModel
{
    public class ImagingLocationsViewModel
    {
        [Description("Imaging Location Id")]
        public int Id { get; set; }
        [Description("Imaging Title")]
        public string Title { get; set; }
        [Description("Imaging date created")]
        public DateTime DateCreated { get; set; }
        [Description("Imaging date updated")]
        public DateTime DateUpdated { get; set; }
        [Description("Imaging address")]
        public ImagingAddress Address { get; set; }
        [Description("Imaging provider ids")]
        public List<DeptProvider> Dept_providers { get; set; }
        [Description("Distance")]
        public double Distance { get; set; }
    }

    public class ImagingAddress
    {
        [Description("Latitude of location")]
        public double Lat { get; set; }
        [Description("Longitude of location")]
        public double Lng { get; set; }
        [Description("Imaging address")]
        public string Address { get; set; }
        [Description("Imaging address detail")]
        public ImagingAddressPart Parts { get; set; }
    }

    public class DeptProvider
    {
        public string Dept_id { get; set; }
        public List<string> Provider_ids { get; set; }
    }

    public class ImagingAddressPart
    {
        [Description("Street number")]
        public string Street_number { get; set; }
        [Description("Imaging address number")]
        public string Number { get; set; }

        [Description("Address")]
        public string Address { get; set; }
        [Description("City")]
        public string City { get; set; }
        [Description("Postal Code")]
        public string Postcode { get; set; }
        [Description("County")]
        public string County { get; set; }
        [Description("State")]
        public string State { get; set; }
        [Description("Country")]
        public string Country { get; set; }
        [Description("Planet")]
        public string Planet { get; set; }
        [Description("System")]
        public string System { get; set; }
        [Description("Arm")]
        public string Arm { get; set; }
        [Description("Galaxy")]
        public string Galaxy { get; set; }
        [Description("Group")]
        public string Group { get; set; }
        [Description("Cluster")]
        public string Cluster { get; set; }
        [Description("Super Cluster")]
        public string Supercluster { get; set; }
    }
}