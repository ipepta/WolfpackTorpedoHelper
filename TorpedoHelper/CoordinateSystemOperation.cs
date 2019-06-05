using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorpedoHelper
{
    public static class CoordinateSystemOperation
    {
        public static ChartEntityModel CalculateEntityPoints(PointModel point, double heading, double height)
        {
            var centerLine = CalculateLineFromPointAndDirection(point, heading);
            LineDirection centerLineHeadDirection, centerLineBottomDirection;
            if(heading< 180)
            {
                centerLineHeadDirection = LineDirection.LEFT_TO_RIGHT;
                centerLineBottomDirection = LineDirection.RIGHT_TO_LEFT;
            }
            else
            {
                centerLineHeadDirection = LineDirection.RIGHT_TO_LEFT;
                centerLineBottomDirection = LineDirection.LEFT_TO_RIGHT;
            }

            var headPoint = CalculatePointByLineAndLength(centerLine, point, height, centerLineHeadDirection);
            var bottomMidPoint = CalculatePointByLineAndLength(centerLine, point, height, centerLineBottomDirection);
            var bottomLineDirection = heading + 90;
            if(bottomLineDirection > 360)
            {
                bottomLineDirection = bottomLineDirection - 360;
            }
            var bottomLine = CalculateLineFromPointAndDirection(bottomMidPoint, bottomLineDirection);
          //  var bottomLine = CalculateLineFromPointAndDirection(point, bottomLineDirection);

            var bottomLeftPoint = CalculatePointByLineAndLength(bottomLine, bottomMidPoint, height/1.5, LineDirection.LEFT_TO_RIGHT);
            var bottomRightPoint = CalculatePointByLineAndLength(bottomLine, bottomMidPoint, height/1.5, LineDirection.RIGHT_TO_LEFT);

            return new ChartEntityModel(headPoint, bottomLeftPoint, bottomRightPoint, centerLine, bottomLine);
        }

        public static PointModel CalculatePointByLineAndLength(LineModel line, PointModel point, double length, LineDirection direction)
        {
            double x2;
            if(direction == LineDirection.LEFT_TO_RIGHT)
            {
                x2 = point.x + length / Math.Sqrt(1 + line.a * line.a);
            }
            else
            {
                x2 = point.x - length / Math.Sqrt(1 + line.a * line.a);
            }
            double y2 = line.CalculateY(x2);

            PointModel result = new PointModel(x2, y2);
            return result;
        }

        public static LineModel CalculateLineFromTwoPoint(PointModel earlyPoint, PointModel laterPoint)
        {
            double a = (earlyPoint.y - laterPoint.y) / (earlyPoint.x - laterPoint.x);
            double b = earlyPoint.y - a * earlyPoint.x;

            LineDirection lineDirection;
            if (earlyPoint.x > laterPoint.x)
            {
                lineDirection = LineDirection.RIGHT_TO_LEFT;
            }
            else
            {
                lineDirection = LineDirection.LEFT_TO_RIGHT;
            }

            return new LineModel(a, b, lineDirection);
        }

        public static LineModel CalculateLineFromPointAndDirection(PointModel point, double direction)
        {
            double yAxisAngle;
            int quadrant;
            if (direction < 90)
            {
                yAxisAngle = direction;
                quadrant = 1;
            }
            else if (direction < 180)
            {
                yAxisAngle = 180 - direction;
                quadrant = 2;
            }
            else if (direction < 270)
            {
                yAxisAngle = direction - 180;
                quadrant = 3;
            }
            else
            {
                yAxisAngle = 360 - direction;
                quadrant = 4;
            }

            var sin = MathOperation.Sin(yAxisAngle);
            var cos = MathOperation.Cos(yAxisAngle);
            var toNextPointLength = 100;
            //var nextPointX = sin * toNextPointLength + point.x;
            //var nextPointY = cos * toNextPointLength + point.y;
            double nextPointX, nextPointY;
            if(quadrant == 1 || quadrant == 2)
            {
                nextPointX = sin * toNextPointLength + point.x;
            }
            else
            {
                nextPointX = point.x - sin * toNextPointLength;
            }
            if(quadrant == 1 || quadrant == 4)
            {
                nextPointY = cos * toNextPointLength + point.y;
            }
            else
            {
                nextPointY = point.y - cos * toNextPointLength;
            }


            var nextPoint = new PointModel(nextPointX, nextPointY);
            var line = CalculateLineFromTwoPoint(point, nextPoint);

            return line;
        }

        public static LineModel CalculateLineFromSlopeAndPoint(double a, PointModel point, LineDirection direction)
        {
            double b = point.y - point.x * a;
            var result = new LineModel(a, b, direction);
            return result;
        }

        public static PointModel CalculatePointByTwoLines(LineModel lineA, LineModel lineB)
        {
            var x = (lineB.b - lineA.b) / (lineA.a - lineB.a);
            var y = lineA.CalculateY(x);

            return new PointModel(x, y);
        }

        public static double CalculateDirectionFromLine(LineModel line)
        {
            var throughZeroLine = new LineModel(line.a, 0, line.lineDirection);

            double x = 100.00;
            var y = throughZeroLine.CalculateY(x);
            if (y == 0)
            {
                return 0;
            }

            var tan = Math.Abs( x / y );
            var angle = MathOperation.TanH(tan);
            if (line.a > 0)
            {
                var direction = angle;
                if (line.lineDirection == LineDirection.RIGHT_TO_LEFT)
                {
                    direction = direction + 180;
                }

                if (direction > 360)
                {
                    direction = direction - 360;
                }

                return Math.Round(direction, 2);
            }
            else
            {
                var direction = 360 - angle;
                if (line.lineDirection == LineDirection.LEFT_TO_RIGHT)
                {
                    direction = direction - 180;
                }

                if (direction > 360)
                {
                    direction = direction - 360;
                }

                return Math.Round(direction, 2);
            }
        }

        public static double CalculateLengthBetweenPoints(PointModel point1, PointModel point2)
        {
            var result = Math.Sqrt(Math.Pow(point2.x - point1.x, 2) + Math.Pow(point2.y - point1.y, 2));
            return result;
        }

        public static double CalculateLengthBetweenPointAndLine(PointModel point, LineModel line)
        {
            var result = (line.a * point.x - point.y + line.b) / Math.Sqrt(line.a * line.a + 1);
            return result;
        }

        public static AttackParameterModel CalculateAttackParameters(LineModel targetLine, double targetHeading, double subHeading, double attackDirection)
        {
            var actualAttackDirection = DirectionOperation.CalculateSubActualDirection(subHeading, attackDirection);

            var attackLine = CalculateLineFromPointAndDirection(new PointModel(0,0), actualAttackDirection);
            var attackPoint = CalculatePointByTwoLines(targetLine, attackLine);

            double aob = 0;
            if (actualAttackDirection < 180 && targetLine.b>0)
            {
                if(targetLine.lineDirection == LineDirection.LEFT_TO_RIGHT)
                {
                    aob = 180 - Math.Abs(targetHeading - actualAttackDirection);
                }
                else
                {
                    aob = Math.Abs(targetHeading - actualAttackDirection);
                }
            }
            else if (actualAttackDirection < 360 && targetLine.b>0)
            {
                if (targetLine.lineDirection == LineDirection.LEFT_TO_RIGHT)
                {
                    aob = Math.Abs(actualAttackDirection - targetHeading) - 180;
                }
                else
                {
                    aob = 180 - Math.Abs(actualAttackDirection - targetHeading);
                }
            }
            else if (actualAttackDirection < 180 && targetLine.b <= 0)
            {
                if (targetLine.lineDirection == LineDirection.LEFT_TO_RIGHT)
                {
                    aob = 180 - Math.Abs(actualAttackDirection - targetHeading);
                }
                else
                {
                    aob = Math.Abs(actualAttackDirection - targetHeading);
                }
            }
            else if (actualAttackDirection < 360 && targetLine.b > 0)
            {
                if (targetLine.lineDirection == LineDirection.LEFT_TO_RIGHT)
                {
                    aob = Math.Abs(targetHeading - actualAttackDirection);
                }
                else
                {
                    aob = 180 - Math.Abs(targetHeading - actualAttackDirection);
                }
            }
            
            return new AttackParameterModel(attackPoint, Math.Round(aob, 2));

        }

        public static LineModel CalculateRandomTargetLineByRotate(LineModel directionLine1, LineModel directionLine2, LineModel directionLine3)
        {
            var randomLength = 3000;
            var randomPoint = CalculatePointByLineAndLength(directionLine2, new PointModel(0, 0), randomLength, directionLine2.lineDirection);

            var startDirection = CalculateDirectionFromLine(directionLine2);
            var increaseDirectionStep = 0.01;
            double increaseDirection = 0;
            var idealLengthDiff = 0.01;
            LineModel result = null;
            double minimumLengthDiff = 99999999999;
            while (true)
            {
                increaseDirection = increaseDirection + increaseDirectionStep;
                if(increaseDirection >= 180)
                {
                    break;
                }
                if(minimumLengthDiff <= idealLengthDiff)
                {
                    break;
                }

                var candidateHeading = startDirection + increaseDirection;
                candidateHeading = DirectionOperation.CorrectDirection(candidateHeading);
                var candidateLine = CalculateLineFromPointAndDirection(randomPoint, candidateHeading);
                var interceptPoint1 = CalculatePointByTwoLines(candidateLine, directionLine1);
                var interceptPoint3 = CalculatePointByTwoLines(candidateLine, directionLine3);

                var length1 = CalculateLengthBetweenPoints(interceptPoint1, randomPoint);
                var length2 = CalculateLengthBetweenPoints(interceptPoint3, randomPoint);
                var lengthDiff = Math.Abs(length1 - length2);

                //verify
                var intercept2 = CalculatePointByTwoLines(candidateLine, directionLine2);
                if(intercept2.x<-1000000 || intercept2.x > 1000000)
                {
                    continue;
                }
                if(lengthDiff == 0)
                {
                    continue;
                }

                if (lengthDiff < minimumLengthDiff)
                {
                    result = candidateLine;
                    minimumLengthDiff = lengthDiff;
                }
            }

            return result;
        }

        public static LineModel CalculateRandomTargetLineByMath(LineModel directionLine1, LineModel directionLine2, LineModel directionLine3)
        {
            var randomLength = 3000;
            var randomPoint = CalculatePointByLineAndLength(directionLine2, new PointModel(0, 0), randomLength, directionLine2.lineDirection);

            var verticalSlope = -1.0 / directionLine2.a;
            var verticalLine = CalculateLineFromSlopeAndPoint(verticalSlope, randomPoint, LineDirection.LEFT_TO_RIGHT);

            var verticalIntercept1 = CalculatePointByTwoLines(directionLine1, verticalLine);
            var verticalIntercept3 = CalculatePointByTwoLines(directionLine3, verticalLine);
            var length1 = CalculateLengthBetweenPoints(verticalIntercept1, randomPoint);
            var length3 = CalculateLengthBetweenPoints(verticalIntercept3, randomPoint);

            LineModel circleLine;
            LineModel otherLine;
            PointModel circlePoint;
            double circleR;
            if (length1 < length3)
            {
                circleLine = directionLine1;
                otherLine = directionLine3;
                circlePoint = verticalIntercept1;
                circleR = length1;
            }
            else
            {
                circleLine = directionLine3;
                otherLine = directionLine1;
                circlePoint = verticalIntercept3;
                circleR = length3;
            }

            var circleDiamLine = CalculateLineFromTwoPoint(circlePoint, randomPoint);
            var circleOppositePoint = CalculatePointByLineAndLength(circleDiamLine, randomPoint, circleR, circleDiamLine.lineDirection);

            var parallelLine = CalculateLineFromSlopeAndPoint(circleLine.a, circleOppositePoint, circleLine.lineDirection);
            var randomTargetLineInterceptPoint = CalculatePointByTwoLines(parallelLine, otherLine);

            var randomTargetLine = CalculateLineFromTwoPoint(randomTargetLineInterceptPoint, randomPoint);
            return randomTargetLine;
        }
    }
}
