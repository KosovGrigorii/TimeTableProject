using System.Collections.Generic;
using Accord.Genetic;
using Castle.Core.Internal;
using Firebase.Database;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddMvc();

            ConfigureDatabase(services);

            services
                .AddSingleton<DependenciesDictionary<FilterGetterParameters, FilterPartialViewData,
                    IDictionaryType<FilterGetterParameters, FilterPartialViewData>>>();
            services.AddSingleton<IDictionaryType<FilterGetterParameters, FilterPartialViewData>, FilterDays>();
            services.AddSingleton<IDictionaryType<FilterGetterParameters, FilterPartialViewData>, FilterDaysCount>();
            
            services.AddSingleton<TimespanDbConverter>(); 
            services.AddSingleton<TimeslotDbConverter>(); 
            services.AddSingleton<TimeDurationDbConverter>();
            services.AddSingleton<SlotInfoDbConverter>();
            
            services.AddSingleton<DependenciesDictionary<AlgoritmInput, IEnumerable<TimeSlot>,
                IDictionaryType<AlgoritmInput, IEnumerable<TimeSlot>>>>();
            services.AddScoped<FiltersPageInterface>();
            services.AddScoped<TimetableResultsInterface>();
            services.AddScoped<InputRecipient>();
            services.AddScoped<FilterNamesGetter>();
            services.AddScoped<TimetableTaskLauncher>();
            services.AddScoped<OutputExecutor>();  //App

            services
                .AddSingleton<DependenciesDictionary<IFormFile, UserInput, IDictionaryType<IFormFile, UserInput>>>();
            services.AddSingleton<IDictionaryType<IFormFile, UserInput>, XlsxInputParser>();
            services.AddSingleton<IDictionaryType<IFormFile, UserInput>, TxtInputParser>();
            services.AddSingleton<InputProvider>(); //Input

            services.AddSingleton<ConverterToAlgorithmInput>();
            services.AddSingleton<IDictionaryType<AlgoritmInput, IEnumerable<TimeSlot>>, GeneticAlgorithm>();
            services.AddSingleton<IDictionaryType<AlgoritmInput, IEnumerable<TimeSlot>>, GraphAlgorithm>();
            services.AddSingleton<FilterHandler>();
            services.AddSingleton<EliteSelection>();
            services.AddSingleton<FitnessFunction>();    //Algo
            
            services
                .AddSingleton<DependenciesDictionary<ParticularTimetable, byte[],
                    IDictionaryType<ParticularTimetable, byte[]>>>();
            services.AddSingleton<IDictionaryType<ParticularTimetable, byte[]>, XlsxOutputFormatter>();
            services.AddSingleton<IDictionaryType<ParticularTimetable, byte[]>, PdfOutputFormatter>();
            services.AddSingleton<OutputConverter>();
            services.AddSingleton<OutputProvider>();          //Output
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var firebaseUrl = configuration.GetConnectionString("FirebaseUrl");
            if (firebaseUrl.IsNullOrEmpty())
            {
                var mysqlConnectionString = configuration.GetConnectionString("MySQLConnection");
                if (mysqlConnectionString.IsNullOrEmpty())
                {
                    services.AddSingleton<IDatabaseWrapper<string, DatabaseSlot>, DictionaryWrapper<string, DatabaseSlot>>();
                    services.AddSingleton<IDatabaseWrapper<string, DatabaseTimeslot>, DictionaryWrapper<string, DatabaseTimeslot>>();
                    services.AddSingleton<IDatabaseWrapper<string, DatabaseTimeSchedule>, DictionaryWrapper<string, DatabaseTimeSchedule>>();
                    services.AddSingleton<IDatabaseWrapper<string, DatabaseLessonMinutesDuration>, 
                        DictionaryWrapper<string, DatabaseLessonMinutesDuration>>();
                }
                else
                {
                    services.AddDbContext<MySQLContext<string, DatabaseSlot>>(options  => options
                        .UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));
                    services.AddScoped<IDatabaseWrapper<string, DatabaseSlot>, MySQLWrapper<string, DatabaseSlot>>();
                
                    services.AddDbContext<MySQLContext<string, DatabaseTimeslot>>(options  => options
                        .UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));
                    services.AddScoped<IDatabaseWrapper<string, DatabaseTimeslot>, MySQLWrapper<string, DatabaseTimeslot>>();
                
                    services.AddDbContext<MySQLContext<string, DatabaseTimeSchedule>>(options  => options
                        .UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));
                    services.AddScoped<IDatabaseWrapper<string, DatabaseTimeSchedule>, MySQLWrapper<string, DatabaseTimeSchedule>>();
                    
                    services.AddDbContext<MySQLContext<string, DatabaseLessonMinutesDuration>>(options  => options
                        .UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString)));
                    services.AddScoped<IDatabaseWrapper<string, DatabaseLessonMinutesDuration>, MySQLWrapper<string, DatabaseLessonMinutesDuration>>();
                }
            }
            else
            {
                services.AddSingleton(new FirebaseClient(firebaseUrl));
                services
                    .AddSingleton<IDatabaseWrapper<string, DatabaseSlot>, FirebaseWrapper<string, DatabaseSlot>>();
                services
                    .AddSingleton<IDatabaseWrapper<string, DatabaseTimeslot>,
                        FirebaseWrapper<string, DatabaseTimeslot>>();
                services
                    .AddSingleton<IDatabaseWrapper<string, DatabaseTimeSchedule>,
                        FirebaseWrapper<string, DatabaseTimeSchedule>>();
                services
                    .AddSingleton<IDatabaseWrapper<string, DatabaseLessonMinutesDuration>,
                        FirebaseWrapper<string, DatabaseLessonMinutesDuration>>();
            }
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