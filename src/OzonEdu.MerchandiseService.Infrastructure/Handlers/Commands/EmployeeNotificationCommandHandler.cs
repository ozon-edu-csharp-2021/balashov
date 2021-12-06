using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Commands
{
    public class EmployeeNotificationCommandHandler : IRequestHandler<EmployeeNotificationCommand, MerchandiseRequest>
    {
        private readonly ILogger<EmployeeNotificationCommandHandler> _logger;

        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;


        public EmployeeNotificationCommandHandler(ILogger<EmployeeNotificationCommandHandler> logger, IMediator mediator, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mediator = mediator;
            _unitOfWork = unitOfWork;

        }

        public async Task<MerchandiseRequest> Handle(EmployeeNotificationCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.StartTransaction(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.StartTransaction(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var name = PersonName.CreateFromOneString(request.EmployeeNotificationEvent.EmployeeName);
            var employee = new Employee(name, new Email(request.EmployeeNotificationEvent.EmployeeEmail));

            var employeeCommand = new EmployeeDetectedCommand{TheEmployee = employee};
            employee = await _mediator.Send(employeeCommand, cancellationToken);
            

            var merchTitle = new MerchPack(request.EmployeeNotificationEvent.Payload.MerchType);
            var size = Size.GetById(request.EmployeeNotificationEvent.Payload.ClothingSize);
            var date = new Date(DateTime.Now);
            var mediatrCommand = new RequestMerchCommand
            {
                HRManagerId = 0,
                EmployeeId = employee.Id,
                RequestedMerchPack = merchTitle,
                Size = size,
                Date = date
            };

            var merchRequest = await _mediator.Send(mediatrCommand, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return merchRequest;

        }
    }
}
