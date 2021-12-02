using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals
{
    public class ExternalConnectionOptions
    {
        public string EmployeeServerUrl { get; set; }

        public string EmailServerUrl { get; set; }

        public string StockApiServerUrl { get; set; }

    }
}
