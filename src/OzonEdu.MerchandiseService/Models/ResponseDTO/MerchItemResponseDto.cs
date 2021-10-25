namespace OzonEdu.MerchandiseService.Models
{
    public class MerchItemResponseDto
    {
        public long Id { get; }
        
        public string ItemName { get; }
        
        public MerchItemResponseDto(long itemId, string itemName)
        {
            Id = itemId;
            ItemName = itemName;
        }
    }
}