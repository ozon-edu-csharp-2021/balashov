using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Migrations
{
    [Migration(3)]
    public class MerchandiseRequestTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE if not exists merchandise_request(
                    id BIGSERIAL PRIMARY KEY,
                    merch_request_status_id INT NOT NULL,
                    hr_manager_id INT NOT NULL,
                    employee_id INT,
                    pack_title_id INT,
                    clothing_size_id INT);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists merchandise_request;");
        }
    }
}