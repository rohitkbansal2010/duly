using PDFGeneratorApp.ViewModel;
using System.Collections.Generic;

namespace Duly.UI.Flourish.GeneratePDF.Helpers
{
    public class ImagingLocationStringHelper
    {
        public static string CreateString(List<ImagingLocationsViewModel> imagingLocations)
        {
            var locationsString = "";
            foreach(var location in imagingLocations)
            {
                locationsString += @"[{lat},{lng}],".Replace("{lat}", location.Address.Lat.ToString()).Replace("{lng}", location.Address.Lng.ToString());
            }
            return locationsString;
        }
    }
}