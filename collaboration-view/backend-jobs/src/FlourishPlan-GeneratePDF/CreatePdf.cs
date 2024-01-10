using Newtonsoft.Json.Linq;
using PDFGeneratorApp.ViewModel;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using Duly.UI.Flourish.GeneratePDF.Views;
using Duly.UI.Flourish.GeneratePDF.Helpers;
using System.Text;
using Duly.UI.Flourish.GeneratePDF.Model;
using Microsoft.Extensions.Configuration;

namespace Duly.UI.Flourish.GeneratePDF
{
    public class CreatePdf
    {
        private string _divEnd = @" </div> ";
        private readonly IConfiguration _configuration;

        public CreatePdf(IConfiguration config)
        {
            _configuration = config;
        }

        public string CreatePDF(CheckoutDetailsViewModel checkoutData, PatientViewModel patient, List<FollowUpViewModel> followUpData, List<ImagingLocationsViewModel> LabLocation, LabAndImagingOrders labsOrders, LabAndImagingOrders imagingOrders, PrescriptionViewModel prescriptions, PharmacyViewModel preferredPharmacy, OpenReferralViewModel openReferrals, List<ScheduleReferral> scheduleReferral, List<ImagingLocationsViewModel> imagingLocations, AppointmentDetailsBySMSStatusViewModel pendingData, SitesDetailsViewModel followUpLocation)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.DisplayHeader = true;
            converter.Options.DisplayFooter = true;
            converter.Header.DisplayOnFirstPage = false;
            converter.Header.DisplayOnOddPages = true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Header.Height = 45;
            converter.Footer.Height = 25;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MinPageLoadTime = 5;
            converter.Options.CssMediaType = HtmlToPdfCssMediaType.Print;
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.InternalLinksEnabled = true;
            converter.Options.JavaScriptEnabled = true;
            converter.Options.JpegCompressionEnabled = true;
            converter.Options.MarginBottom = 2;
            converter.Options.ColorSpace = PdfColorSpace.RGB;
            converter.Options.PdfStandard = PdfStandard.Full;

            var basePath = Directory.GetCurrentDirectory();
            string document = "";
            var head = HeaderView.GetHead();
            string body = @" <body> <div> ";

            //get patient name
            var patientName = PatientHelper.GetName(patient);

            //get the patientImage
            var patientImage = PatientHelper.GetImage(patient);
            patientImage = patientImage == null ? (@"data:image/png;base64," + _configuration.GetSection("Values:default_profile_pic").Value) : patientImage;

            var createdDate = DateTimeOffset.Now;

            //create the html template for Patient header
            var patientContainer = PatientContainerView.CreatePatientContainer(createdDate, patient, patientName, patientImage, pendingData);
            body += patientContainer;

            //create the html template of header
            var pageHeader = GetHeader.CustomHeader(createdDate, patient, patientName, patientImage, pendingData);
            using (FileStream fs = new FileStream("header.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(pageHeader);
                }
            }

            //Add the header to the page PDF
            PdfHtmlSection header = new PdfHtmlSection(basePath + @"\header.html");
            header.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(header);

            //Create the html template of the Footer
            var pageFooter = GetFooter.GetCustomFooter();
            using (FileStream fs = new FileStream("footer.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(pageFooter);
                }
            }

            //Add the footer to the Page
            PdfHtmlSection footer = new PdfHtmlSection(basePath + @"\footer.html");
            PdfTextSection pageNumber = new PdfTextSection(0, 11, "{page_number}/{total_pages}   ", new Font("Arial", 8));
            pageNumber.HorizontalAlign = PdfTextHorizontalAlign.Right;
            footer.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footer);
            converter.Footer.Add(pageNumber);

            //Get the doctor image
            string doctorImage = DoctorHelper.GetDoctorImage(followUpData[0]);
            doctorImage = doctorImage == null ? @"data:image/png;base64," + _configuration.GetSection("Values:default_profile_pic").Value : doctorImage;

            //Create the follow Up Html Template
            body += FollowUpContainerView.CreateFollowUpContainer(checkoutData, followUpData, patient, pendingData, followUpLocation, doctorImage);

            body += _divEnd;

            //Get the Patient Address
            var originAddress = PatientHelper.GetAddress(patient);

            //Labs Html Template
            if (labsOrders.OrderCount > 0)
            {
                body += LabContainerView.CreateLabContainer(labsOrders);

                var labsDisplay = @" <div class='display--flex padded-white'> ";
                var labsDisplay2 = @" <div class='display--flex padded-white'> ";

                string labsSectionStart = @"  <div class='lab-section'> ";
                string labsSectionStart2 = @"  <div class='lab-section2'> ";

                var allScheduleLabLocations = new List<GetLabOrImaging>();
                var allUnScheduleLabLocations = new List<GetLabOrImaging>();
                foreach (var labs in checkoutData.LabDetailsList)
                {
                    if (labs.Type == "L" && !Convert.ToBoolean(labs.Skipped))
                    {
                        if (!labs.Skipped)
                        {
                            allScheduleLabLocations.Add(labs);
                        }
                    }
                }
                var count = allScheduleLabLocations.Count;
                var count2 = labsOrders.TestOrder.Count;
                string labsDetails = "";
                string labsDetails2 = "";
                if (count >= 1)
                {
                    if (count2 <= 3)
                    {
                        labsDetails = LabContainerView.CreateLabOrdersContainer(labsOrders, allScheduleLabLocations, 0, count2 - 1, true);
                    }
                    else
                    {
                        labsDetails = LabContainerView.CreateLabOrdersContainer(labsOrders, allScheduleLabLocations, 0, 2, true);
                        labsDetails2 = LabContainerView.CreateLabOrdersContainer(labsOrders, allScheduleLabLocations, 3, count2 - 1, true);
                    }
                }
                else
                {
                    if (count2 <= 3)
                    {
                        labsDetails = LabContainerView.CreateLabOrdersContainer(labsOrders, allScheduleLabLocations, 0, count2 - 1, false);
                    }
                    else
                    {
                        labsDetails = LabContainerView.CreateLabOrdersContainer(labsOrders, allScheduleLabLocations, 0, 2, false);
                        labsDetails2 = LabContainerView.CreateLabOrdersContainer(labsOrders, allScheduleLabLocations, 3, count2 - 1, false);
                    }
                }

                labsSectionStart += labsDetails + _divEnd;
                labsSectionStart2 += labsDetails2 + _divEnd;

                //Labs Map conatainer
                labsDisplay += labsSectionStart + @"<div class='lab-img page-break' id='map1'> </div>" + _divEnd;
                labsDisplay2 += labsSectionStart2 + _divEnd;

                body += labsDisplay;
                body += labsDisplay2;

                //Labs Other location Html Template
                var allOtherLocations = LabContainerView.CreateOtherLocationsLabs(LabLocation, allScheduleLabLocations, allUnScheduleLabLocations);

                allOtherLocations += _divEnd;

                body += allOtherLocations;
            }

            //Create Referrals Html Template
            body += ReferralsContainerView.CreateRefferalsContainer(openReferrals, scheduleReferral, pendingData);

            //Create Imaging Html Template
            body += ImagingContainerView.CreateImagingContainer(imagingOrders, imagingLocations, checkoutData);

            //Create Prescriptions Html Template
            body += PrescriptionContainerView.CreatePrescriptionsContainer(prescriptions, preferredPharmacy);

            //Get the Google Api jS url string
            var googleMapJsAPIurl = _configuration.GetSection("Values:GoogleMapJsAPI").Value;
            body += @"<script src='" + googleMapJsAPIurl + "' ></script></br> ";

            //Labs Map script
            body += AddScriptsMarkersMaps("map1", originAddress, LabContainerView.destLocation) + "</br>";

            //Imaging Map Script
            body += AddScriptsMarkersMaps("map2", originAddress, ImagingContainerView._imagingLocation) + "</br>";

            //Preferred Pharmacy Map script
            if (preferredPharmacy != null && !string.IsNullOrEmpty(preferredPharmacy.PharmacyName))
            {
                var destAdPharmacy = preferredPharmacy.AddressLine1 + "+" + preferredPharmacy.AddressLine2 + "+" + preferredPharmacy.City + "+" + preferredPharmacy.State + "+" + preferredPharmacy.ZipCode;

                body += AddScriptsRouteMaps("map3", originAddress, destAdPharmacy) + "</br>";
            }

            body += @"</div> </body> </html>";

            //Final html template
            document += head + body;
            Console.WriteLine("Template is Rendering..");
            MemoryStream stream = new MemoryStream();
            PdfDocument doc = converter.ConvertHtmlString(document);
            Console.WriteLine("Template is Rendered!!");
            Console.WriteLine("Document is Saving...");
            doc.Save(stream);
            doc.Close();
            Console.WriteLine("Document is Saved");
            byte[] docBytes = stream.ToArray();
            var docBase64 = Convert.ToBase64String(docBytes);
            Console.WriteLine("Document Created.");
            return docBase64;
        }

        private JToken GetAddress(string address)
        {
            HttpClient http = new HttpClient();
            var uri = _configuration.GetSection("Values:Google_Address_API").Value + address;
            var response = http.GetAsync(uri);
            response.Wait();

            var test = response.Result;
            JToken LocationLatLng = new JObject();
            if (test.IsSuccessStatusCode)
            {
                var result = test.Content.ReadAsStringAsync();
                result.Wait();
                JObject json = JObject.Parse(result.Result);
                LocationLatLng = json["results"][0]["geometry"]["location"];
            }
            return LocationLatLng;
        }

        //for route maker
        private string AddScriptsRouteMaps(string mapId, string originAd, string destAd)
        {
            var origin = GetAddress(originAd);
            var dest = GetAddress(destAd);
            var icon = _configuration.GetSection("Values:DulyIcon").Value;

            var script = @"<script type='text/javascript'>
        var bounds = new google.maps.LatLngBounds();
        var origin = {lat:{Origin-Lat}, lng:{Origin-Lng}};
        var dest = {lat:{Dest-Lat}, lng:{Dest-Lng}};
        bounds.extend(new google.maps.LatLng(origin.lat, origin.lng));
        bounds.extend(new google.maps.LatLng(dest.lat, dest.lng));
        
        var map = new google.maps.Map(document.getElementById('{Map-Id}'), {
          zoom: 10,
          center: new google.maps.LatLng({Origin-Lat}, {Origin-Lng}),
          mapTypeId: google.maps.MapTypeId.ROADMAP
        });
    
        var directionsService = new google.maps.DirectionsService();
    
        var directionsDisplayRenderer = new google.maps.DirectionsRenderer({
            draggable: true,
            suppressMarkers: true,
        });
    
        directionsDisplayRenderer.setMap(map);
        var image = 'data:image/png;base64,{Icon}';
        var request = {
        origin: origin,
        destination: dest,
        travelMode: google.maps.TravelMode.DRIVING,
        unitSystem: google.maps.UnitSystem.IMPERIAL
    }
    
        directionsService.route(request, function (result, status){
            directionsDisplayRenderer.setDirections(result);
        });
        marker = new google.maps.Marker({
            position: new google.maps.LatLng(origin.lat, origin.lng),
            map: map,
        });
        marker = new google.maps.Marker({
            position: new google.maps.LatLng(dest.lat, dest.lng),
            map: map,
            icon: image,
        });
          map.fitBounds(bounds);
          map.panToBounds(bounds);
</script>
    ".Replace("{Origin-Lat}", origin["lat"].ToString()).Replace("{Origin-Lng}", origin["lng"].ToString()).Replace("{Dest-Lat}", dest["lat"].ToString()).Replace("{Dest-Lng}", dest["lng"].ToString()).Replace("{Map-Id}", mapId).Replace("{Icon}", icon);

            return script;
        }

        //for single marker
        private string AddScriptsMarkersMaps(string mapId, string originAddress, string destAddress)
        {
            var origin = GetAddress(originAddress);
            var dest = GetAddress(destAddress);
            var lat = origin["lat"];
            var lng = origin["lng"];
            var icon = _configuration.GetSection("Values:DulyIcon").Value;

            var script = @"<script type='text/javascript'>
    var bounds = new google.maps.LatLngBounds();
    var locations = [
      [{origin-latitude}, {origin-longitude}],
      [{dest-latitude}, {dest-longitude}]
    ];
    var loc = new google.maps.LatLng(locations[0][0], locations[0][1]);
    bounds.extend(loc);
    var map = new google.maps.Map(document.getElementById('{Map-Id}'),
         {
            zoom:9,
            center: new google.maps.LatLng(locations[0][0], locations[0][1]),
         }
         );
    
    var infowindow = new google.maps.InfoWindow();

    var marker, i;
    var image = 'data:image/png;base64,{Icon}';
    marker = new google.maps.Marker({
        position: new google.maps.LatLng(locations[0][0], locations[0][1]),
        map: map,
      });
    var flag = 0;
    for (i = 1; i < locations.length; i++) {  
      if(locations[i][0] == null || locations[i][1] == null){ 
      flag = 1;
      continue;}
      marker = new google.maps.Marker({
        position: new google.maps.LatLng(locations[i][0], locations[i][1]),
        map: map,
        icon: image
      });
      bounds.extend(new google.maps.LatLng(locations[i][0], locations[i][1]));
    }
     if(flag == 0){
    map.fitBounds(bounds);
    map.panToBounds(bounds);
}
  </script> ".Replace("{origin-latitude}", Convert.ToString(lat)).Replace("{origin-longitude}", Convert.ToString(lng)).Replace("{Map-Id}", mapId).Replace("{Icon-SVG}", "").Replace("{dest-latitude}", Convert.ToString(dest["lat"])).Replace("{dest-longitude}", Convert.ToString(dest["lng"])).Replace("{count}", mapId).Replace("{Icon}", icon);

            return script;
        }

        //for multiple markers
        private string AddScriptsMarkersToImagingMap(string mapId, string originAddress, List<ImagingLocationsViewModel> imagingLocations)
        {
            HttpClient http = new HttpClient();
            var origin = GetAddress(originAddress);
            var lat = origin["lat"];
            var lng = origin["lng"];
            var locations = ImagingLocationStringHelper.CreateString(imagingLocations);
            var icon = _configuration.GetSection("DulyIcon").Value;
            var script = @"<script type='text/javascript'>
    var locations = [
      {Locations}
    ];
    
    var map = new google.maps.Map(document.getElementById('{Map-Id}'), {
      zoom: 9,
      center: new google.maps.LatLng({origin-latitude}, {origin-longitude}),
    });
    
    var infowindow = new google.maps.InfoWindow();

    var marker, i;
    var image = 'data:image/png;base64,{Icon}';
    marker = new google.maps.Marker({
        position: new google.maps.LatLng(locations[0][0], locations[0][1]),
        map: map,
      });
    
    for (i = 1; i < locations.length; i++) {  
      marker = new google.maps.Marker({
        position: new google.maps.LatLng(locations[i][0], locations[i][1]),
        map: map,
        icon: image
      });
    }
  </script> ".Replace("{Locations}", locations).Replace("{Map-Id}", mapId).Replace("{Icon-SVG}", "").Replace("{Icon}", icon).Replace("{origin-latitude}", Convert.ToString(lat)).Replace("{origin-longitude}", Convert.ToString(lng));

            return script;
        }
    }
}