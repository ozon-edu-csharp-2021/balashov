using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class MerchPack : ValueObject
    {
        public MerchLine Line { get;  }

        public MerchPackTitle PackTitle { get; private set; }

        public MerchPack(MerchPackTitle packTitle)
        {
            Line = new MerchLine("Ozon-Standart", 2021);
            PackTitle = packTitle;
        }

        public MerchPack(int packId)
        {
            Line = new MerchLine("Ozon-Standart", 2021);

            PackTitle = MerchPackTitle.GetMerchTitleById(packId);
        }

        public MerchPack(MerchLine line, MerchPackTitle packTitle)
        {
            Line = line;
            PackTitle = packTitle;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Line;
            yield return PackTitle;
        }

        public override string ToString()
        {
            return $"Тип набора: {PackTitle.Name}; Серия: {Line}";
        }
    }
}
