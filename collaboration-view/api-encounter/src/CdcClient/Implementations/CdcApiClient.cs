// <copyright file="CdcApiClient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.CdcClient.Interfaces;
using Duly.CollaborationView.CdcClient.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Duly.CollaborationView.CdcClient.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ICdcApiClient"/>
    /// </summary>
    public class CdcApiClient : ICdcApiClient
    {
        private const string RouteSuffixGetContentModelCatalogs = "contentmodel/catalogs?";
        private const string RouteSuffixFindContentModelCatalogRelationsByCodes = "relation?";

        private readonly HttpClient _httpClient;

        public CdcApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<CdcContentModelCatalog[]> GetContentModelCatalogsAsync(string contentModel)
        {
            if (contentModel == null)
                throw new ArgumentNullException(nameof(contentModel));

            return RequestContentModelCatalogsAsync(contentModel);
        }

        public Task<CdcCatalogSourceCodeRelations[]> FindContentModelCatalogRelationsByCodesAsync(
            string contentModel,
            string catalog,
            string[] codes)
        {
            if (contentModel == null)
                throw new ArgumentNullException(nameof(contentModel));

            if (catalog == null)
                throw new ArgumentNullException(nameof(catalog));

            if (codes == null)
                throw new ArgumentNullException(nameof(codes));

            return RequestContentModelCatalogRelationsByCodesAsync(contentModel, catalog, codes);
        }

        protected virtual async Task<T> ReadObjectResponseAsync<T>(HttpResponseMessage response)
        {
            try
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                using var streamReader = new StreamReader(responseStream);
                using var jsonTextReader = new JsonTextReader(streamReader);

                var serializer = JsonSerializer.Create();
                var typedContent = serializer.Deserialize<T>(jsonTextReader);

                return typedContent;
            }
            catch (JsonException exception)
            {
                var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                throw new ApiException(message, (int)response.StatusCode, exception);
            }
        }

        private static string AppendToURL(string baseURL, params string[] segments)
        {
            return string.Join(
                "/",
                new[] { baseURL?.TrimEnd('/') }.Concat(segments.Select(s => s.Trim('/'))));
        }

        private async Task<CdcCatalogSourceCodeRelations[]> RequestContentModelCatalogRelationsByCodesAsync(string contentModel, string catalog, string[] codes)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(RouteSuffixFindContentModelCatalogRelationsByCodes);
            urlBuilder.Append("contentModel=").Append(Uri.EscapeDataString(contentModel)).Append('&');
            urlBuilder.Append("catalog=").Append(Uri.EscapeDataString(catalog)).Append('&');

            foreach (var code in codes)
            {
                urlBuilder.Append("codes=").Append(Uri.EscapeDataString(code)).Append('&');
            }

            urlBuilder.Length--;

            using var request = new HttpRequestMessage();

            request.Method = new HttpMethod("GET");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url = AppendToURL(_httpClient.BaseAddress?.AbsoluteUri, urlBuilder.ToString());
            request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

            var response = await _httpClient.SendAsync(request);
            try
            {
                var status = (int)response.StatusCode;
                switch (status)
                {
                    case 200:
                        {
                            var objectResponse = await ReadObjectResponseAsync<CdcCatalogSourceCodeRelations[]>(response);
                            return objectResponse;
                        }

                    case 400:
                        throw new ApiException("Returns a validation error result.", status, null);
                    case 500:
                        throw new ApiException("Returns a server error", status, null);
                    default:
                        throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, null);
                }
            }
            finally
            {
                response.Dispose();
            }
        }

        private async Task<CdcContentModelCatalog[]> RequestContentModelCatalogsAsync(string contentModel)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(RouteSuffixGetContentModelCatalogs);
            urlBuilder.Append("contentModel=").Append(Uri.EscapeDataString(contentModel));

            using var request = new HttpRequestMessage();

            request.Method = new HttpMethod("GET");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url = AppendToURL(_httpClient.BaseAddress?.AbsoluteUri, urlBuilder.ToString());
            request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

            var response = await _httpClient.SendAsync(request);
            try
            {
                var status = (int)response.StatusCode;
                switch (status)
                {
                    case 200:
                    {
                        var objectResponse = await ReadObjectResponseAsync<CdcContentModelCatalog[]>(response);
                        return objectResponse;
                    }

                    case 400:
                        throw new ApiException("Returns a validation error result.", status, null);
                    case 500:
                        throw new ApiException("Returns a server error", status, null);
                    default:
                        throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, null);
                }
            }
            finally
            {
                response.Dispose();
            }
        }
    }
}
