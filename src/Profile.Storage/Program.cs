using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Profile.Storage
{
    public class Program
    {
        private const string EnvVariableName = "ASPNETCORE_ENVIRONMENT";
        
        public static void Main(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable(EnvVariableName);
            var settingsFileName = !string.IsNullOrEmpty(envName)
                ? $"appsettings.{envName}.json"
                : $"appsettings.Development.json";
            
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFileName)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseSerilog();
    }
}