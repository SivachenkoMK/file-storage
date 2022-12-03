using FluentMigrator;

namespace Profile.Storage.Migrations.Migrations
{
    [Migration(101)]
    public class CreateMetadataTable : Migration
    {
        public override void Up()
        {
            Create.Table("FileMetadata")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Name").AsString(300).NotNullable()
                .WithColumn("ContentType").AsAnsiString(150).NotNullable()
                .WithColumn("StorageType").AsByte();
        }

        public override void Down()
        {
            Delete.Table("FileMetadata");
        } 
    }
}