using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces;


namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private const int Timeout = 5;

        public EmployeeRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<Employee> CreateAsync(Employee itemToCreate, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO employees (first_name, last_name, middle_name, email)
                VALUES (@fname, @lname, @mname, @email) RETURNING id;";

            var parameters = new
            {
                fname = itemToCreate.Name.FirstName,
                lname = itemToCreate.Name.LastName,
                mname = itemToCreate.Name.MiddleName,
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
                throw new Exception("Запись работника в базу провалилась, Id не бы возваращён");
            }

            _changeTracker.Track(itemToCreate);
            return itemToCreate;
        }

        public async Task<Employee> UpdateAsync(Employee itemToUpdate, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            UPDATE employees
            SET first_name = @fname, last_name = @lname, middle_name = @mname,
            email = @email 
            WHERE id = @eid;";

            var parameters = new
            {
                eid = itemToUpdate.Id,
                fname = itemToUpdate.Name.FirstName,
                lname = itemToUpdate.Name.LastName,
                mname = itemToUpdate.Name.MiddleName,
                email = itemToUpdate.Email.EmailString,
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

        public async Task<Employee> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT * FROM employees WHERE employees.id = @emplid;";

            var parameters = new { emplid = id};

            var employees = await DoRequest_GetEmployees(sql, cancellationToken, parameters);
            if (employees.Count == 0)
                return null;

            var employee = employees.First();
            _changeTracker.Track(employee);

            return employee;
        }

        public async Task<List<Employee>> FindByNameIdAsync(PersonName personName, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT * FROM employees WHERE employees.first_name = @fname AND employees.last_name = @lname;";

            var parameters = new { fname = personName.FirstName, lname = personName.LastName };

            var employeeList = (await DoRequest_GetEmployees(sql, cancellationToken, parameters)).ToList();

            foreach (var empl in employeeList)
                _changeTracker.Track(empl);

            return employeeList;
        }

        public async Task<Employee> FindByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT * FROM employees WHERE employees.email = @email;";

            var parameters = new { email = email.EmailString };

            var employeeList = (await DoRequest_GetEmployees(sql, cancellationToken, parameters)).ToList();
            if (employeeList.Count == 0)
                return null;
            
            if (employeeList.Count > 1)
                throw new Exception("Обнаружено несколько сотрудников с одинаковым Email");

            var employee = employeeList.First();
            _changeTracker.Track(employee);

            return employee;
        }

        public async Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT * FROM employees;";

            var employeeList = (await DoRequest_GetEmployees(sql, cancellationToken)).ToList();

            // Добавление после успешно выполненной операции.
            foreach (var empl in employeeList)
                _changeTracker.Track(empl);

            return employeeList;
        }

        private async Task<List<Employee>> DoRequest_GetEmployees(
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

            var dbElements = await connection.QueryAsync<Models.Employee>(commandDefinition);

            var employees = dbElements.Select(
                dbEmployee => new Employee(
                    PersonName.Create(dbEmployee.FirstName, dbEmployee.LastName, dbEmployee.MiddleName),
                    new Email(dbEmployee.Email))
                    .SetId(dbEmployee.Id)).ToList();

            return employees;
        }

    }
}

