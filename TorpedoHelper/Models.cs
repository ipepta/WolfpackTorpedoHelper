using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TorpedoHelper
{
    public class PointModel
    {
        public double x;
        public double y;

        public PointModel()
        { }

        public PointModel(double x, double y)
        {
            this.x = Math.Round(x, 2);
            this.y = Math.Round(y, 2);
        }

        public double DistanceToZeroPoint
        {
            get
            {
                return Math.Round(Math.Sqrt(x * x + y * y), 2);
            }
        }

        public override string ToString()
        {
            return "(" + this.x + "," + this.y + ")";
        }

        public static PointModel Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            try
            {
                input = input.Replace("(", "").Replace(")", "").Replace(" ", "");
                var x = Convert.ToDouble(input.Split(',')[0]);
                var y = Convert.ToDouble(input.Split(',')[1]);
                return new PointModel(x, y);
            }
            catch
            {
                return null;
            }
        }
    }

    //y = ax + b
    public class LineModel
    {
        public double a;
        public double b;
        public LineDirection lineDirection;

        public LineModel()
        { }

        public LineModel(double a, double b, LineDirection lineDirection)
        {
            this.a = Math.Round(a, 2);
            this.b = Math.Round(b, 2);
            this.lineDirection = lineDirection;
        }

        public double InterceptX
        {
            get
            {
                return b / a * -1.00;
            }
        }

        public double InterceptY
        {
            get
            {
                return b;
            }
        }

        public PointModel InterceptPointX
        {
            get
            {
                return new PointModel(InterceptX, 0);
            }
        }

        public PointModel InterceptPointY
        {
            get
            {
                return new PointModel(0, InterceptY);
            }
        }

        public double CalculateY (double x)
        {
            return a * x + b;
        }

        public double CalculateX(double y)
        {
            return (y - b) / a;
        }

        public override string ToString()
        {
            return "y = " + a + "x + " + b;
        }
    }

    public class AttackParameterModel
    {
        public PointModel attackPoint;
        public double AOB;

        public AttackParameterModel()
        { }

        public AttackParameterModel(PointModel attackPoint, double AOB)
        {
            this.attackPoint = attackPoint;
            this.AOB = AOB;
        }

        public double Range
        {
            get
            {
                return attackPoint.DistanceToZeroPoint;
            }
        }
    }

    public class ChartEntityModel
    {
        public PointModel headPoint;
        public PointModel bottomLeftPoint;
        public PointModel bottomRightPoint;

        public LineModel centerLine;
        public LineModel bottomLine;

        public ChartEntityModel()
        {  }

        public ChartEntityModel(PointModel headPoint, PointModel bottomLeftPoint, PointModel bottomRightPoint, LineModel centerLine, LineModel bottomLine)
        {
            this.headPoint = headPoint;
            this.bottomLeftPoint = bottomLeftPoint;
            this.bottomRightPoint = bottomRightPoint;

            this.centerLine = centerLine;
            this.bottomLine = bottomLine;
        }
    }

    public enum LineDirection
    {
        LEFT_TO_RIGHT,
        RIGHT_TO_LEFT
    }
}
