using Bloggo.Helpers;
using Bloggo.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bloggo.Services
{
    public class HttpHandler
    {
        private HttpClient _httpClient;
        private UserDatabaseService _userDatabaseService;

        public HttpHandler(UserDatabaseService userDatabaseService)
        {
            _httpClient = new HttpClient();
            _userDatabaseService = userDatabaseService;
        }

        public async Task<T> Get<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request);
        }

        public async Task Post(string uri, object value)
        {
            var request = createRequest(HttpMethod.Post, uri, value);
            await sendRequest(request);
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            var request = createRequest(HttpMethod.Post, uri, value);
            return await sendRequest<T>(request);
        }

        public async Task Put(string uri, object value)
        {
            var request = createRequest(HttpMethod.Put, uri, value);
            await sendRequest(request);
        }

        public async Task<T> Put<T>(string uri, object value)
        {
            var request = createRequest(HttpMethod.Put, uri, value);
            return await sendRequest<T>(request);
        }

        public async Task Delete(string uri)
        {
            var request = createRequest(HttpMethod.Delete, uri);
            await sendRequest(request);
        }

        public async Task<T> Delete<T>(string uri)
        {
            var request = createRequest(HttpMethod.Delete, uri);
            return await sendRequest<T>(request);
        }

        // helper methods
        private HttpRequestMessage createRequest(HttpMethod httpMethod, string uri, object value = null)
        {
            var request = new HttpRequestMessage(httpMethod, uri);

            if (value != null)
            {
                request.Content = new StringContent(value.ToString()); // will need to work on this
            }

            return request;
        }

        private async Task sendRequest(HttpRequestMessage httpRequest)
        {
            //await addJwtHeader(request);

            // send request
            using var response = await _httpClient.SendAsync(httpRequest);

            // auto logout on 401 response
            // if (response.StatusCode == HttpStatusCode.Unauthorized)
            // {
            //     _navigationManager.NavigateTo("account/logout");
            //     return;
            // }

            await handleErrors(response);
        }

        private async Task<T> sendRequest<T>(HttpRequestMessage httpRequest)
        {
            //await addJwtHeader(request);
            
            // send request
            using var response = await _httpClient.SendAsync(httpRequest);

            // // auto logout on 401 response
            // if (response.StatusCode == HttpStatusCode.Unauthorized)
            // {
            //     _navigationManager.NavigateTo("account/logout");
            //     return default;
            // }

            await handleErrors(response);

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new StringConverter());
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private async Task handleErrors(HttpResponseMessage response)
        {
            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }
        }

    }
}