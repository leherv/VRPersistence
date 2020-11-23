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
            services.AddDbContext<VRPersistenceDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("Db")));

            services.AddScoped<IReleaseService, ReleaseService>();
            services.AddScoped<IReleaseDataStore, ReleaseDataStore>();
            services.AddScoped<INotificationEndpointService, NotificationEndpointService>();
            services.AddScoped<INotificationDataStore, NotificationDataStore>();
            services.AddScoped<IMediaDataStore, MediaDataStore>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<ISubscriptionDataStore, SubscriptionDataStore>();
            
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
    }
}