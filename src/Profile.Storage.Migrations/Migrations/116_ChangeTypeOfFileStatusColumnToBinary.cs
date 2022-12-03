using FluentMigrator;

namespace Profile.Storage.Migrations.Migrations
{
    [Migration(116)]
    public class ChangeTypeOfFileStatusColumnToBinary : Migration
    {
        public override void Up()
        {
            Alter.Table("FileMetadata")
                .AlterColumn("FileStatus").AsBinary();
        }

        public override void Down()
        {
            Delete.Column("FileStatus").FromTable("FileMetadata");
        } 
    }
}