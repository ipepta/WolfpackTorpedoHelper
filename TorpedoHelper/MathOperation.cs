using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorpedoHelper
{
    public static class MathOperation
    {
        public static double Cos(double input)
        {
            return Math.Cos(input * Math.PI / 180.00);
        }

        public static double Sin(double input)
        {
            return Math.Sin(input * Math.PI / 180.00);
        }

        public static double Tan(double input)
        {
            return Math.Tan(input * Math.PI / 180.00);
        }

        public static double SinH(double input)
        {
            return Math.Sinh(input) * 180.00 / Math.PI;
        }

        public static double CosH(double input)
        {
            return Math.Cosh(input) * 180.00 / Math.PI;
        }

        public static double TanH(double input)
        {
            return Math.Tanh(input) * 180.00 / Math.PI;
        }
    }
}
