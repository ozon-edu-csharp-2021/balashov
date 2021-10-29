namespace OzonEdu.MerchandiseService.Models
{
    public class MerchItemModel
    {
        public long Id { get; }
        
        public string ItemName { get; }
        
        public MerchItemModel(long itemId, string itemName)
        {
            Id = itemId;
            ItemName = itemName;
        }
    }
}