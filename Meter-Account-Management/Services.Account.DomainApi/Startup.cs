using System;
using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Services.Account.DomainApi.Filter;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Services.Account.DomainApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _hostingEnv = env;
            var loggerFactory = new LoggerFactory();
            _logger = loggerFactory.CreateLogger<Startup>();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddControllers();
            services.AddAntiforgery();
            services.AddHttpContextAccessor();
            services.AddLogging();
            services.AddResponseCompression();
            services.AddRouting();
            services.AddScoped<GlobalExceptionFilter>();

            services.AddControllers(config =>
            {
                config.Filters.Add<GlobalExceptionFilter>();
            });
            services.AddMvcCore().AddFluentValidation(fvOptions =>
            {
                fvOptions.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                fvOptions.DisableDataAnnotationsValidation = true;
                fvOptions.ImplicitlyValidateChildProperties = true;
            });
            services.AddMvcCore(option => option.EnableEndpointRouting = false).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddServices(_hostingEnv, Configuration, _logger);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Energy.Domain.MeterAccountManagement", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Services.Account.DomainApi v1"));
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
