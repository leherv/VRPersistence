using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VRPersistence.Config;
using VRPersistence.DataStores;
using VRPersistence.Services;

namespace VRPersistence
{
    public class Startup
    {
        private readonly IWebHostEnvironment hostEnvironment;
        
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            this.hostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var connectionString = GetConnectionString(Configuration, hostEnvironment);
            services.AddDbContext<VRPersistenceDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IReleaseService, ReleaseService>();
            services.AddScoped<IReleaseDataStore, ReleaseDataStore>();
            
            services.Configure<TrackedMediaSettings>(Configuration.GetSection("TrackedMediaSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, VRPersistenceDbContext dbContext)
        {
            dbContext.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static string GetConnectionString(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            return $"Host={EnvVar("VRPersistenceDbHost")};" +
                   $"Port={EnvVar("VRPersistenceDbPort")};" +
                   $"Database={EnvVar("VRPersistenceDbName")};" +
                   $"Username={EnvVar("VRPersistenceDbUsername")};" +
                   $"Password={EnvVar("VRPersistenceDbPassword")}";
        }

        private static string EnvVar(string envVar)
        {
            return Environment.GetEnvironmentVariable(envVar);
        }
    }
}