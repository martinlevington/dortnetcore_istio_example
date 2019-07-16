using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pingService.Models;

namespace pingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Registers required services for health checks
            services.AddHealthChecks()
            .AddCheck<ReadyHealthCheck>("Ready", failureStatus: null, tags: new[] { "ready", });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            // This will register the health checks middleware at the URL /health.
            // 
            // By default health checks will return a 200 with 'Healthy'.
            // - No health checks are registered by default, the app is healthy if it is reachable
            // - The default response writer writes the HealthCheckStatus as text/plain content
            //
            // This is the simplest way to use health checks, it is suitable for systems
            // that want to check for 'liveness' of an application.
          //  app.UseHealthChecks("/meta/health");

            // The readiness check uses all registered checks with the 'ready' tag.
            app.UseHealthChecks("/meta/health/ready", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("ready"),
            });

            // The liveness filters out all checks and just returns success
            app.UseHealthChecks("/meta/health/live", new HealthCheckOptions()
            {
                // Exclude all checks, just return a 200.
                Predicate = (check) => false,
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
