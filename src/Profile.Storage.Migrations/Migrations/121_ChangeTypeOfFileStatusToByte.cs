using FluentMigrator;

namespace Profile.Storage.Migrations.Migrations
{
    [Migration(121)]
    public class ChangeTypeOfFileStatusToByte : Migration
    {
        public override void Up()
        {
            Alter.Table("FileMetadata")
                .AlterColumn("FileStatus").AsByte();
        }

        public override void Down()
        {
            Delete.Column("FileStatus").FromTable("FileMetadata");
        } 
    }
} 