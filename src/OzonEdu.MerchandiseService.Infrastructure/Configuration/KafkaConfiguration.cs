namespace OzonEdu.MerchandiseService.Infrastructure.Configuration
{
    public class KafkaConfiguration
    {
        public string GroupId { get; set; }
        
        public string EmailTopic { get; set; }
        
        public string EmployeeNotificationTopic { get; set; }

        public string StockTopic { get; set; }
        
        public string BootstrapServers { get; set; }
    }
}