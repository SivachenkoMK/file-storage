using FluentMigrator;

namespace Profile.Storage.Migrations.Migrations
{
    [Migration(106)]
    public class AddMetadataDateLoaded : Migration
    {
        public override void Up()
        {
            Alter.Table("FileMetadata")
                .AddColumn("DateLoaded").AsDateTime();
        }

        public override void Down()
        {
            Delete.Column("DateLoaded").FromTable("FileMetadata");
        } 
    }
}