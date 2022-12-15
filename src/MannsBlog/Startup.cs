// Copyright (C) 2021 Sascha Manns <Sascha.Manns@outlook.de>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using HealthChecks.UI.Client;
using MannsBlog.Config;
using MannsBlog.EntityFramework.Context;
using MannsBlog.Helpers;
using MannsBlog.Logger;
using MannsBlog.MetaWeblog;
using MannsBlog.Models;
using MannsBlog.Repositories;
using MannsBlog.Services;
using MannsBlog.Services.DataProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WilderMinds.AzureImageStorageService;
using WilderMinds.MetaWeblog;

namespace MannsBlog
{
    /// <summary>
    /// Class for Startup the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The cors policy name.
        /// </summary>
        public const string CorsPolicyName = "_saigkill_cors";

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// The env.
        /// </summary>
        private readonly IHostEnvironment _env;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="env">The env.</param>
        public Startup(IConfiguration config, IHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="env">The env.</param>
        public void ConfigureServices(IServiceCollection svcs)
        {
            svcs.Configure<AppSettings>(_config);

            if (_env.IsDevelopment() && _config.GetValue<bool>("MailService:TestInDev") == false)
            {
                svcs.AddTransient<IMailService, LoggingMailService>();
            }
            else if (_config.GetValue<bool>("Blog:UseSendgrid") == true)
            {
                svcs.AddTransient<IMailService, SendgridMailService>();
            }
            else
            {
                svcs.AddTransient<IMailService, OutlookMailService>();
            }

            svcs.AddTransient<GoogleCaptchaService>();

            svcs.AddDbContext<MannsContext>(ServiceLifetime.Scoped);

            svcs.AddIdentity<MannsUser, IdentityRole>()
              .AddEntityFrameworkStores<MannsContext>();

            if (_config.GetValue<bool>("MannsDb:TestData"))
            {
                svcs.AddScoped<IMannsRepository, MemoryRepository>();
            }
            else
            {
                svcs.AddScoped<IMannsRepository, MannsRepository>();
            }

            svcs.AddCors(setup =>
            {
                var httpsOrigin = this._config.GetValue<string>("Blog:HTTPSUrl");
                var httpOrigin = this._config.GetValue<string>("Blog:HTTPUrl");
                setup.AddPolicy(CorsPolicyName, cfg =>
                {
                    if (this._env.IsDevelopment())
                    {
                        cfg.AllowAnyMethod();
                        cfg.AllowAnyOrigin();
                        cfg.AllowAnyHeader();
                    }
                    else
                    {
                        cfg.WithMethods("POST");
                        cfg.WithOrigins(httpsOrigin, httpOrigin);
                        cfg.AllowAnyHeader();
                    }
                });
            });

            svcs.ConfigureHealthChecks(_config);

            svcs.AddTransient<MannsInitializer>();
            svcs.AddScoped<AdService>();

            // Data Providers (non-EF)
            svcs.AddScoped<CalendarProvider>();
            svcs.AddScoped<PublicationsProvider>();
            svcs.AddScoped<TalksProvider>();
            svcs.AddScoped<VideosProvider>();
            svcs.AddScoped<JobsProvider>();
            svcs.AddScoped<TestimonialsProvider>();
            svcs.AddScoped<CertsProvider>();
            svcs.AddScoped<ProjectsProvider>();

            if ((_env.IsDevelopment() && _config.GetValue<bool>("BlobStorage:TestInDev") == false) ||
                _config["BlobStorage:Account"] == "FOO")
            {
                svcs.AddTransient<IAzureImageStorageService, FakeAzureImageService>();
            }
            else
            {
                svcs.AddAzureImageStorageService(
                  _config["BlobStorage:Account"],
                  _config["BlobStorage:Key"],
                  _config["BlobStorage:StorageUrl"]);
            }

            // Supporting Live Writer (MetaWeblogAPI)
            svcs.AddMetaWeblog<MannsWeblogProvider>();

            //DSGVO
            svcs.Configure<CookiePolicyOptions>(options =>
            {
                // Sets the display of the Cookie Consent banner (/Pages/Shared/_CookieConsentPartial.cshtml).
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            // Add Caching Support
            svcs.AddMemoryCache(opt => opt.ExpirationScanFrequency = TimeSpan.FromMinutes(5));

            // Add MVC to the container
            svcs.AddLocalization(opt =>
            {
                opt.ResourcesPath = "Resources";
            });

            svcs.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-DE"),
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            svcs.AddMvc()
                .AddMvcLocalization()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);


            svcs.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            if (!svcs.Any(x => x.ServiceType == typeof(HttpClient)))
            {
                svcs.AddScoped<HttpClient>(s =>
                {
                    var uriHelper = s.GetRequiredService<NavigationManager>();
                    return new HttpClient()
                    {
                        BaseAddress = new Uri(uriHelper.BaseUri),
                    };
                });
            }

            svcs.AddServerSideBlazor();

            svcs.AddApplicationInsightsTelemetry(_config);

        }

        /// <summary>
        /// Configures the specified application.
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mailService">The mail service.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="contextAccessor">The context accessor.</param>
        public void Configure(IApplicationBuilder app,
                              ILoggerFactory loggerFactory,
                              IMailService mailService,
                              IServiceScopeFactory scopeFactory,
                              IOptions<AppSettings> settings,
                              IHttpContextAccessor contextAccessor)
        {
            // Add the following to the request pipeline only in development environment.
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Early so we can catch the StatusCode error
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Exception");

                // Support logging to email
                loggerFactory.AddEmail(mailService, contextAccessor, LogLevel.Critical);

                app.UseHttpsRedirection();
            }

            // Syncfusion License Key
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(settings.Value.Syncfusion.License);

            // Support MetaWeblog API
            app.UseMetaWeblog("/livewriter");

            // Rewrite old URLs to new URLs
            app.UseUrlRewriter();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Email Uncaught Exceptions
            if (settings.Value.Exceptions.TestEmailExceptions || !_env.IsDevelopment())
            {
                app.UseMiddleware<EmailExceptionMiddleware>();
            }

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();


            // Globalizing & Localizing
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>()?.Value;
            app.UseRequestLocalization(options);

            app.UseEndpoints(cfg =>
            {
                cfg.MapControllers();
                cfg.MapHealthChecks("/_hc");
                cfg.MapHealthChecks("/_hc.json", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                });
                cfg.MapBlazorHub();
            });
        }
    }
}
