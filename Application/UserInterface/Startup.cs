using System;
using Castle.Core.Internal;
using Infrastructure;
using LiteDB;
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

            ConfigureDatabases(services);

            services.AddScoped<ParserChooser>();
            services.AddScoped<InputProvider>();
            services.AddScoped<AlgorithmChooser>();
            services.AddScoped<TimetableMakingProvider>();
            services.AddScoped<DatabaseProvider>();
            services.AddScoped<DatabasesChooser>();
            services.AddScoped<OutputProvider>();
            services.AddScoped<FormatterChooser>();
            
            services.AddScoped<IInputParser, XlsxInputParser>();
            services.AddScoped<IInputParser, TxtInputParser>();
            services.AddScoped<ITimetableMaker, GeneticAlgorithm>();
            services.AddScoped<ITimetableMaker, GraphAlgorithm>();
            services.AddScoped<OutputFormatter, XlsxOutputFormatter>();
            services.AddScoped<OutputFormatter, PdfOutputFormatter>();
        }

        private void ConfigureDatabases(IServiceCollection services)
        {
            var firebaseUrl = configuration.GetConnectionString("FirebaseUrl");
            if (!firebaseUrl.IsNullOrEmpty())
            {
                services.AddSingleton<IDatabaseWrapper<string, DatabaseSlot>>(
                    new FirebaseWrapper<string, DatabaseSlot>(firebaseUrl, "User"));
                services.AddSingleton<IDatabaseWrapper<string, DatabaseTimeslot>>(
                    new FirebaseWrapper<string, DatabaseTimeslot>(firebaseUrl, "User"));
                services.AddSingleton<IDatabaseWrapper<string, DatabaseTimeSchedule>>(
                    new FirebaseWrapper<string, DatabaseTimeSchedule>(firebaseUrl, "User"));
            }
            
            var mysqlConnectionString = configuration.GetConnectionString("MySQLConnection");
            if (!mysqlConnectionString.IsNullOrEmpty())
            {
                services.AddDbContext<MySQLContext<string, DatabaseSlot>>(options  => options
                    .UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));
                services.AddSingleton<IDatabaseWrapper<string, DatabaseSlot>, MySQLWrapper<string, DatabaseSlot>>();
                
                services.AddDbContext<MySQLContext<string, DatabaseTimeslot>>(options  => options
                    .UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));
                services.AddSingleton<IDatabaseWrapper<string, DatabaseTimeslot>, MySQLWrapper<string, DatabaseTimeslot>>();
                
                services.AddDbContext<MySQLContext<string, DatabaseTimeSchedule>>(options  => options
                    .UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));
                services.AddSingleton<IDatabaseWrapper<string, DatabaseTimeSchedule>, MySQLWrapper<string, DatabaseTimeSchedule>>();
            }
            
            services.AddSingleton<IDatabaseWrapper<string, DatabaseSlot>, DictionaryWrapper<string, DatabaseSlot>>();
            services.AddSingleton<IDatabaseWrapper<string, DatabaseTimeslot>, DictionaryWrapper<string, DatabaseTimeslot>>();
            services.AddSingleton<IDatabaseWrapper<string, DatabaseTimeSchedule>, DictionaryWrapper<string, DatabaseTimeSchedule>>();
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