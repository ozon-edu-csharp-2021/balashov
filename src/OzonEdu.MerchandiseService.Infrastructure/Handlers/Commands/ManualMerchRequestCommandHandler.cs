using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Infrastructure.Commands;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Commands
{
    class ManualMerchRequestCommandHandler : IRequestHandler<ManualMerchRequestCommand, MerchandiseRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public ManualMerchRequestCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<MerchandiseRequest> Handle(ManualMerchRequestCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);

            var employeeCommand = new EmployeeDetectedCommand { EmployeeId = request.EmployeeId };
            var localEmployee = await _mediator.Send(employeeCommand, cancellationToken);

            var mediatrCommand = new RequestMerchCommand()
            {
                HRManagerId = request.HRManagerId,
                EmployeeEmail = localEmployee.Email,
                RequestedMerchPack = request.RequestedMerchPack,
                Size = request.Size,
                Date = request.Date
            };

            var merchRequest =  await _mediator.Send(mediatrCommand, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return merchRequest;
        }
    }
}
