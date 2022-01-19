using System.Collections.Generic;
using Accord.Genetic;
using Castle.Core.Internal;
using Firebase.Database;
using System.IO;
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
            
            //UI
                //Filters
            services
                .AddSingleton<ImplementationSelector<FilterGetterParameters, FilterPartialViewData,
                    IImplementation<FilterGetterParameters, FilterPartialViewData>>>();
            services.AddSingleton<IImplementation<FilterGetterParameters, FilterPartialViewData>, FilterDays>();
            services.AddSingleton<IImplementation<FilterGetterParameters, FilterPartialViewData>, FilterDaysCount>();
                //InputParsers
            services.AddSingleton<ImplementationSelector<Stream, UserInput, IImplementation<Stream, UserInput>>>();
            services.AddSingleton<IImplementation<Stream, UserInput>, XlsxInputParser>();
            services.AddSingleton<IImplementation<Stream, UserInput>, TxtInputParser>();
            services.AddSingleton<InputProvider>();
            
            //Application
                //Layers connection
            services.AddScoped<InputRecipient>();
            services.AddScoped<FiltersPageInterface>();
            services.AddScoped<FilterNamesGetter>();
            services.AddScoped<TimetableResultsInterface>();
            services.AddScoped<TimetableTaskLauncher>();
            services.AddScoped<OutputExecutor>();
                //Converters
            services.AddSingleton<TimespanDbConverter>(); 
            services.AddSingleton<TimeslotDbConverter>(); 
            services.AddSingleton<TimeDurationDbConverter>();
            services.AddSingleton<SlotInfoDbConverter>();

                //Output
            services
                .AddSingleton<ImplementationSelector<ParticularTimetable, byte[],
                    IImplementation<ParticularTimetable, byte[]>>>();
            services.AddSingleton<IImplementation<ParticularTimetable, byte[]>, XlsxOutputFormatter>();
            services.AddSingleton<IImplementation<ParticularTimetable, byte[]>, PdfOutputFormatter>();
            services.AddSingleton<OutputConverter>();

            services.AddSingleton<OutputProvider>();    

            //Domain
            services.AddSingleton<ImplementationSelector<AlgoritmInput, IEnumerable<TimeSlot>,
                IImplementation<AlgoritmInput, IEnumerable<TimeSlot>>>>();
            services.AddSingleton<IImplementation<AlgoritmInput, IEnumerable<TimeSlot>>, GeneticAlgorithm>();
            services.AddSingleton<IImplementation<AlgoritmInput, IEnumerable<TimeSlot>>, GraphAlgorithm>();
            services.AddSingleton<ConverterToAlgorithmInput>();
            services.AddSingleton<FilterHandler>();
            services.AddSingleton<EliteSelection>();
            services.AddSingleton<FitnessFunction>();
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