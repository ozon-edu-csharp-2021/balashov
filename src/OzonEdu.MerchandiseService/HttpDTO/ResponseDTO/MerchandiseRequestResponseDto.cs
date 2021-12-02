using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Models
{
    public class MerchandiseRequestResponseDto
    {
        public long Id { get; }

        public string Status { get; }

        public long HRManagerId { get; }

        public long EmployeeId { get; }

        public string Size { get; }

        public string RequestedMerchPack { get; }


        public MerchandiseRequestResponseDto(MerchandiseRequest issuingMerch)
        {
            Id = issuingMerch.Id;

            Status = issuingMerch.Status.ToString();

            HRManagerId = issuingMerch.HRManagerId;

            EmployeeId = issuingMerch.EmployeeId;

            RequestedMerchPack = issuingMerch.RequestedMerchPack.ToString();

            Size = issuingMerch.Size is null ? "not set" : issuingMerch.Size.Name;

        }
}
}