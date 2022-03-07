using System;
using System.IO.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Account.DomainApi.Domain.Account.Processor;
using Services.Account.DomainApi.Domain.Applicant.Processor;
using Services.Account.DomainApi.Model;
using Services.Account.Repository;
using Services.Account.Repository.DomainRepository;

namespace Services.Account.DomainApi
{
    public static class ServiceExtenstion
    {

        private  static readonly IConfiguration Configuration;

        /// <summary>
        ///  Adds the services.
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="env">Environment</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public static void AddServices(this IServiceCollection services,
            IWebHostEnvironment env,
            IConfiguration configuration,
            ILogger logger)
        {
            services.AddTransient<IApplicantProcessor, ApplicantProcessor>();
            services.AddTransient<IAccountProcessor, AccountProcessor>();        
            services.AddSingleton<IFileSystem, FileSystem>();
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
        }
    }
}
