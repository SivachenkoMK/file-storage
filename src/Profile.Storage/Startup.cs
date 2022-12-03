using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Profile.Storage.Application;
using Profile.Storage.Domain.Exceptions;
using Profile.Storage.Persistence.Configs;
using Serilog;
using Stripe;

namespace Profile.Storage
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            StripeConfiguration.ApiKey =
                "sk_test_51KdZlqFdlWuHlakgsoMm1VB4rbUCJcYTqAbq8Fgm89YeToK6WriGdg6Q2Av1ANf1KQWimUhRCb8RDhqUdz0LDNaJ00sqmE0wyA";
            _configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container. Dependency Injection -> DI container -> google.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer();

            services.Configure<StorageConfig>(_configuration.GetSection(nameof(StorageConfig)));
            var connectionStringSection = _configuration.GetSection(nameof(DbConfig));
            
            services.Configure<DbConfig>(connectionStringSection);
            services.Configure<S3Config>(_configuration.GetSection(nameof(S3Config)));
            services.Configure<BackgroundDeletionConfig>(_configuration.GetSection(nameof(BackgroundDeletionConfig)));
            services.Configure<SwaggerGenConfig>(_configuration.GetSection(nameof(SwaggerGenConfig)));
            
            services.RegisterStorageServices(connectionStringSection.Value);

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://integration.profile-me.io";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "app.internal");
                });
            });
            
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                });
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Storage API - v1", Version = "v1" });

                    var swaggerGenConfig = _configuration.GetSection(nameof(SwaggerGenConfig)).Get<SwaggerGenConfig>();
                    
                    var files = Directory.GetFiles(swaggerGenConfig.FilePath, swaggerGenConfig.XmlPattern);
                    foreach (var file in files)
                    {
                        c.IncludeXmlComments(file);
                    }
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error is FileMetadataNotFoundByIdException 
                        || exceptionHandlerPathFeature?.Error is FileNotFoundByNameException)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message);
                    }
                });
            });

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "STORAGE API V1");
            });

            app.UseReDoc(c =>
            {
                c.RoutePrefix = "storage/api";
                c.SpecUrl = "/swagger/v1/swagger.json";
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}