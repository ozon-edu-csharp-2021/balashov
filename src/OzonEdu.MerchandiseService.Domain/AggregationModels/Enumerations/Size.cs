using System;
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

        public static Size GetSizeFromString(string sizeString)
        {
            var uperSizeString = sizeString.ToUpper();

            if (XS.Name == uperSizeString)
                return XS;
            if (S.Name == uperSizeString)
                return S;
            if (M.Name == uperSizeString)
                return M;
            if (L.Name == uperSizeString)
                return L;
            if (XL.Name == uperSizeString)
                return XL;
            if (XXL.Name == uperSizeString)
                return XXL;

            throw new ArgumentException("Unknown Size name!");
        }

        public static Size GetById(int id)
        {
            if (XS.Id == id)
                return XS;
            if (S.Id == id)
                return S;
            if (M.Id == id)
                return M;
            if (L.Id == id)
                return L;
            if (XL.Id == id)
                return XL;
            if (XXL.Id == id)
                return XXL;

            throw new ArgumentException("Unknown Size name!");
        }
    }
}