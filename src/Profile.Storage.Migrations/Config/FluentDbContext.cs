using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Serilog;

namespace Profile.Storage.Migrations.Config
{
    public sealed class StorageDbContext : DbContext
    {
        private readonly string _connection;
        
        public StorageDbContext(string connectionStringString)
        {
            _connection = connectionStringString;
        }
        
        public static void InitMigrations(string connection)
        {
            Log.Information("Trying to run migrations over database...");
            using var context = new StorageDbContext(connection);
            context.Database.EnsureCreated();
            DigitalOceanDbHack(context);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connection, ServerVersion.AutoDetect(_connection), options =>
            {
                options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null);
            });
        }

        /// <summary>
        /// VersionInfo table is created without primary key, so current utility even cannot start migration process
        /// In addition to that set sql_require_primary_key=0 for digitalocean won't work.
        /// Reconfiguration of VersionInfo table is not possible that's why I needed to create it from scratch.
        /// </summary>
        /// <param name="context"></param>
        private static void DigitalOceanDbHack(DbContext context)
        {
            Log.Information("Executing command to create/verify if exists VersionInfo table");
            using var connect = (MySqlConnection)context.Database.GetDbConnection();
            connect.Open();
            using var verifyCmd = new MySqlCommand($"SELECT count(*) FROM information_schema.TABLES WHERE (TABLE_SCHEMA = '{connect.Database}') AND (TABLE_NAME = 'VersionInfo')", connect);
            using var vrdReader = verifyCmd.ExecuteReader(CommandBehavior.SingleResult);
            if (vrdReader.Read() && (long)vrdReader[0] == 1)
            {
                Log.Information("VersionInfo table exists, so it will be reused");
                return;
            }
            
            connect.Close();
            connect.Open();
            Log.Information("VersionInfo table doesn't exists, so it will be created");
            using var createCmd = new MySqlCommand("create table VersionInfo (Version bigint not null primary key, AppliedOn datetime null, Description varchar(1024) charset utf8 null)", connect);
            using var rdr = createCmd.ExecuteReader(CommandBehavior.SingleResult);
        }
    }
}