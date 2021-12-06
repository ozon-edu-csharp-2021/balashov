using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.ExternalDataSources;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Commands
{
    public class EmployeeDetectedCommandHandler : IRequestHandler<EmployeeDetectedCommand, Employee>
    {
        private readonly ILogger<EmployeeDetectedCommandHandler> _logger;
        private readonly IEmployeeServer _employeeServer;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeDetectedCommandHandler(
            ILogger<EmployeeDetectedCommandHandler> logger, 
            IEmployeeServer employeeServer, 
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _employeeServer = employeeServer;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Employee> Handle(EmployeeDetectedCommand request, CancellationToken cancellationToken)
        {
            //await _unitOfWork.StartTransaction(cancellationToken);
            //Так как уже реализовал класс, который ходит в HTTP-API Employee-service, то пусть тут и ходит,
            //чтобы в swaggere/postman было бы поприятнее и как демонстрация технологии
            //Но, так-то данные про Employee должны подаваться на вход полностью и явно (например из админки)
            var employee = request.TheEmployee;

            if (request.TheEmployee is null)
                employee = await GetFromEmployeeServerById(request.EmployeeId, cancellationToken);

            if (employee is null)
                throw new Exception("Некорректные входные данные про работника");

            var dbEmployee = await InsertOrUpdateToRepository(employee, cancellationToken);

            //await _unitOfWork.SaveChangesAsync(cancellationToken);

            return dbEmployee;
        }

        private async Task<Employee> GetFromEmployeeServerById(long employId, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Извлекаются данные о сотруднике с ID {id} из EmployeeService", employId);
                return await _employeeServer.GetByIdAsync(employId, cancellationToken);
            }

        private async Task<Employee> InsertOrUpdateToRepository(Employee employee, CancellationToken cancellationToken)
        {
            var dbEmploy = await _employeeRepository.FindByEmailAsync(employee.Email, cancellationToken);

            if (dbEmploy is null)
            {
                _logger.LogInformation("Добавляется новый сотрудник в местный репозиторий {@elployee}", employee);
                dbEmploy = await _employeeRepository.CreateAsync(employee, cancellationToken);
            }
            else
            {
                employee.SetId(dbEmploy.Id);
                _logger.LogInformation("Обновляется сотрудник в местном репозитории {@elployee}", employee);
                dbEmploy = await _employeeRepository.UpdateAsync(employee, cancellationToken);
            }

            return dbEmploy;

        }
    }
}
