// <copyright file="Wipfli.Client.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

#pragma warning disable SA1601 // Partial elements should be documented

using Duly.Common.Annotations.Json;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Wipfli.Adapter.Client
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface declaration")]
    public interface IWipfliClient
    {
        System.Threading.Tasks.Task<Schedule> GetScheduleDaysForProvider(
            ScheduleDaysForProviderSearchCriteria body,
            System.Threading.CancellationToken cancellationToken = default);

        System.Threading.Tasks.Task<ScheduledAppointmentWithInsurance> ScheduleAppointmentWithInsurance(
            ScheduleAppointmentWithInsuranceRequest body,
            System.Threading.CancellationToken cancellationToken = default);
    }

    public partial class WipfliClient : IWipfliClient
    {
        private readonly HttpClient _httpClient;
        private readonly System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

        public WipfliClient(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
        }

        public bool ReadResponseAsString { get; set; }
        protected Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings => _settings.Value;

        private Newtonsoft.Json.JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            UpdateJsonSerializerSettings(settings);
            return settings;
        }

        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings);
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url);
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder);
        partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response);

        public virtual async System.Threading.Tasks.Task<Schedule> GetScheduleDaysForProvider(ScheduleDaysForProviderSearchCriteria body, System.Threading.CancellationToken cancellationToken = default)
        {
            if (body == null)
                throw new System.ArgumentNullException(nameof(body));

            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append("WIPFLI/api/epic/2017/PatientAccess/External/GetScheduleDaysForProvider2/Scheduling/Open/Provider/GetScheduleDays2");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    var content_ = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(body, _settings.Value));
                    content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request_.Content = content_;
                    request_.Method = new System.Net.Http.HttpMethod("POST");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Schedule>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }

                            return objectResponse_.Object;
                        }
                        else if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<FaultResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }

                            throw new ApiException<FaultResponse>(objectResponse_.Object.ExceptionMessage, status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else if (status_ == 401)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<string>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }

                            throw new ApiException<FaultResponse>(
                                "Access denied. Please contact system administrator.",
                                status_,
                                objectResponse_.Text,
                                headers_,
                                new FaultResponse { Message = objectResponse_.Object },
                                null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        public virtual async System.Threading.Tasks.Task<ScheduledAppointmentWithInsurance> ScheduleAppointmentWithInsurance(ScheduleAppointmentWithInsuranceRequest body, System.Threading.CancellationToken cancellationToken = default)
        {
            if (body == null)
                throw new System.ArgumentNullException(nameof(body));

            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append("WIPFLI/api/epic/2018/PatientAccess/External/ScheduleAppointmentWithInsurance/Scheduling2018/Open/ScheduleWithInsurance");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("POST");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);

                    var requestDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(Newtonsoft.Json.JsonConvert.SerializeObject(body));
                    var url_ = QueryHelpers.AddQueryString(urlBuilder_.ToString(), requestDictionary);
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ScheduledAppointmentWithInsurance>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }

                            return objectResponse_.Object;
                        }
                        else if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<FaultResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }

                            throw new ApiException<FaultResponse>(objectResponse_.Object.ExceptionMessage, status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else if (status_ == 401)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<string>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }

                            throw new ApiException<FaultResponse>(
                                "Access denied. Please contact system administrator.",
                                status_,
                                objectResponse_.Text,
                                headers_,
                                new FaultResponse { Message = objectResponse_.Object },
                                null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        protected virtual async System.Threading.Tasks.Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(System.Net.Http.HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Threading.CancellationToken cancellationToken)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default, string.Empty);
            }

            if (ReadResponseAsString)
            {
                var responseText = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    var typedBody = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                    return new ObjectResponseResult<T>(typedBody, responseText);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false))
                    using (var streamReader = new System.IO.StreamReader(responseStream))
                    using (var jsonTextReader = new Newtonsoft.Json.JsonTextReader(streamReader))
                    {
                        var serializer = Newtonsoft.Json.JsonSerializer.Create(JsonSerializerSettings);
                        var typedBody = serializer.Deserialize<T>(jsonTextReader);
                        return new ObjectResponseResult<T>(typedBody, string.Empty);
                    }
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }

        protected struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                Object = responseObject;
                Text = responseText;
            }

            public T Object { get; }

            public string Text { get; }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.15.5.0 (NJsonSchema v10.6.6.0 (Newtonsoft.Json v12.0.0.0))")]
    public partial class FaultResponse
    {
        public string Message { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionType { get; set; }

        public string StackTrace { get; set; }

        public FaultResponse InnerException { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.15.5.0 (NJsonSchema v10.6.6.0 (Newtonsoft.Json v12.0.0.0))")]
    public partial class ApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.15.5.0 (NJsonSchema v10.6.6.0 (Newtonsoft.Json v12.0.0.0))")]
    public partial class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }

    public class ScheduleDaysForProviderSearchCriteria
    {
        /// <summary>
        /// Date range start date.
        /// </summary>
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateFormat)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date range end date.
        /// </summary>
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateFormat)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Provider ID Type ("External").
        /// </summary>
        public string ProviderIDType { get; set; }

        /// <summary>
        /// Provider ID value.
        /// </summary>
        public string ProviderID { get; set; }

        /// <summary>
        /// Collection of department IDs.
        /// </summary>
        public Identity[] DepartmentIDs { get; set; }

        /// <summary>
        /// Collection of Visit type IDs.
        /// </summary>
        public Identity[] VisitTypeIDs { get; set; }
    }

    public class ScheduleAppointmentWithInsuranceRequest
    {
        /// <summary>
        /// Patient ID value.
        /// </summary>
        public string PatientID { get; set; }

        /// <summary>
        /// Patient ID Type.
        /// </summary>
        public string PatientIDType { get; set; }

        /// <summary>
        /// Department ID value.
        /// </summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// Department ID Type.
        /// </summary>
        public string DepartmentIDType { get; set; }

        /// <summary>
        /// Visit Type ID value.
        /// </summary>
        public string VisitTypeID { get; set; }

        /// <summary>
        /// Visit Type ID Type.
        /// </summary>
        public string VisitTypeIDType { get; set; }

        /// <summary>
        /// Prevents appointment creation if true.
        /// </summary>
        public bool IsReviewOnly { get; set; }

        /// <summary>
        /// Provider ID value.
        /// </summary>
        public string ProviderID { get; set; }

        /// <summary>
        /// Provider ID Type.
        /// </summary>
        public string ProviderIDType { get; set; }

        /// <summary>
        /// Appointment date.
        /// </summary>
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateFormat)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Appointment time.
        /// </summary>
        public TimeSpan Time { get; set; }
    }

    public class Schedule
    {
        public System.Collections.Generic.ICollection<Department> Departments { get; set; }
        public Provider Provider { get; set; }
        public System.Collections.Generic.ICollection<VisitType> VisitTypes { get; set; }
        public System.Collections.Generic.ICollection<ScheduleDay> ScheduleDays { get; set; }
    }

    public class ScheduledAppointmentWithInsurance
    {
        public string InsuranceQueryKey { get; set; }
        public string InsuranceQueryErrorCode { get; set; }
        public string InsuranceQueryErrorCodeDescription { get; set; }
        public Appointment Appointment { get; set; }
    }

    public class Appointment
    {
        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }
        public System.Collections.Generic.ICollection<string> PatientInstructions { get; set; }
        public Provider Provider { get; set; }
        public Department Department { get; set; }
        public VisitType VisitType { get; set; }
        public Patient Patient { get; set; }
        public System.Collections.Generic.ICollection<Identity> ContactIDs { get; set; }
        public DateTime ScheduledTime { get; set; } = DateTime.UtcNow;
    }

    public class Patient
    {
        public string Name { get; set; }
        public System.Collections.Generic.ICollection<Identity> IDs { get; set; }
    }

    public class Department
    {
        public string Name { get; set; }
        public System.Collections.Generic.ICollection<string> LocationInstructions { get; set; }
        public System.Collections.Generic.ICollection<Identity> IDs { get; set; }
        public Address Address { get; set; }
        public Specialty Specialty { get; set; }
        public OfficialTimeZone OfficialTimeZone { get; set; }
        public System.Collections.Generic.ICollection<Phone> Phones { get; set; }
    }

    public class Provider
    {
        public string DisplayName { get; set; }
        public System.Collections.Generic.ICollection<Identity> IDs { get; set; }
    }

    public class VisitType
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public System.Collections.Generic.ICollection<string> PatientInstructions { get; set; }
        public System.Collections.Generic.ICollection<Identity> IDs { get; set; }
    }

    public class ScheduleDay
    {
        public DateTime Date { get; set; }
        public System.Collections.Generic.ICollection<Slot> Slots { get; set; }
        public Provider Provider { get; set; }

        [JsonProperty("Department")]
        public Identity DepartmentID { get; set; }

        [JsonProperty("VisitType")]
        public Identity VisitTypeID { get; set; }
    }

    public class Identity
    {
        /// <summary>
        /// ID value.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string ID { get; set; }

        /// <summary>
        /// ID type.
        /// </summary>
        public string Type { get; set; }
    }

    public class Address
    {
        public System.Collections.Generic.ICollection<string> StreetAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string HouseNumber { get; set; }
        public State State { get; set; }
        public Country Country { get; set; }
        public District District { get; set; }
        public County County { get; set; }
    }

    public class Specialty
    {
        public string Number { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
        public string ExternalName { get; set; }
    }

    public class OfficialTimeZone
    {
        public string Number { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Phone
    {
        public string Type { get; set; }
        public string Number { get; set; }
    }

    public class State
    {
        public string Number { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Country
    {
        public string Number { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
    }

    public class District
    {
        public string Number { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
    }

    public class County
    {
        public string Number { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Slot
    {
        public TimeSpan Time { get; set; }
        public TimeSpan DisplayTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
    }
}

#pragma warning restore SA1601
