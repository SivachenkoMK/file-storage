using System;
using System.IO;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Storage.Migrations.Config;
using Serilog;

namespace Profile.Storage.Migrations
{
    public class Program
    {
        private const string EnvVariableName = "ASPNETCORE_ENVIRONMENT";
        private const string DbConfigKey = "Db";
        
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            
            var connection = configuration.GetConnectionString(DbConfigKey);
            StorageDbContext.InitMigrations(connection);
            
            IServiceProvider provider = FluentConfig.ConfigureMigrator(connection);
            using var scope = provider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        private static IConfiguration GetConfiguration()
        {
            var envName = Environment.GetEnvironmentVariable(EnvVariableName);
            var settingsFileName = !string.IsNullOrEmpty(envName)
                ? $"appsettings.{envName}.json"
                : $"appsettings.Development.json";
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFileName)
                .AddEnvironmentVariables()
                .Build();
            return configuration;
        }
    }
}