﻿using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternal.FakeExternals
{
    public class FakeStockApiServer : IStockApiServer
    {
        public bool IsMerchAmountEnough(MerchandiseRequest merchRequest)
        {
            Console.Write("Понарошку проверяю наличие мерча на выдуманном складе");
            return true;
        }

        public bool ReserveMerch(MerchandiseRequest merchRequest)
        {
            Console.Write("Понарошку резервирую мерч при его наличии на выдуманном складе");
            return true;
        }
    }
}
