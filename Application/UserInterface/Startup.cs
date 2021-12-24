using System;
using Castle.Core.Internal;
using Firebase.Database;
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
            services.AddScoped<OutputConverter>();
            services.AddScoped<OutputProvider>();
            services.AddScoped<FormatterChooser>();
            services.AddScoped<DatabaseEntityConverter>();
            
            services.AddScoped<IInputParser, XlsxInputParser>();
            services.AddScoped<IInputParser, TxtInputParser>();
            services.AddScoped<ITimetableMaker, GeneticAlgorithm>();
            services.AddScoped<ITimetableMaker, GraphAlgorithm>();
            services.AddScoped<IOutputFormatter, XlsxOutputFormatter>();
            services.AddScoped<IOutputFormatter, PdfOutputFormatter>();
        }

        private void ConfigureDatabases(IServiceCollection services)
        {
            var firebaseUrl = configuration.GetConnectionString("FirebaseUrl");
            if (!firebaseUrl.IsNullOrEmpty())
            {
                services.AddSingleton(new FirebaseClient(firebaseUrl));
                services
                    .AddSingleton<IDatabaseWrapper<string, DatabaseSlot>, FirebaseWrapper<string, DatabaseSlot>>();
                services
                    .AddSingleton<IDatabaseWrapper<string, DatabaseTimeslot>,
                        FirebaseWrapper<string, DatabaseTimeslot>>();

            }
            
            var mysqlSlotsConnectionString = configuration.GetConnectionString("MySQLSlotConnection");
            if (!mysqlSlotsConnectionString.IsNullOrEmpty())
            {
                services.AddDbContext<MySQLContext<string, DatabaseSlot>>(options  => options
                    .UseMySql(mysqlSlotsConnectionString, ServerVersion.AutoDetect(mysqlSlotsConnectionString)));
                services.AddSingleton<IDatabaseWrapper<string, DatabaseSlot>, MySQLWrapper<string, DatabaseSlot>>();
            }
            
            var mysqlTimeslotsConnectionString = configuration.GetConnectionString("MySQLTimeslotConnection");
            if (!mysqlSlotsConnectionString.IsNullOrEmpty())
            {
                services.AddDbContext<MySQLContext<string, DatabaseTimeslot>>(options  => options
                    .UseMySql(mysqlTimeslotsConnectionString, ServerVersion.AutoDetect(mysqlTimeslotsConnectionString)));
                services.AddSingleton<IDatabaseWrapper<string, DatabaseTimeslot>, MySQLWrapper<string, DatabaseTimeslot>>();
            }
            
            services.AddSingleton<IDatabaseWrapper<string, DatabaseSlot>, DictionaryWrapper<string, DatabaseSlot>>();
            services.AddSingleton<IDatabaseWrapper<string, DatabaseTimeslot>, DictionaryWrapper<string, DatabaseTimeslot>>();
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