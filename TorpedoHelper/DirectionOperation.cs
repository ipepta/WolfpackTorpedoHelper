using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorpedoHelper
{
    public static class DirectionOperation
    {
        public static double CalculateSubActualDirection(double subHeading, double targetDirection)
        {
            //double result;
            //if(targetDirection > 180)
            //{
            //    result = subHeading - (360 - targetDirection);
            //}
            //else
            //{
            //    result = subHeading + targetDirection;
            //}

            var result = subHeading + targetDirection;

            result = CorrectDirection(result);

            return result;
        }

        public static double CorrectDirection(double input)
        {
            if (input > 360)
            {
                return input - 360;
            }
            else if (input < 0)
            {
                return 360 + input;
            }
            else
            {
                return input;
            }
        }
       
    }
}
