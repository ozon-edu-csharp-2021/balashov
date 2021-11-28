namespace OzonEdu.MerchandiseService.Models
{
    public class MerchandiseRequestRequestDto
    {
        public long EmployeeId { get; set; }

        public long HRManagerId { get; set; }

        public int RequestedMerchPackType { get; set; }

        public string Size { get; set; }
    }
}