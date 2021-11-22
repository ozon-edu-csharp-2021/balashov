using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(11)]
    public class AddSomeFakeData: ForwardOnlyMigration 
    {
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO employees (first_name, last_name, middle_name, phone, email)
                VALUES ('ИмяТест1', 'ФамилияТест1', 'ОтчествоТест1', '+123456789', 'test@email.emp') ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO employees (first_name, last_name, middle_name, phone, email)
                VALUES ('ИмяТест2', 'ФамилияТест2', 'ОтчествоТест2', '+222456789', 'test2@email.emp') ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO employees (first_name, last_name, middle_name, phone, email)
                VALUES ('ИмяТест3', 'ФамилияТест3', '', '+123456999', 'test3@email.emp') ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO hr_managers (first_name, last_name, middle_name, phone, email)
                VALUES ('МИмяТест1', 'МФамилияТест1', 'МОтчествоТест1', '+123456789', 'mtest@email.emp') ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO hr_managers (first_name, last_name, middle_name, phone, email)
                VALUES ('МИмяТест2', 'МФамилияТест2', 'МОтчествоТест2', '+123456789', 'mtest2@email.emp') ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO hr_managers (first_name, last_name, middle_name, phone, email)
                VALUES ('МИмяТест3', 'МФамилияТест3', '', '+123456789', 'mtest3@email.emp') ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO hr_assign_tasks (hr_manager_id, assigned_tasks)
                VALUES (1, 1) ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO hr_assign_tasks (hr_manager_id, assigned_tasks)
                VALUES (2, 0) ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO hr_assign_tasks (hr_manager_id, assigned_tasks)
                VALUES (3, 4) ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
INSERT INTO merchandise_requests (merch_request_status_id, hr_manager_id, employee_id, pack_title_id, clothing_size_id, last_change_date)
                VALUES (2, 1, 2, 10, 1, '2021-10-20') ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
INSERT INTO merchandise_requests (merch_request_status_id, hr_manager_id, employee_id, pack_title_id, clothing_size_id, last_change_date)
                VALUES (4, 2, 3, 20, 3, '2018-11-20')  ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
INSERT INTO merchandise_requests (merch_request_status_id, hr_manager_id, employee_id, pack_title_id, clothing_size_id, last_change_date)
                VALUES (5, 3, 1, 40, 5, '2020-11-20')  ON CONFLICT DO NOTHING
            ");
        }
    }
}