using AudioServer.Models;
using AudioServer.Models.DTOs;
using AudioServer.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AudioServer.Services
{
    public class FileServerClient : IFileServerClient
    {
        private HttpClient _httpClient;
        private string _fileServerURL;

        public FileServerClient() { }

        public FileServerClient(IHttpClientFactory clientFactory, IOptions<Hosts> hosts)
        {
            _httpClient = clientFactory.CreateClient();
            _fileServerURL = hosts.Value.FileServerURL + "/files";
        }

        public async Task<string> AddFile(string fileContent, string relativePath)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _fileServerURL);

            var requestBody = new SaveFileTO()
            {
                FileContent = fileContent,
                RelativePath = relativePath
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> ReadFile(string relativePath)
        {
            var urlBuilder = new UriBuilder(_fileServerURL);
            var queryBuilder = new QueryBuilder { { "path", relativePath } };
            urlBuilder.Query = queryBuilder.ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());
            request.Content = new StringContent(relativePath);

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
