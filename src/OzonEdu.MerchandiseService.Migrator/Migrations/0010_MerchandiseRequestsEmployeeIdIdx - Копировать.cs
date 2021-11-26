using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(10, TransactionBehavior.None)]
    public class MerchandiseRequestsEmployeeIdIdx: ForwardOnlyMigration 
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE INDEX CONCURRENTLY merchandise_requests_employee_id_idx 
                ON merchandise_requests (employee_id);"
            );
        }
    }
}