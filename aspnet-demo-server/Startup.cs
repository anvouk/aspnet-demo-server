using aspnet_demo_server.Controllers.HealthChecks;
using aspnet_demo_server.Filters;
using aspnet_demo_server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;

namespace aspnet_demo_server
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
            services
                .AddControllers(options =>
                    options.Filters.Add(new ExceptionFilter()))
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                ;
            services.AddHttpClient();
            services.AddHealthChecks()
                .AddCheck<SimpleHealthCheck>("simple")
                ;
            services
                .AddSingleton<IPlantManager, PlantManager>()
                ;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "aspnet-demo-server", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            appLifetime.ApplicationStopping.Register(ApplicationStopping);
            appLifetime.ApplicationStopped.Register(ApplicationStopped);

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                };
                options.GetLevel = (ctx, d, ex) =>
                {
                    return ctx.Response.StatusCode switch
                    {
                        _ when ex != null => LogEventLevel.Error,
                        _ when ctx.GetEndpoint()?.DisplayName == "Health checks" => LogEventLevel.Verbose,
                        > 499 => LogEventLevel.Error,
                        _ => LogEventLevel.Information,
                    };
                };
            });

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(builder => builder.WithOrigins("*").AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "aspnet-demo-server v1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        private void ApplicationStopping()
        {
            Log.Information("Graceful shutdown in progress");
        }

        private void ApplicationStopped()
        {
            Log.Information("Graceful shutdown completed");
        }
    }
}
