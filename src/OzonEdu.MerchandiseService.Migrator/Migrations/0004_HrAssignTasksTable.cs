using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(4)]
    public class HrAssignTasksTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE if not exists hr_assign_tasks(
                    id BIGSERIAL PRIMARY KEY,
                    hr_manager_id INT NOT NULL,
                    assigned_tasks INT NOT NULL);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists hr_assign_tasks;");
        }
    }
}