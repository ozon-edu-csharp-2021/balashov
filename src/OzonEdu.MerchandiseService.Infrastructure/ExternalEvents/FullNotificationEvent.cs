using CSharpCourse.Core.Lib.Events;

namespace OzonEdu.MerchandiseService.Infrastructure.ExternalEvents
{
    public class FullNotificationEvent : NotificationEvent
    {
        public Payload Payload { get; set; }
        //public int MerchType { get; set; }

        //public int ClothingSize { get; set; }
    }
    public class Payload
    {
        public int MerchType { get; set; }

        public int ClothingSize { get; set; }
    }
}
