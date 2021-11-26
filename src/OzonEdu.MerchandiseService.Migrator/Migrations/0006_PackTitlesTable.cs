using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(6)]
    public class PackTitles : Migration
    {
        public override void Up()
        {
            Create
                .Table("pack_titles")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("pack_titles");
        }
    }
}