using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(3)]
    public class MerchandiseRequestsTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE if not exists merchandise_requests(
                    id BIGSERIAL PRIMARY KEY,
                    merch_request_status_id INT NOT NULL,
                    hr_manager_id INT NOT NULL,
                    employee_email TEXT NOT NULL,
                    pack_title_id INT,
                    clothing_size_id INT,
                    last_change_date timestamp without time zone);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists merchandise_requests;");
        }
    }
}