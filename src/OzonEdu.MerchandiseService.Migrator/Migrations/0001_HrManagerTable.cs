using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Migrations
{
    [Migration(1)]
    public class HrManagerTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE if not exists hr_manager(
                    id BIGSERIAL PRIMARY KEY,
                    first_name TEXT NOT NULL,
                    last_name TEXT NOT NULL,
                    middle_name TEXT,
                    phone TEXT NOT NULL,
                    email TEXT NOT NULL);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists hr_manager;");
        }
    }
}