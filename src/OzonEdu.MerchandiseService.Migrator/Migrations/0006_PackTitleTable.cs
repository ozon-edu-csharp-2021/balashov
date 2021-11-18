using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Temp
{
    [Migration(6)]
    public class PackTitle : Migration
    {
        public override void Up()
        {
            Create
                .Table("pack_title")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("pack_title");
        }
    }
}