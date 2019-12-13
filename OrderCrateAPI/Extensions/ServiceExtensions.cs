using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderCrateAPI.Contracts;
using OrderCrateAPI.LoggerService;
using OrderCrateAPI.Entities;
using Microsoft.EntityFrameworkCore;
using OrderCrateAPI.Repository;

namespace OrderCrateAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    //.AllowAnyMethod()
                    .WithMethods("POST", "GET", "PUT", "PATCH")
                    //.AllowAnyHeader()
                    .WithHeaders("accept", "content-type")
                    .AllowCredentials());
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        public static void ConfigureMsSQLContext(this IServiceCollection services, IConfiguration config)
        {
            // OrderCrateDB
            var connectionString = config["ConnectionStrings:OrderCrateDB"];
            services.AddDbContext<OrdercratedbContext>(o => o.UseSqlServer(connectionString));
        }
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}
