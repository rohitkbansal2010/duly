using System.Collections.Generic;

namespace PDFGeneratorApp.ViewModel
{
    public class FollowUpViewModel
    {
        public string id { get; set; }
        public HumanName humanName { get; set; }
        public Photo photo { get; set; }
        public string speciality { get; set; }
        public Location location { get; set; }

    }

    public class GeographicCoordinates
    {
        public string latitude { get; set; }
        public string longitude { get; set; }

    }
    public class HumanName
    {
        public string familyName { get; set; }
        public List<string> givenNames { get; set; }
        public List<string> suffixes { get; set; }
        public List<string> prefixes { get; set; }

    }

    public class Photo
    {
        public string contentType { get; set; }
        public string url { get; set; }
    }

    public class Location
    {
        public string id { get; set; }
        public Address address { get; set; }
        public GeographicCoordinates geographicCoordinates { get; set; }
        public string phoneNumber { get; set; }
        public double distance { get; set; }
    }

    public class Address
    {
        public string addressLine { get; set; }
        public string adderessLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipeCode { get; set; }
    }
}