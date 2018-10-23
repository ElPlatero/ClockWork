using Clockwork.Lib.Calculators;
using Clockwork.Lib.Repositories;
using ClockWork.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ClockWork.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<LiteDbOptions>(options => options.DatabaseFile = "clockwork.db");
            services.TryAddSingleton<IClockWorkRepository, LiteClockWorkRepository>();
            services.TryAddSingleton<IEffectiveWorkingTimeCalculator, StsWorkCalculator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var options = app.ApplicationServices.GetService<IOptions<LiteDbOptions>>();
            options.Value.DatabaseFile = $"{env.ContentRootPath}\\{options.Value.DatabaseFile}";


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
