using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals
{
    public class EmployeeServer : IEmployeeServer
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string ServerUrl;

        public EmployeeServer(IHttpClientFactory clientFactory, IOptions<ExternalConnectionOptions> externalsOptions)
        {
            _clientFactory = clientFactory;
            ServerUrl = externalsOptions.Value.EmployeeServerUrl;

            if(ServerUrl.Last() == '/')
                ServerUrl = ServerUrl.Remove(ServerUrl.Length - 1);
        }

        public async Task<List<Employee>> GetAll(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //TODO проверить и поправить EmployeeServer.GetAll, хотя в бизнес логике он не нужен теперь

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{ServerUrl}/api/employees/getall");

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

        public async Task<Employee> GetByIdAsync(long employeeId, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient();
            
            var response = await client.GetStringAsync($"{ServerUrl}/api/employees/{employeeId.ToString()}",
                cancellationToken);

            if (string.IsNullOrWhiteSpace(response))
                return null;

            //todo что-то не работет System.Text.Json.JsonSerializer, нужно разобраться..
            //var employeeDto = JsonSerializer.Deserialize<Repositories.Models.Employee>(response);

            var employeeDto = JsonConvert.DeserializeObject<Repositories.Models.Employee>(response);

            if (employeeDto != null)
                return new Employee(
                    PersonName.Create(employeeDto.FirstName, employeeDto.LastName, employeeDto.MiddleName),
                    new Email(employeeDto.Email)).SetId(employeeDto.Id);
                    
            return null;
        }
    }
}
