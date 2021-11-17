using System;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations
{
    public class MerchPackTitle : Enumeration
    {
        public static MerchPackTitle WelcomePack = new(10, nameof(WelcomePack));
        public static MerchPackTitle StarterPack = new(20, nameof(StarterPack));
        public static MerchPackTitle ConferenceListenerPack = new(30, nameof(ConferenceListenerPack));
        public static MerchPackTitle ConferenceSpeakerPack = new(40, nameof(ConferenceSpeakerPack));
        public static MerchPackTitle VeteranPack = new(50, nameof(VeteranPack));

        public MerchPackTitle(int id, string name) : base(id, name)
        {
        }

        public static MerchPackTitle GetMerchTitleById(int packId)
        {
            if (MerchPackTitle.StarterPack.Id == packId)
                return MerchPackTitle.StarterPack;
            if (MerchPackTitle.WelcomePack.Id == packId)
                return MerchPackTitle.WelcomePack;
            if (MerchPackTitle.VeteranPack.Id == packId)
                return MerchPackTitle.VeteranPack;
            if (MerchPackTitle.ConferenceListenerPack.Id == packId)
                return MerchPackTitle.ConferenceListenerPack;
            if (MerchPackTitle.ConferenceSpeakerPack.Id == packId)
                return MerchPackTitle.ConferenceSpeakerPack;

            throw new ArgumentException("unknown MerchPackTitle id!");
        }
    }
}