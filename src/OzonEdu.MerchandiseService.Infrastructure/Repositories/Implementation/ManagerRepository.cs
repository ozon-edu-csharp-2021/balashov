using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Implementation
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private const int Timeout = 5;

        public ManagerRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<Manager> CreateAsync(Manager itemToCreate, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO hr_managers (first_name, last_name, middle_name, phone, email)
                VALUES (@fname, @lname, @mname, @phone, @email) RETURNING id;";

            var parameters = new
            {
                fname = itemToCreate.Name.FirstName,
                lname = itemToCreate.Name.LastName,
                mname = itemToCreate.Name.MiddleName,
                phone = itemToCreate.PhoneNumber.Phone,
                email = itemToCreate.Email.EmailString
            };

            var commandDefinition = new CommandDefinition(
                commandText: sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var obj = await connection.QueryFirstOrDefaultAsync<long>(commandDefinition);

            if (obj != default)
                itemToCreate.SetId(obj);
            else
            {
                throw new Exception("Запись менеджера в базу провалилась, Id не бы возваращён");
            }

            await CreateManagerTasksAsync(itemToCreate, cancellationToken);

            _changeTracker.Track(itemToCreate);
            return itemToCreate;
        }

        private async Task CreateManagerTasksAsync(Manager itemToCreate, CancellationToken cancellationToken)
        {
            const string sqlTasks = @"                
                INSERT INTO hr_assign_tasks (hr_manager_id, assigned_tasks)
                VALUES (@mrlid, @asstasks);";

            var parametersTasks = new
            {
                mrlid = itemToCreate.Id,
                asstasks = itemToCreate.AssignedTasks
            };

            var commandDefinition = new CommandDefinition(
                commandText: sqlTasks,
                parameters: parametersTasks,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
        }

        const string SqlUpdateHrAssignedTasks = @"
            UPDATE hr_assign_tasks
            SET hr_assign_tasks.assigned_tasks = @asstasks
            WHERE hr_assign_tasks.hr_manager_id = @mrlid;";

        public async Task<Manager> UpdateAsync(Manager itemToUpdate, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            UPDATE hr_managers
            SET hr_managers.first_name = @fname, hr_managers.last_name = @lname, hr_managers.middle_name = @mname,
            hr_managers.phone = @phone, hr_managers.email = @email 
            WHERE hr_managers.id = @mrlid;" + SqlUpdateHrAssignedTasks;
            
            var parameters = new
            {
                mrlid = itemToUpdate.Id,
                fname = itemToUpdate.Name.FirstName,
                lname = itemToUpdate.Name.LastName,
                mname = itemToUpdate.Name.MiddleName,
                phone = itemToUpdate.PhoneNumber.Phone,
                email = itemToUpdate.Email.EmailString,
                asstasks = itemToUpdate.AssignedTasks
            };
            var commandDefinition = new CommandDefinition(
                commandText: sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            await connection.ExecuteAsync(commandDefinition);

            _changeTracker.Track(itemToUpdate);

            return itemToUpdate;
        }

        public async Task<Manager> UpdateAssignedTasksAsync(Manager itemToUpdate, CancellationToken cancellationToken = default)
        {
            const string sql = SqlUpdateHrAssignedTasks;

            var parameters = new
            {
                mrlid = itemToUpdate.Id,
                asstasks = itemToUpdate.AssignedTasks
            };
            var commandDefinition = new CommandDefinition(
                commandText: sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            await connection.ExecuteAsync(commandDefinition);

            _changeTracker.Track(itemToUpdate);

            return itemToUpdate;
        }

        public async Task<Manager> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT hr_managers.id, hr_managers.first_name, hr_managers.last_name,
            hr_managers.middle_name, hr_managers.phone, hr_managers.email,
            hr_assign_tasks.Id, hr_assign_tasks.hr_manager_id, hr_assign_tasks.assigned_tasks
            FROM hr_managers
            LEFT JOIN hr_assign_tasks on hr_assign_tasks.hr_manager_id = hr_managers.id
            WHERE hr_managers.id = @mrlid;";

            var parameters = new { mrlid = id };

            var manager = (await DoRequest_GetManagers(sql, cancellationToken, parameters)).First();

            _changeTracker.Track(manager);

            return manager;
        }

        public async Task<List<Manager>> FindByNameIdAsync(PersonName personName, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT hr_managers.id, hr_managers.first_name, hr_managers.last_name,
            hr_managers.middle_name, hr_managers.phone, hr_managers.email,
            hr_assign_tasks.Id, hr_assign_tasks.hr_manager_id, hr_assign_tasks.assigned_tasks
            FROM hr_managers
            LEFT JOIN hr_assign_tasks on hr_assign_tasks.hr_manager_id = hr_managers.id
            WHERE hr_managers.first_name = @fname AND hr_managers.last_name = @lname;";

            var parameters = new { fname = personName.FirstName, lname = personName.LastName };

            var managerList = (await DoRequest_GetManagers(sql, cancellationToken, parameters)).ToList();

            foreach (var mgr in managerList)
                _changeTracker.Track(mgr);

            return managerList;
        }

        public async Task<List<Manager>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT hr_managers.id, hr_managers.first_name, hr_managers.last_name,
            hr_managers.middle_name, hr_managers.phone, hr_managers.email,
            hr_assign_tasks.Id, hr_assign_tasks.hr_manager_id, hr_assign_tasks.assigned_tasks
            FROM hr_managers
            LEFT JOIN hr_assign_tasks on hr_assign_tasks.hr_manager_id = hr_managers.id;";

            var managerList = (await DoRequest_GetManagers(sql,cancellationToken)).ToList();

            foreach (var mgr in managerList)
                _changeTracker.Track(mgr);

            return managerList;
        }

        private async Task<IEnumerable<Manager>> DoRequest_GetManagers(
            string sql,
            CancellationToken cancellationToken,
            object parameters = null)
        {
            var commandDefinition = new CommandDefinition(
                commandText: sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var managers = await connection.QueryAsync<Models.HrManager, Models.HrAssignTasks, Manager>(
                commandDefinition,
                (dbManager, dbAssignTasks) => new Manager(
                    PersonName.Create(dbManager.FirstName, dbManager.LastName, dbManager.MiddleName),
                    new Email(dbManager.Email),
                    new PhoneNumber(dbManager.Phone),
                    dbAssignTasks.AssignedTasks).SetId(dbManager.Id));
            
            return managers;
        }
    }
}
