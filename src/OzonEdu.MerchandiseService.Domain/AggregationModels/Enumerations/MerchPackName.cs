using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations
{
    public class MerchPackTitle : Enumeration
    {
        public static MerchPackTitle WelcomePack = new(1, nameof(WelcomePack));
        public static MerchPackTitle StarterPack = new(2, nameof(StarterPack));
        public static MerchPackTitle ConferenceListenerPack = new(3, nameof(ConferenceListenerPack));
        public static MerchPackTitle ConferenceSpeakerPack = new(4, nameof(ConferenceSpeakerPack));
        public static MerchPackTitle VeteranPack = new(5, nameof(VeteranPack));

        public MerchPackTitle(int id, string name) : base(id, name)
        {
        }
    }
}