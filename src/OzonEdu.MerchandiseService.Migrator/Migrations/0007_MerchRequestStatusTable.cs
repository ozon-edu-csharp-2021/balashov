using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Temp
{
    [Migration(7)]
    public class MerchRequestStatus: Migration
    {
        public override void Up()
        {
            Create
                .Table("merch_request_status")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("merch_request_status");
        }
    }
}