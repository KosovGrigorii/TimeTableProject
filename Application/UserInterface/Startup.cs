using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimetableApplication;
using TimetableDomain;

namespace UserInterface
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();
            services.AddScoped<IInputParser, XlsxInputParser>();
            services.AddScoped<ITimetableMaker, GeneticAlgorithm>();
            services.AddScoped<OutputFormatter, XlsxOutputFormatter>();
            services.AddSingleton<IUserData, UserToData>();
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
            
            app.Run(async x =>
            {
                x.Response.StatusCode = 200;
                // await x.Response.WriteAsync("Hello, World!");
                await x.Response.WriteAsync(x.Request.QueryString.Value);
            });
        }
    }
}