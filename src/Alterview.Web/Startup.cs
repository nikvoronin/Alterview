﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alterview.Core.Interfaces;
using Alterview.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Alterview.Web
{
    public class Startup
    {
        private const string DefaultConnectionName = "Default";
        private const string ControllersSectionName = "Controllers";
        private const string SuppressMapClientErrorsKey = "SuppressMapClientErrors";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        internal bool SuppressMapClientErrorsValue => (bool)(Configuration
                        ?.GetSection(ControllersSectionName)
                        ?.GetValue(typeof(bool), SuppressMapClientErrorsKey)
                        ?? false);

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString(DefaultConnectionName);
            services.AddSingleton<IEventsRepository>(s => new EventsRepository(connectionString));
            services.AddSingleton<ISportsRepository>(s => new SportsRepository(connectionString));
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = SuppressMapClientErrorsValue;
                });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
