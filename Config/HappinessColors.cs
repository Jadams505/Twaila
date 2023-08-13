using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twaila.Config
{
    public class HappinessColors
    {
        public ColorWrapper LoveColor;

        public ColorWrapper LikeColor;

        public ColorWrapper DislikeColor;

        public ColorWrapper HateColor;

        public HappinessColors()
        {
            LoveColor = new(73, 215, 43, 255);
            LikeColor = new(220, 215, 31, 255);
            DislikeColor = new(224, 111, 28, 255);
            HateColor = new(232, 56, 31, 255);
        }

        public override bool Equals(object obj)
        {
            return obj is HappinessColors other &&
                   LoveColor == other.LoveColor &&
                   LikeColor == other.LikeColor &&
                   DislikeColor == other.DislikeColor &&
                   HateColor == other.HateColor;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LoveColor, LikeColor, DislikeColor, HateColor);
        }
    }
}
