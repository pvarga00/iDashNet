using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;

namespace iDashNet
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddLogging();

            //services.AddSingleton<Controllers.HomeController>();


            //// Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "iDash", Version = "v1" });
                c.TagActionsBy(api => api.RelativePath);
            });


            services.Configure<MySettings>(options => Configuration.GetSection("MySettings").Bind(options));
            services.Configure<List<SonarQubeApp>>(options => Configuration.GetSection("SonarQubeApps").Bind(options));
            services.Configure<List<GoogleAnalyticsApp>>(options => Configuration.GetSection("GoogleAnalyticsApps").Bind(options));
            services.Configure<List<AppDynamicsApp>>(options => Configuration.GetSection("AppDynamicsApps").Bind(options));
            services.Configure<List<DCRUMApp>>(options => Configuration.GetSection("DCRUMApps").Bind(options));
            services.Configure<List<ZabbixApp>>(options => Configuration.GetSection("ZabbixApps").Bind(options));
            services.Configure<List<CherwellApp>>(options => Configuration.GetSection("CherwellApps").Bind(options));
            services.Configure<List<HomePageApp>>(options => Configuration.GetSection("HomePageApps").Bind(options));
            services.Configure<List<AvailabilityApps>>(options => Configuration.GetSection("AvailibilityApps").Bind(options));

            services.AddMvc();


            services.AddOptions();





            //services.AddSingleton<IConfiguration>(Configuration);

            var connection = @"Server=5PLJWF2\LOCALSQLSERVER;Database=iDash;Trusted_Connection=True;integrated security=True";
            services.AddDbContext<iDashNet.Models.iDashContext>(options => options.UseSqlServer(connection));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=HomeIndex}/{id?}");
            });




            //app.UseMvcWithDefaultRoute();

            //// Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
   



            //// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "iDash V1");
            });
        }
    }
}
