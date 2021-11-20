using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(7)]
    public class MerchRequestStatusTypes: Migration
    {
        public override void Up()
        {
            Create
                .Table("merch_request_status_types")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("merch_request_status_types");
        }
    }
}