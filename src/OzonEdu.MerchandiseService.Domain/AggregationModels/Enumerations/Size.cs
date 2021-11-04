using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations
{
    public class Size : Enumeration
    {
        public static Size XS = new(1, nameof(XS));
        public static Size S = new(2, nameof(S));
        public static Size M = new(3, nameof(M));
        public static Size L = new(4, nameof(L));
        public static Size XL = new(5, nameof(XL));
        public static Size XXL = new(6, nameof(XXL));

        public Size(int id, string name) : base(id, name)
        {
        }
    }
}