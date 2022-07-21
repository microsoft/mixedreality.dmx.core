// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Configurations;
using Microsoft.Extensions.Configuration;
using RESTFulSense.Clients;

namespace DMX.Core.Api.Brokers.LabApis
{
    public partial class ExternalLabApiBroker : IExternalLabApiBroker
    {
        private readonly IRESTFulApiFactoryClient apiClient;
        private readonly HttpClient httpClient;
        private readonly string accessKey;

        public ExternalLabApiBroker(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.apiClient = GetApiClient(configuration);
            this.accessKey = GetApiAccessToken(configuration);
        }

        private async ValueTask<T> GetAsync<T>(string relativeUrl) =>
            await this.apiClient.GetContentAsync<T>(relativeUrl);

        private async ValueTask<U> PostAync<T, U>(string relativeUrl, T content) =>
            await this.apiClient.PostContentAsync<T, U>(relativeUrl, content);

        private IRESTFulApiFactoryClient GetApiClient(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations = configuration.Get<LocalConfigurations>();
            string apiBaseUrl = localConfigurations.ApiConfigurations.Url;
            this.httpClient.BaseAddress = new Uri(apiBaseUrl);

            return new RESTFulApiFactoryClient(this.httpClient);
        }

        private static string GetApiAccessToken(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations =
                configuration.Get<LocalConfigurations>();

            return localConfigurations.ApiConfigurations.AccessKey;
        }
    }
}
