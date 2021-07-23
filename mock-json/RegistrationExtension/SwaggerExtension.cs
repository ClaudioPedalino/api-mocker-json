using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace mock_json.RegistrationExtension
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "mock json data",
                        Version = "v1",
                        Description = "A easy mocker json api",
                        TermsOfService = new Uri("https://github.com/ClaudioPedalino"),
                        Contact = new OpenApiContact
                        {
                            Name = "Claudio Pedalino",
                            Email = "https://www.linkedin.com/in/claudio-pedalino/"
                        },
                    });
                //c.TagActionsBy(api => api.HttpMethod);
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                c.CustomSchemaIds((type) => type.FullName);

                var filePath = Path.Combine(AppContext.BaseDirectory, "apidocumentation.xml");
                c.IncludeXmlComments(filePath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
            //services.AddApiExplorer();

            return services;
        }

        public static IApplicationBuilder AddSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            Console.WriteLine(Environment.CurrentDirectory + "/swagger-custom-style.css");
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "cpedalino mock json v1");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "cpedalino mock json v2");
                c.InjectStylesheet($"{Environment.CurrentDirectory}/swagger-custom-style.css");
                //c.EnableAnnotations();
                //c.DefaultModelExpandDepth(2);
                //c.DefaultModelRendering(ModelRendering.Model);
                //c.DefaultModelsExpandDepth(-1);
                //c.DisplayOperationId();
                //c.DisplayRequestDuration();
                //c.DocExpansion(DocExpansion.None);
                //c.EnableDeepLinking();
                //c.EnableFilter();  //// INVEstigar
                //c.MaxDisplayedTags(5);
                //c.ShowExtensions();
                //c.ShowCommonExtensions();
                //c.EnableValidator();
                //c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head);

                //c.UseRequestInterceptor("(request) => { return request; }");
                //c.UseResponseInterceptor("(response) => { return response; }");
            });

            return app;
        }
    }
}