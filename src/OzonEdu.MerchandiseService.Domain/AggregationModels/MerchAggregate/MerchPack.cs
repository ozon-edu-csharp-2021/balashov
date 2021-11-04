using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    class MerchPack : Entity
    {
        public List<Merch> MerchList { get; set; }

        public string MerchPackName { get; set; }

        public int MerchLine { get; set; }

        public int Size { get; set; }

    }
}
