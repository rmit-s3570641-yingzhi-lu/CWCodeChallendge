using CW.Infrastructure;
using CW.Infrastructure.Interfaces;
using CWCodeChallendge.Extensions;
using CWCodeChallendge.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace CWCodeChallendge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Bind database options. Invalid configuration will terminate the application startup.
            var (serviceEndpoint, authKey) = Configuration.GetSection("ConnectionStrings").Get<ConnectionStringOptions>();
            var cosmosDbOptions = Configuration.GetSection("CosmosDb").Get<CosmosDbOptions>();
            var (databaseName, collectionData) = cosmosDbOptions;
            var collectionNames = collectionData.Select(c => c.Name).ToList();

            // Add CosmosDb. This verifies database and collections existence.
            services.AddCosmosDb(serviceEndpoint, authKey, databaseName, collectionNames);

            services.AddScoped<IProductRepository, ProductRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
