namespace OzonEdu.MerchandiseService.Models
{
    public class MerchItemRequestDto
    {
        public string ItemName { get; }
        
        public MerchItemRequestDto(string itemName)
        {
            ItemName = itemName;
        }
    }
}