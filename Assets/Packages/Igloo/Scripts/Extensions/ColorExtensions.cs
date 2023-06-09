using UnityEngine;

namespace Packages.Igloo.Scripts.Extensions
{
    public static class ColorExtensions
    {
        public static Color A(this Color color, float a)
        {
            color.a = a;
            return color;
        }

        public static Color R(this Color color, float r)
        {
            color.r = r;
            return color;
        }

        public static Color G(this Color color, float g)
        {
            color.g = g;
            return color;
        }

        public static Color B(this Color color, float b)
        {
            color.b = b;
            return color;
        }
    }
}