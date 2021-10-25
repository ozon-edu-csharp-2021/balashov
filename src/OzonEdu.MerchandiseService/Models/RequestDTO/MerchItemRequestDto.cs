namespace OzonEdu.MerchandiseService.Models
{
    public class MerchItemRequestDto
    {
        public long Id { get; }
        
        public string ItemName { get; }
        
        public MerchItemRequestDto(long itemId, string itemName)
        {
            Id = itemId;
            ItemName = itemName;
        }
    }
}