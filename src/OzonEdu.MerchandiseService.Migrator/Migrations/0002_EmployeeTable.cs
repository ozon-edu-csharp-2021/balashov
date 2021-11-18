using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Migrations
{
    [Migration(2)]
    public class EmployeeTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE if not exists employee(
                    id BIGSERIAL PRIMARY KEY,
                    first_name TEXT NOT NULL,
                    last_name TEXT NOT NULL,
                    middle_name TEXT,
                    phone TEXT NOT NULL,
                    email TEXT NOT NULL,
                    clothing_size_id INT NOT NULL,
                    height_id INT);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists employee;");
        }
    }
}