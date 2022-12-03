using FluentMigrator;

namespace Profile.Storage.Migrations.Migrations
{
    [Migration(111)]
    public class AddMetadataFileExistanceColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("FileMetadata")
                .AddColumn("FileStatus").AsDateTime();
        }

        public override void Down()
        {
            Delete.Column("FileStatus").FromTable("FileMetadata");
        } 
    }
}