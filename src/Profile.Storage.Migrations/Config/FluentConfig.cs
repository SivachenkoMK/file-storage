using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Profile.Storage.Migrations.Config
{
    public class FluentConfig
    {
        public static IServiceProvider ConfigureMigrator(string connection)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb.AddMySql5()
                    .WithGlobalConnectionString(connection)
                    .ScanIn(typeof(FluentConfig).Assembly).For.Migrations())
                .AddLogging(lb =>
                {
                    lb.AddFluentMigratorConsole();
                    lb.AddSerilog(dispose: true);
                })
                .BuildServiceProvider(false);
        }
    }
}