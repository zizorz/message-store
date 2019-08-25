using System.Collections.Generic;
using AutoMapper;
using MessageStoreApplication.Mapping;
using MessageStoreApplication.Middleware;
using MessageStoreApplication.Services;
using MessageStoreApplication.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace MessageStoreApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "MessageStoreAPI", Version = "v1"});
                c.AddSecurityDefinition("oauth2", new ApiKeyScheme
                {
                    Description = "API-KEY",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", new[] { "readAccess", "writeAccess" } }
                });
            });
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSingleton<IMessageStore, MessageStore>();
            services.AddTransient<IMessageService, MessageService>();
            SetupAutoMapper(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MessageStoreAPI");
            });
            
            app.UseHttpsRedirection();
            app.UseMiddleware<AuthorizationMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMvc();
        }

        private void SetupAutoMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}