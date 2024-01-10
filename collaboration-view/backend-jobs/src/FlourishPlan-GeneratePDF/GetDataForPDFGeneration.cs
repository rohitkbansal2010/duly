using Duly.UI.Flourish.GeneratePDF.AuthenticationModel;
using Duly.UI.Flourish.GeneratePDF.Data;
using Duly.UI.Flourish.GeneratePDF.Helpers;
using Duly.UI.Flourish.GeneratePDF.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDFGeneratorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Duly.UI.Flourish.GeneratePDF
{
    public class GetDataForPDFGeneration
    {
        private HttpClient _httpClient = new HttpClient();
      //  private readonly ILogger _logger;
        private readonly DBHelper objDBHelper;
        private readonly IConfiguration _configuration;
        private string _baseApiAddress = string.Empty;        
        private string _appointmentId = string.Empty;
        private int _totalRecordsCount = 0;
        private int _successfulProcessedRecordsCount = 0;
        private int _failedRecordsCount = 0;        
        private bool _isError = false;

        public GetDataForPDFGeneration(IConfiguration config)
        {
            /*_logger = logger*/;
            _configuration = config;
            _baseApiAddress = _configuration.GetSection("Values:BaseApiAddress").Value;
            objDBHelper = new DBHelper(_configuration);
        }
        public void Generator()
        {
            var accessToken = GetAccessToken().Result;

            //var config = new ConfigurationBuilder()
            //      .AddJsonFile($"appsettings.json", true, true).Build();

            //var builder = new ConfigurationBuilder()
            //                    .SetBasePath(Directory.GetCurrentDirectory())
            //                    .AddJsonFile("Parameters.json", optional: true, reloadOnChange: true);

            // var config = new ConfigurationBuilder()
            //  .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
            //// .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            // .AddEnvironmentVariables()
            // .Build();



            _httpClient.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
            _httpClient.DefaultRequestHeaders.Add("Authorization", accessToken);
            _httpClient.DefaultRequestHeaders.Add("subscription-key", _configuration.GetSection("Authentication:Environment:ServiceSettings:SubscriptionKey").Value);
           
            var allPendingData = GetPendingData();

            if (allPendingData != null)
            {
                _totalRecordsCount = allPendingData.Count;
                var followUpLocations = objDBHelper.FollowUpLocation();
                foreach (var pending in allPendingData)
                {
                    //Do not run job if phone number is missing.
                    if (!String.IsNullOrEmpty(pending.PhoneNumber))
                    {
                        var followUpLocation = followUpLocations.Where(location => location.Id == pending.SiteId).FirstOrDefault();

                        AllRequests(pending, followUpLocation);
                    }
                    else
                    {
                        LogException("FlourishJob", null, pending.AppointmentId,
                            "Phone number not found",
                            "No API Call",
                            "Job is not running for this Appointment because phone number is missing");
                    }
                }
                objDBHelper.InsertJobDetail("FlourishJob", _totalRecordsCount, _successfulProcessedRecordsCount, _failedRecordsCount);
            }
            else
            {
                LogException("FlourishJob", null, "No appointment found", "No appointment found", "Not getting appoitnemnt data, DBHelper class , GetPendingAppointmentsBySMSStatus() function", "No appointment found");
            }
        }

        private async Task<string> GetAccessToken()
        {
           
            var authUrl = _configuration.GetSection("Authentication:Environment:ServiceSettings:AuthUrl").Value;

            var formContent = new FormUrlEncodedContent(new[]
                 {
                    new KeyValuePair<string, string>("Scope", _configuration.GetSection("Authentication:Environment:ServiceSettings:Scope").Value),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", _configuration.GetSection("Authentication:Credentials:Username").Value),
                    new KeyValuePair<string, string>("password", _configuration.GetSection("Authentication:Credentials:Password").Value)
                });

            Uri baseUri = new Uri(authUrl);
            _httpClient.BaseAddress = baseUri;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.ConnectionClose = true;

            var authenticationString = $"{_configuration.GetSection("Authentication:Environment:ServiceSettings:ClientId").Value}:{_configuration.GetSection("Authentication:Environment:ServiceSettings:ClientSecret").Value}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _configuration.GetSection("Authentication:Environment:ServiceSettings:AuthUrlVersionUrl").Value);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = formContent;

            var response = await _httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<AuthResponse>(data);
                //_logger.LogInformation($"{data}");
                return "Bearer " + responseData.access_token;
            }
            return "";

        }

        private List<AppointmentDetailsBySMSStatusViewModel> GetPendingData()
        {
            List<AppointmentDetailsBySMSStatusViewModel> _pendingData = null;
            try
            {
                _pendingData = objDBHelper.GetPendingAppointmentsBySMSStatus("Pending,Error");
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, "", ex.StackTrace, "GetPendingAppointments funciton reported some error. it gets data from app database.", ex.Message);
            }
            return _pendingData;
        }
       
        public void AllRequests(AppointmentDetailsBySMSStatusViewModel pendingData, SitesDetailsViewModel followUpLocation)
        {
            _isError = false;
            _appointmentId = pendingData.AppointmentId;
            // Update SMS status to schedule 
            objDBHelper.UpdateAppointmentSMSStatus(_appointmentId, "Scheduled");


            string ProviderId = pendingData.ProviderId;
            string PatientId = pendingData.PatientId;
            int afterVisitPDFId = 0;
            try
            {
                if (_isError)
                    return;

                var checkoutDetailsData = GetCheckoutDetails(_appointmentId);
                if (_isError)
                    return;

                IEnumerable<FollowUpViewModel> allFollowUp = new List<FollowUpViewModel>();
                IEnumerable<FollowUpViewModel> followUpData = new List<FollowUpViewModel>();
                // Follow up data.
                followUpData = GetDoctorFollowUpDetails(PatientId);
                if (_isError)
                    return;

                // Get Patient data.
                var patientData = GetPatientDetails(PatientId);
                if (_isError)
                    return;

                // Get patient Address
                var patientAddress = GetPatientAddess(patientData);
                if (_isError)
                    return;

                // Get Location.
                var value = _configuration.GetSection("Values:GoogleApiKey").Value;
                var patientLocationlatLong = GetLatLngByAddress(patientAddress, _configuration.GetSection("Values:GoogleApiKey").Value);
                if (_isError)
                    return;

                // Get lab location.
                var labLocationDetails = GetImagingLocations("laboratory-services", Convert.ToString(patientLocationlatLong["lat"]), Convert.ToString(patientLocationlatLong["lng"])).OrderBy(x => x.Distance).ToList();
                if (_isError)
                    return;

                // Get lab orders
                var labsOrders = GetlabsAndImagingOrders("Labs", PatientId, _appointmentId);
                if (_isError)
                    return;

                // Get Imaging orders
                var ImagingOrders = GetlabsAndImagingOrders("Imaging", PatientId, _appointmentId);
                if (_isError)
                    return;
                Dictionary<string, List<ImagingLocationsViewModel>> allImagingLocations = new Dictionary<string, List<ImagingLocationsViewModel>>();
                var imagingLocation = new List<ImagingLocationsViewModel>();
                // Get Imaging location.    
                foreach (var imaging in ImagingOrders.TestOrder)
                {
                    var serviceSlug = ImagingLocationHelper.GetImagingLocationServiceSlug(imaging.OrderName, imaging.Code);
                    if (!allImagingLocations.ContainsKey(serviceSlug))
                    {
                        var imagingLocations = GetImagingLocations(serviceSlug, Convert.ToString(patientLocationlatLong["lat"]), Convert.ToString(patientLocationlatLong["lng"]));
                        if (imagingLocations != null)
                        {
                            allImagingLocations.Add(serviceSlug, imagingLocations);
                        }
                    }
                }
                imagingLocation = FilterAllImagingLocations(allImagingLocations);
                if (_isError)
                    return;

                List<ScheduleReferral> scheduleReferral = new List<ScheduleReferral>();
                try
                {
                    if (checkoutDetailsData != null)
                    {
                        foreach (var data in checkoutDetailsData.ScheduleFollowUpList)
                        {
                            if (data.Type == "R")
                            {
                                scheduleReferral.Add(data);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LogException("FlourishJob", null, _appointmentId, e.StackTrace, "imagingLocations iteration error", e.Message);
                }
                var openReferrals = GetOpenReferrals(PatientId);
                if (_isError)
                    return;

                var prescriptions = GetPrescriptions(PatientId, _appointmentId);
                if (_isError)
                    return;

                var preferredPharmacy = GetPreferredPharmacy(PatientId);
                if (_isError)
                    return;

                Console.WriteLine("All data is collected successfully!");
                CreatePdf pdf = new CreatePdf(_configuration);
                var pdfData = "";
                try
                {
                    pdfData = pdf.CreatePDF(checkoutDetailsData, patientData, followUpData.ToList(), labLocationDetails, labsOrders, ImagingOrders, prescriptions, preferredPharmacy, openReferrals, scheduleReferral, imagingLocation, pendingData, followUpLocation);
                }
                catch (Exception ex)
                {
                    objDBHelper.LogException("FlourishJob", null, _appointmentId, ex.Message, "Create PDF Method", "Pdf not created, error occurred");
                }
                if (_isError)
                    return;

                afterVisitPDFId = AfterVisitPdf(PatientId, ProviderId, pdfData, _appointmentId, false, pendingData.PhoneNumber);
                if (_isError)
                    return;

                if (afterVisitPDFId != 0)
                {
                    Console.Write("Pdf is saved!!");
                    Console.WriteLine("AFtervisitpdfid :" + afterVisitPDFId);

                    if (!string.IsNullOrEmpty(pendingData.PhoneNumber))
                    {
                        var sendSmsResponse = AfterVisitPdfSendSms(_appointmentId, PatientId, afterVisitPDFId.ToString(), pendingData.PhoneNumber).Result;
                    }
                    else
                    {
                        objDBHelper.LogException("FlourishJob", afterVisitPDFId, _appointmentId, "Phone number not found that is why SMS not sent", "Phone number not found", "Phone number is missing");
                        Console.WriteLine("Phone number is not given!");
                    }
                }
            }
            catch (Exception e)
            {
                LogException("FlourishJob", afterVisitPDFId, _appointmentId, e.StackTrace, "Error in Method : AllRequests", e.Message);
            }
        }

        private int AfterVisitPdf(string PatientId, string ProviderId, string pdf, string AppointmentId, bool triggerSMS, string phoneNumber)
        {
            int _response = 0;
            try
            {
                var content = new AfterVisitPdf();
                content.AfterVisitPDF = pdf;
                content.AppointmentId = AppointmentId;
                content.PatientId = PatientId;
                content.PhoneNumber = phoneNumber;
                content.ProviderId = ProviderId;
                content.TriggerSMS = triggerSMS;
                _response = objDBHelper.PostAfterVisitPDF(content);
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, AppointmentId, ex.StackTrace, "DB Call", ex.Message);
            }
            return _response;
        }
        private async Task<HttpResponseMessage> AfterVisitPdfSendSms(string AppointmentId, string PatientId, string PdfId, string PhoneNumber)
        {
            Console.WriteLine("Sending SMS.");
            var uri = _baseApiAddress + $"/sendsms/SendSms";
            try
            {
                var content = new SendSmsViewModel();
                content.PdfId = PdfId;
                content.PatientId = PatientId;
                content.AppointmentId = AppointmentId;
                content.PhoneNumber = PhoneNumber;

                string jsonRequestBody = System.Text.Json.JsonSerializer.Serialize(content);
                var body = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
                var _response = await _httpClient.PostAsync(uri, body);

                if (_response.IsSuccessStatusCode)
                {
                    _successfulProcessedRecordsCount++;
                    // Update SMS successfully sent to patient.
                    objDBHelper.UpdateAppointmentSMSStatus(_appointmentId, "Sent");
                    Console.WriteLine("SmsSent");
                }
                else
                {
                    LogException("FlourishJob", Convert.ToInt32(PdfId), _appointmentId, _response.ReasonPhrase, uri, Convert.ToString(_response.StatusCode));
                    Console.WriteLine("SMS Not sent");
                }
                return _response;
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", Convert.ToInt32(PdfId), AppointmentId, ex.StackTrace, uri, ex.Message);
                return null;
            }
        }
        private OpenReferralViewModel GetOpenReferrals(string patientId)
        {
            Console.WriteLine("Getting open referrals detail.");
            var uri = _baseApiAddress + $"/referral/Referral/GetOpenReferralDataByPatientId/{patientId}";
            var dateTime = DateTime.UtcNow.Day;
            var openReferrals = new OpenReferralViewModel();
            try
            {

                var response = _httpClient.GetAsync(uri);
                response.Wait();
                var _response = response.Result;
                if (_response.IsSuccessStatusCode)
                {
                    var result = _response.Content.ReadAsStringAsync();
                    result.Wait();
                    openReferrals = JsonConvert.DeserializeObject<OpenReferralViewModel>(result.Result);
                    if (openReferrals.OpenRefferalResponse.Count() > 0)
                    {
                        openReferrals.OpenRefferalResponse = openReferrals.OpenRefferalResponse.Where(referral => dateTime - Int32.Parse(referral.ReferredDate.Split("-")[1]) <= 1);
                    }
                }
                else
                {
                    LogException("FlourishJob", null, _appointmentId, _response.ReasonPhrase, uri, Convert.ToString(_response.StatusCode));
                }

                Console.WriteLine("Open referrals detail found.");
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, _appointmentId, ex.StackTrace, uri, ex.Message);
            }
            return openReferrals;
        }

        private List<ImagingLocationsViewModel> FilterAllImagingLocations(Dictionary<string, List<ImagingLocationsViewModel>> allImagingLocations)
        {
            if (allImagingLocations.Count >= 0)
            {
                List<ImagingLocationsViewModel> allFilteredImagingLocations = new List<ImagingLocationsViewModel>();
                Dictionary<int, KeyValuePair<int, ImagingLocationsViewModel>> filteredLocations = new Dictionary<int, KeyValuePair<int, ImagingLocationsViewModel>>();
                try
                {
                    var count = 1;
                    foreach (var locations in allImagingLocations)
                    {
                        foreach (var location in locations.Value)
                        {
                            filteredLocations.Add(location.Id, new KeyValuePair<int, ImagingLocationsViewModel>(count, location));
                        }
                        count++;
                        break;
                    }
                    foreach (var locations in allImagingLocations)
                    {
                        if (count != 2)
                        {
                            foreach (var location in locations.Value)
                            {
                                if (filteredLocations.ContainsKey(location.Id))
                                {
                                    var prevCount = filteredLocations.GetValueOrDefault(location.Id);
                                    filteredLocations[location.Id] = new KeyValuePair<int, ImagingLocationsViewModel>(prevCount.Key + 1, location);
                                }
                            }
                        }
                        count++;
                    }
                    var locationImaging = filteredLocations.OrderBy(x => x.Value.Value.Distance).OrderByDescending(x => x.Value.Key);
                    foreach (var location in locationImaging)
                    {
                        allFilteredImagingLocations.Add(location.Value.Value);
                    }
                    return allFilteredImagingLocations;
                }
                catch (Exception ex)
                {
                    LogException("FlourishJob", null, _appointmentId, ex.StackTrace, "Filtering All Imagination", ex.Message);
                }
            }
            return null;
        }

        private List<ImagingLocationsViewModel> GetImagingLocations(string service_slug, string latitude, string longitude)
        {
            Console.WriteLine("Getting Imaging location detail");
            var uri = _baseApiAddress + $"/imagingdetail/ImagingDetail/ImagingLocation?service_slug={service_slug}&latitude={latitude}&longitude={longitude}";
            List<ImagingLocationsViewModel> imagingLocations = new List<ImagingLocationsViewModel>();
            try
            {
                var response = _httpClient.GetAsync(uri);
                response.Wait();
                var _response = response.Result;
                if (_response.IsSuccessStatusCode)
                {
                    var data = _response.Content.ReadAsStringAsync();
                    data.Wait();
                    imagingLocations = JsonConvert.DeserializeObject<List<ImagingLocationsViewModel>>(data.Result);
                }
                else
                {
                    LogException("FlourishJob", null, _appointmentId, _response.ReasonPhrase, uri, Convert.ToString(_response.StatusCode));
                }
                Console.WriteLine("Getting Imaging location detail found");
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, _appointmentId, ex.StackTrace, uri, ex.Message);
            }
            return imagingLocations;
        }


        private PharmacyViewModel GetPreferredPharmacy(string patientId)
        {
            Console.WriteLine("Getting Preferred pharmacy detail");
            var uri = _baseApiAddress + $"/Pharmacy/{patientId}";
            PharmacyViewModel data = new PharmacyViewModel();
            try
            {
                var response = _httpClient.GetAsync(uri);
                response.Wait();
                var _response = response.Result;
                if (_response.IsSuccessStatusCode)
                {
                    var result = _response.Content.ReadAsStringAsync();
                    result.Wait();
                    data = JsonConvert.DeserializeObject<PharmacyViewModel>(result.Result);
                }
                else
                {
                    LogException("FlourishJob", null, _appointmentId, _response.ReasonPhrase, uri, Convert.ToString(_response.StatusCode));
                }
                Console.WriteLine("Preferred pharmacy detail found");
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, _appointmentId, ex.StackTrace, uri, ex.Message);
            }
            return data;
        }

        private PrescriptionViewModel GetPrescriptions(string patientId, string appointmentId)
        {
            Console.WriteLine("Getting Prescriptions detail");
            var uri = _baseApiAddress + $"/patientId/{patientId}/appointmentId/{appointmentId}";
            PrescriptionViewModel prescriptions = new PrescriptionViewModel();
            try
            {
                var response = _httpClient.GetAsync(uri);
                var _response = response.Result;
                if (_response.IsSuccessStatusCode)
                {
                    var result = _response.Content.ReadAsStringAsync();
                    prescriptions = JsonConvert.DeserializeObject<PrescriptionViewModel>(result.Result);
                }
                else
                {
                    LogException("FlourishJob", null, _appointmentId, _response.ReasonPhrase, uri, Convert.ToString(_response.StatusCode));
                }
                Console.WriteLine("Prescriptions detail found");
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, appointmentId, ex.StackTrace, uri, ex.Message);

            }
            return prescriptions;
        }

        private LabAndImagingOrders GetlabsAndImagingOrders(string type, string patientId, string appointmentId)
        {
            Console.WriteLine("Getting Lab and Imaging orders");
            var uri = _baseApiAddress + $"/ServiceRequest/{patientId}/appointment/{appointmentId}/type/{type}";
            LabAndImagingOrders allOrders = new LabAndImagingOrders();
            try
            {
                var response = _httpClient.GetAsync(uri);
                response.Wait();
                var _response = response.Result;
                if (_response.IsSuccessStatusCode)
                {
                    var result = _response.Content.ReadAsStringAsync();
                    result.Wait();
                    allOrders = JsonConvert.DeserializeObject<LabAndImagingOrders>(result.Result);
                }
                else
                {
                    LogException("FlourishJob", null, _appointmentId, _response.ReasonPhrase, uri, Convert.ToString(_response.StatusCode));
                }
                Console.WriteLine("Lab and Imaging orders found");
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, appointmentId, ex.StackTrace, uri, ex.Message);
            }
            return allOrders;
        }

        public string GetPatientAddess(PatientViewModel patientData)
        {
            var patientAddress = "";
            if (patientData.PatientAddress != null)
            {

                foreach (var patient in patientData.PatientAddress)
                {
                    if (patient.Use == "Home")
                    {
                        foreach (var line in patient.Line)
                        {
                            patientAddress += line + " ";
                        }
                        patientAddress += patient.City + " " + patient.State + " " + patient.District + " " + patient.PostalCode;
                    }
                }
            }
            return patientAddress;
        }

        public JToken GetLatLngByAddress(string Address, string api_key)
        {
            Console.WriteLine("Getting Latitude and Longitude details");
            JToken patientLocationlatLong = new JObject();
            var uri = $"https://maps.googleapis.com/maps/api/geocode/json?key={api_key}&address=" + Address;
            try
            {
                var response = _httpClient.GetAsync(uri);
                response.Wait();
                var _response = response.Result;
                if (_response.IsSuccessStatusCode)
                {
                    var result = _response.Content.ReadAsStringAsync();
                    result.Wait();
                    JObject data = JObject.Parse(result.Result);
                    patientLocationlatLong = data["results"][0]["geometry"]["location"];
                }
                else
                {
                    LogException("FlourishJob", null, _appointmentId, _response.ReasonPhrase, uri, Convert.ToString(_response.StatusCode));
                }
                Console.WriteLine("Latitude and Longitude details found");
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, _appointmentId, ex.StackTrace, uri, ex.Message);

            }
            return patientLocationlatLong;
        }

        private List<LabDetailsByLatLongViewModel> GetLabDetails(JToken locaionLatLong)
        {
            var lat = locaionLatLong["lat"];
            var lng = locaionLatLong["lng"];
            var uri = _baseApiAddress + $"/labdetail/LabDetail/ByLatLng/{lat}/{lng}";
            List<LabDetailsByLatLongViewModel> labLocation = new List<LabDetailsByLatLongViewModel>();
            try
            {
                var response = _httpClient.GetAsync(uri);
                response.Wait();
                var _response = response.Result;
                if (_response.IsSuccessStatusCode)
                {
                    var result = _response.Content.ReadAsStringAsync();
                    result.Wait();
                    labLocation = JsonConvert.DeserializeObject<List<LabDetailsByLatLongViewModel>>(result.Result);
                    if (labLocation != null)
                        labLocation = labLocation.OrderBy(x => Convert.ToDecimal(x.Distance)).ToList();
                }
                else
                {
                    LogException("FlourishJob", null, _appointmentId, _response.ReasonPhrase, uri, Convert.ToString(_response.StatusCode));
                }
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, _appointmentId, ex.StackTrace, uri, ex.Message);
            }
            return labLocation;
        }

        private PatientViewModel GetPatientDetails(string patientId)
        {
            Console.WriteLine("Getting Patient details");
            var uri = _baseApiAddress + "/Patients/" + patientId;
            try
            {
                var response = _httpClient.GetAsync(uri);
                response.Wait();
                var _response = response.Result;
                PatientViewModel patient = new PatientViewModel();
                if (_response.IsSuccessStatusCode)
                {
                    var data = _response.Content.ReadAsStringAsync();
                    data.Wait();
                    patient = JsonConvert.DeserializeObject<PatientViewModel>(data.Result);
                }
                else
                {
                    LogException("FlourishJob", null, _appointmentId, _response.ReasonPhrase, uri, Convert.ToString(_response.StatusCode));
                }
                Console.WriteLine("Patient details found");
                return patient;
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, _appointmentId, ex.StackTrace, uri, ex.Message);
                return null;
            }
        }

        private IOrderedEnumerable<LabAndImagingLocationViewModel> GetLabsAndImagingLocations(string patientId)
        {
            var uri = _baseApiAddress + "/v1/appointment/" + patientId + "/location-details__ZZ";
            try
            {
                var response = _httpClient.GetAsync(uri);
                response.Wait();
                var test = response.Result;
                List<LabAndImagingLocationViewModel> allLabsLocations = new List<LabAndImagingLocationViewModel>();
                if (test.IsSuccessStatusCode)
                {
                    var data = test.Content.ReadAsStringAsync();
                    data.Wait();
                    allLabsLocations = JsonConvert.DeserializeObject<List<LabAndImagingLocationViewModel>>(data.Result);
                }
                var allSortedLabsLocations = allLabsLocations.OrderBy(x => x.Location.distance);
                return allSortedLabsLocations;
            }
            catch (Exception ex)
            {
                PostSMSStatus content = new PostSMSStatus();
                content.AppointmentId = _appointmentId;
                content.SMSStatus = "Error";
                LogException("FlourishJob", null, _appointmentId, ex.StackTrace, uri, ex.Message);
                return null;
            }
        }

        private CheckoutDetailsViewModel GetCheckoutDetails(string appointmentId)
        {
            Console.WriteLine("Getting checkout details");
            var uri = _baseApiAddress + $"/{Convert.ToInt32(appointmentId)}/CheckOutDetails";
            CheckoutDetailsViewModel allCheckoutDetails = new CheckoutDetailsViewModel();
            try
            {
                var response = _httpClient.GetAsync(uri);
                response.Wait();
                var _response = response.Result;

                if (_response.IsSuccessStatusCode)
                {
                    var result = _response.Content.ReadAsStringAsync();
                    result.Wait();
                    allCheckoutDetails = JsonConvert.DeserializeObject<CheckoutDetailsViewModel>(result.Result);
                }
                else
                {
                    LogException("FlourishJob", null, _appointmentId, _response.ReasonPhrase, uri, _response.StatusCode.ToString());
                }
                Console.WriteLine("Checkout details Found");
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, appointmentId, ex.StackTrace, uri, ex.Message);
            }
            return allCheckoutDetails;
        }

        private IEnumerable<FollowUpViewModel> GetDoctorFollowUpDetails(string patientId)
        {
            Console.WriteLine("Getting Followup details");
            var uri = _baseApiAddress + $"/v1/appointment/{patientId}/follow-up-details";
            IEnumerable<FollowUpViewModel> followUpData = new List<FollowUpViewModel>();
            try
            {
                var response = _httpClient.GetAsync(uri);
                response.Wait();
                var _response = response.Result;
                if (_response.IsSuccessStatusCode)
                {
                    var result = _response.Content.ReadAsStringAsync();
                    result.Wait();
                    followUpData = JsonConvert.DeserializeObject<List<FollowUpViewModel>>(result.Result);
                }
                else
                {
                    LogException("FlourishJob", null, _appointmentId, _response.ReasonPhrase, uri, _response.StatusCode.ToString());
                }
                Console.WriteLine("Followup details found.");
                return followUpData;
            }
            catch (Exception ex)
            {
                LogException("FlourishJob", null, _appointmentId, ex.StackTrace, uri, ex.Message);
                return followUpData;
            }
        }

        private void LogException(string jobName, int? afterVisitPdfId, string appointmentId, string errDesc, string errInApi, string shortErr)
        {
            objDBHelper.UpdateAppointmentSMSStatus(appointmentId, "Error");
            objDBHelper.LogException(jobName, afterVisitPdfId, appointmentId, errDesc, errInApi, shortErr);
            _failedRecordsCount++;
            _isError = true;
        }
    }
}