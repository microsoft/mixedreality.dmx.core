// ---------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------

using System.Text.Json.Serialization;
using DMX.Core.Api.Brokers.Artifacts;
using DMX.Core.Api.Brokers.DateTimes;
using DMX.Core.Api.Brokers.LabApis;
using DMX.Core.Api.Brokers.Loggings;
using DMX.Core.Api.Brokers.Queues;
using DMX.Core.Api.Brokers.Storages;
using DMX.Core.Api.Services.Foundations.ExternalLabs;
using DMX.Core.Api.Services.Foundations.LabCommandEvents;
using DMX.Core.Api.Services.Foundations.LabCommands;
using DMX.Core.Api.Services.Foundations.Labs;
using DMX.Core.Api.Services.Foundations.LabWorkflowCommands;
using DMX.Core.Api.Services.Foundations.LabWorkflowEvents;
using DMX.Core.Api.Services.Foundations.LabWorkflows;
using DMX.Core.Api.Services.Orchestrations.LabCommands;
using DMX.Core.Api.Services.Orchestrations.Labs;
using DMX.Core.Api.Services.Orchestrations.LabWorkflows;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace DMX.Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddHttpClient();
            services.AddLogging();
            services.AddDbContext<StorageBroker>();
            AddAuthentication(services);
            AddBrokers(services);
            AddFoundationServices(services);
            AddOrchestrationServices(services);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo
                    {
                        Title = "DMX.Core.Api",
                        Version = "v1"
                    });

                options.CustomSchemaIds(type => type.ToString());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        url: "/swagger/v1/swagger.json",
                        name: "DMX.Core.Api v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            MapControllersForEnvironments(app, env);
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IExternalLabApiBroker, ExternalLabApiBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IQueueBroker, QueueBroker>();
            services.AddTransient<IArtifactsBroker, ArtifactsBroker>();
        }

        private static void AddFoundationServices(IServiceCollection services)
        {
            services.AddTransient<IExternalLabService, ExternalLabService>();
            services.AddTransient<ILabService, LabService>();
            services.AddTransient<ILabCommandService, LabCommandService>();
            services.AddTransient<ILabCommandEventService, LabCommandEventService>();
            services.AddTransient<ILabWorkflowService, LabWorkflowService>();
            services.AddTransient<ILabWorkflowCommandService, LabWorkflowCommandService>();
            services.AddTransient<ILabWorkflowEventService, LabWorkflowEventService>();
        }

        private static void AddOrchestrationServices(IServiceCollection services)
        {
            services.AddTransient<ILabOrchestrationService, LabOrchestrationService>();
            services.AddTransient<ILabCommandOrchestrationService, LabCommandOrchestrationService>();
            services.AddTransient<ILabWorkflowOrchestrationService, LabWorkflowOrchestrationService>();
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(
                        Configuration.GetSection("AzureAd"));
        }

        private static void MapControllersForEnvironments(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            app.UseEndpoints(endpoints =>
            {
                if (env.IsDevelopment())
                {
                    endpoints.MapControllers().AllowAnonymous();
                }
                else
                {
                    endpoints.MapControllers();
                }
            });
        }
    }
}
