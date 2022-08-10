// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using DMX.Core.Api.Models.Configurations;
using DMX.Core.Api.Models.Foundations.ExternalLabs;
using Microsoft.Extensions.Configuration;
using RESTFulSense.Clients;

namespace DMX.Core.Api.Brokers.LabApis
{
    public partial class ExternalLabApiBroker : IExternalLabApiBroker
    {
        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient apiClient;
        private readonly string getAllDevicesAccessKey;
        private readonly string getAvailableDevicesAccessKey;
        private readonly ExternalLabServiceInformation externalLabServiceInformation;

        public ExternalLabApiBroker(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.apiClient = GetApiClient(configuration);
            this.getAvailableDevicesAccessKey = GetAvailableDevicesAccessToken(configuration);
            this.getAllDevicesAccessKey = GetAllDevicesAccessToken(configuration);
            this.externalLabServiceInformation = GetExternalLabServiceInformation(configuration);
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

        private static string GetAvailableDevicesAccessToken(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations =
                configuration.Get<LocalConfigurations>();

            return localConfigurations.ApiConfigurations.AvailableDevicesAccessKey;
        }

        private static string GetAllDevicesAccessToken(IConfiguration configuration)
        {
            LocalConfigurations localConfigurations =
                configuration.Get<LocalConfigurations>();

            return localConfigurations.ApiConfigurations.AllDevicesAccessKey;
        }

        private static ExternalLabServiceInformation GetExternalLabServiceInformation(IConfiguration configuration) =>
            configuration
                .GetSection(nameof(ExternalLabServiceInformation))
                    .Get<ExternalLabServiceInformation>();
    }
}
