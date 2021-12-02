using System;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals
{
    public class EmailServer : IEmailServer
    {
        public Task<bool> SendEmailAboutMerchAsync(long employeeId, string text)
        {
            throw new NotImplementedException();
            //TODO реализовать обращение с Емайлом через кафку
        }
    }
}
