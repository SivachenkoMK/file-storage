using FluentMigrator;

namespace Profile.Storage.Migrations.Migrations
{
    [Migration(126)]
    public class AlterFileStatusAsByte : Migration
    {
        public override void Up()
        {
            Alter.Column("FileStatus").OnTable("FileMetadata").AsByte();
        }

        public override void Down()
        {
            Delete.Column("FileStatus").FromTable("FileMetadata");
        } 
    }
}