using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using mock_json.Data;
using mock_json.Interfaces;
using mock_json.RegistrationExtension;
using mock_json.Service;

namespace mock_json
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
            services.AddControllers();

            services.AddDbContext<DataContext>(opt => opt.UseSqlite("Filename=Data.db"));

            services.AddTransient<IMockService, MockService>();

            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(async (context, next) =>
            {
                var routeRequested = context.Request.Path.Value;
                if (routeRequested.Equals("/") || string.IsNullOrEmpty(routeRequested))
                    context.Request.Path = "/swagger";
                await next();
            });

            
            app.AddSwagger();
            app.UseSwaggerUI(c =>
            {
                c.IndexStream = () => GetType().Assembly
                    .GetManifestResourceStream("index.html"); // requires file to be added as an embedded resource
            });

            UpdateDatabase(app);

            app.UseRouting();

            app.UseAuthorization();

            /// app.UseMiddleware<LoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            using var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.Migrate();
        }
    }
}
