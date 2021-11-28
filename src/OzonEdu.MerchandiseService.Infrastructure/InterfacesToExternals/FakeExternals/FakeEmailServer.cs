using System;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals.FakeExternals
{
    public class FakeEmailServer :IEmailServer
    {
        public async Task<bool> SendEmailAboutMerchAsync(long employeeId, string text)
        {
            Console.WriteLine($"Понарошку отправляю письмо пользователю с id:{employeeId}\nПисьмо\n{text}");
            return true;
        }
    }
}
