using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals
{
    public class EmployeeServer : IEmployeeServer
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public async Task<Employee> GetByIdAsync(long employeeId, CancellationToken cancellationToken)
        {
            var employees = await GetAll(cancellationToken);
            if (employees == null) return null;

            return employees.FirstOrDefault(e => e.Id == employeeId);
        }

        public async Task<List<Employee>> GetAll(CancellationToken cancellationToken)
        {
            //TODO заменить URL на проверенный, передать конфигурацию, чтобы URL можно было настраивать
            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://employees-service:8443/api/employees/getall");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var employees = await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>(responseStream, cancellationToken: cancellationToken);
                if (employees != null) 
                    return employees.ToList();
            }

            return null;
        }
    }
}
