using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations
{
    public class MerchRequestStatusType : Enumeration
    {
        public static MerchRequestStatusType Draft = new(1, nameof(Draft));
        public static MerchRequestStatusType Created = new(2, nameof(Created));
        public static MerchRequestStatusType Assigned = new(3, nameof(Assigned));
        public static MerchRequestStatusType InProgress = new(4, nameof(InProgress));
        public static MerchRequestStatusType Done = new(5, nameof(Done));

        public MerchRequestStatusType(int id, string name) : base(id, name)
        {
        }
    }
}
