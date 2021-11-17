namespace OzonEdu.MerchandiseService.Models
{
    public class MerchandiseRequestRequestDto
    {
        public long HRManagerId { get; set; }

        public int RequestedMerchPackType { get; set; }

        public string Size { get; set; }
    }
}