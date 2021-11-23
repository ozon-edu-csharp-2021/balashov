using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Implementation
{
    public class MerchRepository : IMerchRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private const int Timeout = 5;
        public MerchRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<MerchandiseRequest> CreateAsync(MerchandiseRequest itemToCreate, CancellationToken cancellationToken = default)
        {
            string sql = @"
                INSERT INTO merchandise_requests (merch_request_status_id, hr_manager_id, employee_id, pack_title_id, clothing_size_id, last_change_date)
                VALUES (@status, @hrid, @eid, @pack, @size, @lastdate) RETURNING id;";

            var parameters = new
            {
                status = itemToCreate.Status.Status.Id,
                hrid = itemToCreate.HRManagerId,
                eid = itemToCreate.EmployeeId,
                pack = itemToCreate.RequestedMerchPack.PackTitle.Id,
                size = itemToCreate.Size.Id,
                lastdate = itemToCreate.Status.Date.ToDateTime()
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
                throw new Exception("Запись в базу заявки на мерч провалилась, Id не бы возваращён");
            }

            _changeTracker.Track(itemToCreate);
            return itemToCreate;
        }

        public async Task<MerchandiseRequest> UpdateAsync(MerchandiseRequest itemToUpdate, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            UPDATE merchandise_requests
            SET merch_request_status_id = @status, hr_manager_id = @hrid, employee_id = @eid,
            pack_title_id = @pack, clothing_size_id = @size, last_change_date = @lastdate
            WHERE merchandise_requests.id = @mrid;";

            var parameters = new
            {
                mrid = itemToUpdate.Id,
                status = itemToUpdate.Status.Status.Id,
                hrid = itemToUpdate.HRManagerId,
                eid = itemToUpdate.EmployeeId,
                pack = itemToUpdate.RequestedMerchPack.PackTitle.Id,
                size = itemToUpdate.Size.Id,
                lastdate = itemToUpdate.Status.Date.ToDateTime()
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

        public async Task<MerchandiseRequest> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT merchandise_requests.id, merchandise_requests.hr_manager_id, merchandise_requests.employee_id,
            merchandise_requests.clothing_size_id, merchandise_requests.pack_title_id, merchandise_requests.merch_request_status_id,
            employees.id, employees.phone,
            hr_managers.id, hr_managers.phone
            FROM merchandise_requests
			INNER JOIN employees on merchandise_requests.employee_id = employees.id
            INNER JOIN hr_managers on merchandise_requests.hr_manager_id = hr_managers.id
            WHERE merchandise_requests.id = @mrId;";

            var parameters = new { mrId = id };

            var merchRequest = (await DoRequest_GetMerchandiseRequest(sql, cancellationToken, parameters)).First();
                _changeTracker.Track(merchRequest);

            return merchRequest;
        }

        public async Task<List<MerchandiseRequest>> FindByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT merchandise_requests.id, merchandise_requests.hr_manager_id, merchandise_requests.employee_id,
            merchandise_requests.clothing_size_id, merchandise_requests.pack_title_id, merchandise_requests.merch_request_status_id, merchandise_requests.last_change_date,
            employees.id, employees.phone,
            hr_managers.id, hr_managers.phone
            FROM merchandise_requests
			INNER JOIN employees on merchandise_requests.employee_id = employees.id
            INNER JOIN hr_managers on merchandise_requests.hr_manager_id = hr_managers.id
            WHERE merchandise_requests.employee_id = @emplId;";

            var parameters = new { emplId = employeeId };

            var merchRequests = (await DoRequest_GetMerchandiseRequest(sql, cancellationToken, parameters)).ToList();

            return merchRequests.ToList();
        }

        private async Task<IEnumerable<MerchandiseRequest>> DoRequest_GetMerchandiseRequest(
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

            var merchRequests = await connection.QueryAsync<Models.MerchandiseRequest, Models.Employee, Models.HrManager, MerchandiseRequest>(commandDefinition,
                (dbMerchandiseRequest, dbEmployee, dbHrManager) => new MerchandiseRequest(
                        dbMerchandiseRequest.HrManagerId,
                        new PhoneNumber(dbHrManager.Phone),
                        dbMerchandiseRequest.PackTitleId is not null
                            ? new MerchPack((int)dbMerchandiseRequest.PackTitleId)
                            : null,
                        MerchRequestStatusType.GetById(dbMerchandiseRequest.MerchRequestStatusId),
                        new Date(dbMerchandiseRequest.LastChangeDate))
                    .AddEmployeeInfoFromDB(
                        dbMerchandiseRequest.EmployeeId,
                        new PhoneNumber(dbEmployee.Phone),
                        dbMerchandiseRequest.ClothingSizeId is not null
                            ? Size.GetById((int)dbMerchandiseRequest.ClothingSizeId)
                            : null)
                    .SetId(dbMerchandiseRequest.Id));

            return merchRequests;
        }
    }
}
