// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Configurations;
using Microsoft.Extensions.Configuration;
using RESTFulSense.Clients;

namespace DMX.Core.Api.Brokers.ReverbApis
{
    public partial class ReverbApiBroker : IReverbApiBroker
    {
        private readonly IRESTFulApiFactoryClient apiClient;
        private readonly HttpClient httpClient;

        public ReverbApiBroker(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.apiClient = GetApiClient(configuration);
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
    }
}
