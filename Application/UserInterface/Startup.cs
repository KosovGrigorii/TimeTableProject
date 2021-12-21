using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimetableApplication;
using TimetableDomain;

namespace UserInterface
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();
            
            var firebaseUrl = configuration.GetConnectionString("FirebaseUrl");
            var mysqlConnectionString = configuration.GetConnectionString("MySQLConnection");
            
            services.AddDbContext<MySQLDataContext>(
                options => options.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString))
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());
            
            services.AddScoped<IInputParser, XlsxInputParser>();
            services.AddScoped<IInputParser, TxtInputParser>();
            services.AddScoped<ITimetableMaker, GeneticAlgorithm>();
            services.AddScoped<OutputFormatter, XlsxOutputFormatter>();
            
            services.AddSingleton<IDatabaseClient, SimpleDictionary>();
            services.AddSingleton<IDatabaseClient, MySQLClient>();
            services.AddSingleton<IDatabaseClient>(new FirebaseClient(firebaseUrl));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=MainPage}/{action=Index}/{id?}");
            });
        }
    }
}