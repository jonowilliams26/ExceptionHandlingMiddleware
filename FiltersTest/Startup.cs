using FiltersTest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IO;

namespace FiltersTest
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FiltersTest", Version = "v1" });
            });
            services.AddTransient<ITodoService, TodoService>();
            services.AddScoped<INotificationService, NotificationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next();
            });

            app.UseExceptionHandler(app =>
            {
                app.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                    var notificationService = context.RequestServices.GetRequiredService<INotificationService>();
                    var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
                    await notificationService.Send(exception);

                    // Read Request body
                    if (context.Request.ContentLength.HasValue)
                    {
                        var stream = new StreamReader(context.Request.Body);
                        stream.BaseStream.Seek(0, SeekOrigin.Begin);
                        var body = await stream.ReadToEndAsync();
                        logger.LogInformation("Request Body: {Body}", body);
                    }

                    // Handle exceptions here
                    await context.Response.WriteAsJsonAsync(new { Error = "Sorry something went wrong" }); 
                });
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FiltersTest v1"));
                //app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
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
