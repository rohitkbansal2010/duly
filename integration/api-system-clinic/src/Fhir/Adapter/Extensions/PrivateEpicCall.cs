// <copyright file="PrivateEpicCall.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Common.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Extensions
{
    public class PrivateEpicCall : IPrivateEpicCall
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        public PrivateEpicCall(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async Task<GetPatientPhotoRoot> GetPatientPhotoAsync(PatientPhotoByParam request)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new System.Uri(_configuration.GetSection("FhirServerUrls:BaseUrlPrivateEpic").Value);

            string jsonRequestBody = System.Text.Json.JsonSerializer.Serialize(request);
            httpRequestMessage.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonConvert.DeserializeObject<GetPatientPhotoRoot>(content);
                return responseData;
            }

            return null;
        }
    }
}