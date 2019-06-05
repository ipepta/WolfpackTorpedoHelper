using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TorpedoHelper
{
    public static class ObservationOperation
    {
        public static void CalculateTargetObservationPos(TextBox targetDirectionText, TextBox subDirectionText, TextBox observationHeightText, TextBox targetHeightText, ref TextBox distanceText, ref TextBox observationDateText, ref TextBox targetPositionText, bool updateObservationDate = true)
        {
            if (updateObservationDate)
            {
                observationDateText.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (string.IsNullOrEmpty(targetHeightText.Text) || string.IsNullOrEmpty(observationHeightText.Text))
            {
                return;
            }

            double targetHeight, observationHeight = 0;
            try
            {
                targetHeight = Convert.ToDouble(targetHeightText.Text);
                observationHeight = Convert.ToDouble(observationHeightText.Text);
            }
            catch
            {
                MessageBox.Show("invalid number: " + targetHeightText.Text + ", " + observationHeightText.Height);
                return;
            }

            var distance = Math.Round(targetHeight / observationHeight * 400.00, 2);
            distanceText.Text = distance.ToString();

            if (string.IsNullOrEmpty(targetDirectionText.Text) || string.IsNullOrEmpty(subDirectionText.Text))
            {
                return;
            }

            double targetDirection = 0, subDirection = 0;
            try
            {
                targetDirection = Convert.ToDouble(targetDirectionText.Text);
                subDirection = Convert.ToDouble(subDirectionText.Text);
            }
            catch
            {
                MessageBox.Show("invalid number: " + targetDirectionText.Text + ", " + subDirectionText.Text);
            }

            var actualDirection = DirectionOperation.CalculateSubActualDirection(subDirection, targetDirection);
            double xAxisAngle;
            int quadrantIndex;
            if (actualDirection < 90)
            {
                xAxisAngle = 90 - actualDirection;
                quadrantIndex = 1;
            }
            else if (actualDirection < 180)
            {
                xAxisAngle = actualDirection - 90;
                quadrantIndex = 2;
            }
            else if (actualDirection < 270)
            {
                xAxisAngle = 270 - actualDirection;
                quadrantIndex = 3;
            }
            else
            {
                xAxisAngle = actualDirection - 270;
                quadrantIndex = 4;
            }

            var cos = MathOperation.Cos(xAxisAngle);
            var sin = MathOperation.Sin(xAxisAngle);
            var x = cos * distance;
            var y = sin * distance;

            if (quadrantIndex == 2)
            {
                y = -1 * y;
            }
            if (quadrantIndex == 3)
            {
                y = -1 * y;
                x = -1 * x;
            }
            if (quadrantIndex == 4)
            {
                x = -1 * x;
            }

            var point = new PointModel(x, y);
            targetPositionText.Text = point.ToString();
        }

        public static void CalculateTargetLine(TextBox targetPos1Text, TextBox observeDate1Text, TextBox targetPos2Text, TextBox observeDate2Text, TextBox targetPos3Text, ref TextBox targetLineText, ref LineModel targetLine, ref TextBox targetHeadingText)
        {
            var pos1 = PointModel.Parse(targetPos1Text.Text);
            var pos2 = PointModel.Parse(targetPos2Text.Text);
            var pos3 = PointModel.Parse(targetPos3Text.Text);

            if (pos1 == null || pos2 == null)
            {
                return;
            }

            DateTime pos1Date, pos2Date;
            try
            {
                pos1Date = DateTime.ParseExact(observeDate1Text.Text, "yyyy-MM-dd HH:mm:ss", null);
                pos2Date = DateTime.ParseExact(observeDate2Text.Text, "yyyy-MM-dd HH:mm:ss", null);
            }
            catch
            {
                MessageBox.Show("invalid date time:" + observeDate1Text.Text + ", " + observeDate2Text.Text);
                return;
            }

            PointModel earlyPoint, laterPoint;
            if (pos1Date < pos2Date)
            {
                earlyPoint = pos1;
                laterPoint = pos2;
            }
            else
            {
                earlyPoint = pos2;
                laterPoint = pos1;
            }

            var line = CoordinateSystemOperation.CalculateLineFromTwoPoint(earlyPoint, laterPoint);
            targetLine = line;
            targetLineText.Text = line.ToString();

            var targetHeading = CoordinateSystemOperation.CalculateDirectionFromLine(line);
            targetHeadingText.Text = targetHeading.ToString();
        }
       
        public static void CalculateTargetCurrentPos(LineModel targetLine, TextBox targetPosition1Text, TextBox observeDate1Text, TextBox targetPosition2Text, TextBox observeDate2Text, TextBox targetObserveSpeedText, ref TextBox targetEstimatedPosText)
        {
            if (targetLine == null || string.IsNullOrEmpty(targetPosition1Text.Text) || string.IsNullOrEmpty(observeDate1Text.Text) || string.IsNullOrEmpty(targetPosition2Text.Text) || string.IsNullOrEmpty(observeDate2Text.Text) || string.IsNullOrEmpty(targetObserveSpeedText.Text))
            {
                return;
            }

            PointModel pos1 = PointModel.Parse(targetPosition1Text.Text);
            PointModel pos2 = PointModel.Parse(targetPosition2Text.Text);
            if (pos1 == null || pos2 == null)
            {
                return;
            }

            DateTime pos1Date, pos2Date;
            try
            {
                pos1Date = DateTime.ParseExact(observeDate1Text.Text, "yyyy-MM-dd HH:mm:ss", null);
                pos2Date = DateTime.ParseExact(observeDate2Text.Text, "yyyy-MM-dd HH:mm:ss", null);
            }
            catch
            {
                return;
            }

            DateTime latestPosDate;
            PointModel latestPos;
            if (pos1Date > pos2Date)
            {
                latestPosDate = pos1Date;
                latestPos = pos1;
            }
            else
            {
                latestPosDate = pos2Date;
                latestPos = pos2;
            }
            var time = (DateTime.Now - latestPosDate).TotalSeconds;


            double speedByKnot;
            try
            {
                speedByKnot = Convert.ToDouble(targetObserveSpeedText.Text);
            }
            catch
            {
                return;
            }

            var speedByMeter = speedByKnot / 1.94;
            var passingDistance = time * speedByMeter;
            var currentPos = CoordinateSystemOperation.CalculatePointByLineAndLength(targetLine, latestPos, passingDistance, targetLine.lineDirection);
            targetEstimatedPosText.Text = currentPos.ToString();

        }

        public static void CalculateTargetCurrentPos(LineModel targetLine, PointModel targetActualPos, DateTime targetActualPosDate, TextBox targetSpeedText, ref TextBox targetEstimatedPosText)
        {
            if(targetLine == null || targetActualPos == null || targetActualPosDate == DateTime.MinValue || string.IsNullOrEmpty(targetSpeedText.Text))
            {
                return;
            }

            double speedByKnot;
            try
            {
                speedByKnot = Convert.ToDouble(targetSpeedText.Text);
            }
            catch
            {
                return;
            }

            var time = (DateTime.Now - targetActualPosDate).TotalSeconds;

            var speedByMeter = speedByKnot / 1.94;
            var passingDistance = time * speedByMeter;
            var currentPos = CoordinateSystemOperation.CalculatePointByLineAndLength(targetLine, targetActualPos, passingDistance, targetLine.lineDirection);
            targetEstimatedPosText.Text = currentPos.ToString();
        }

        public static void CalculateCorrectionPointDistance(LineModel targetLine, TextBox targetPosition3Text, ref TextBox correctionPointDistanceText)
        {
            if (targetLine == null || string.IsNullOrEmpty(targetPosition3Text.Text))
            {
                return;
            }

            var pos3 = PointModel.Parse(targetPosition3Text.Text);
            if (pos3 == null)
            {
                return;
            }

            var correctionDistance = CoordinateSystemOperation.CalculateLengthBetweenPointAndLine(pos3, targetLine);
            correctionPointDistanceText.Text = Math.Round(correctionDistance, 2).ToString();
        }

        public static void CalcualteAttackParameter(LineModel targetLine, TextBox targetHeadingText, TextBox subHeadingText, TextBox attackDirectionText, ref TextBox aobText, ref TextBox rangeText)
        {
            if (targetLine == null || string.IsNullOrEmpty(targetHeadingText.Text) || string.IsNullOrEmpty(subHeadingText.Text) || string.IsNullOrEmpty(attackDirectionText.Text))
            {
                return;
            }

            double targetHeading, subHeading, attackDirection;
            try
            {
                targetHeading = Convert.ToDouble(targetHeadingText.Text);
                subHeading = Convert.ToDouble(subHeadingText.Text);
                attackDirection = Convert.ToDouble(attackDirectionText.Text);
            }
            catch
            {
                MessageBox.Show("invalid number: " + targetHeadingText.Text + ", " + subHeadingText.Text + ", " + attackDirectionText.Text);
                return;
            }

            var attackParam = CoordinateSystemOperation.CalculateAttackParameters(targetLine, targetHeading, subHeading, attackDirection);
            aobText.Text = attackParam.AOB.ToString(); ;
            rangeText.Text = attackParam.Range.ToString();
        }

        public static void CalculateDirectionLine(TextBox subHeadingText, TextBox directionText, TextBox observePos1PositionText, ref LineModel directionLine, ref TextBox directionLineText, bool isObservePos1DirectionLine = true)
        {
            if (string.IsNullOrEmpty(directionText.Text) || string.IsNullOrEmpty(subHeadingText.Text))
            {
                return;
            }

            double direction, subHeading;
            try
            {
                direction = Convert.ToDouble(directionText.Text);
                subHeading = Convert.ToDouble(subHeadingText.Text);
            }
            catch
            {
                MessageBox.Show("invalid number: " + directionText.Text+", "+subHeadingText.Text);
                return;
            }

            direction = DirectionOperation.CalculateSubActualDirection(subHeading, direction);

            PointModel observePos = new PointModel(0, 0);
            if (isObservePos1DirectionLine && !string.IsNullOrEmpty(observePos1PositionText.Text))
            {
                observePos = PointModel.Parse(observePos1PositionText.Text);

                if (observePos == null)
                {
                    return;
                }
            }

            directionLine = CoordinateSystemOperation.CalculateLineFromPointAndDirection(observePos, direction);
            directionLineText.Text = directionLine.ToString();
        }

        public static void CalculateSubObservePosition1(TextBox subHeadingText, TextBox odometer1Text, TextBox odometer2Text, ref TextBox observePosition1Text)
        {
            if (string.IsNullOrEmpty(subHeadingText.Text) || string.IsNullOrEmpty(odometer1Text.Text) || string.IsNullOrEmpty(odometer2Text.Text))
            {
                return;
            }

            double subHeading, odometer1, odometer2;
            try
            {
                subHeading = Convert.ToDouble(subHeadingText.Text);
                odometer1 = Convert.ToDouble(odometer1Text.Text);
                odometer2 = Convert.ToDouble(odometer2Text.Text);
            }
            catch
            {
                MessageBox.Show("invalid number: " + subHeadingText.Text + ", " + odometer1Text.Text + ", " + odometer2Text.Text);
                return;
            }

            var reverseDirection = subHeading - 180;
            reverseDirection = DirectionOperation.CorrectDirection(reverseDirection);
            var reverseLine = CoordinateSystemOperation.CalculateLineFromPointAndDirection(new PointModel(0, 0), reverseDirection);
            var observePos1 = CoordinateSystemOperation.CalculatePointByLineAndLength(reverseLine, new PointModel(0, 0), Math.Abs(odometer2 - odometer1), reverseLine.lineDirection);
            observePosition1Text.Text = observePos1.ToString();
        }

        public static void CalculateRandomTargetLine(LineModel directionLine1, LineModel directionLine2, LineModel directionLine3, ref LineModel randomTargetLine, ref TextBox randomTargetLineText, ref TextBox targetLineLengthDiffText, ref TextBox randomTargetLineMethodText)
        {
            if (directionLine1 == null || directionLine2 == null || directionLine3 == null)
            {
                return;
            }

            var randomTaretLineByMath = CoordinateSystemOperation.CalculateRandomTargetLineByMath(directionLine1, directionLine2, directionLine3);
            var randomTargetLineByRotate = CoordinateSystemOperation.CalculateRandomTargetLineByRotate(directionLine1, directionLine2, directionLine3);
            if (randomTargetLineByRotate == null)
            {
                MessageBox.Show("calculate parallel route failed.");
                return;
            }

            var rotateLineDiff = CalculateRandomTargetLineLengthDiff(directionLine1, directionLine2, directionLine3, randomTargetLineByRotate);
            var mathLineDiff = CalculateRandomTargetLineLengthDiff(directionLine1, directionLine2, directionLine3, randomTaretLineByMath);

            if (rotateLineDiff < mathLineDiff)
            {
                randomTargetLine = randomTargetLineByRotate;
                randomTargetLineMethodText.Text = "ROTATE";
                targetLineLengthDiffText.Text = rotateLineDiff.ToString();
            }
            else
            {
                randomTargetLine = randomTaretLineByMath;
                randomTargetLineMethodText.Text = "CALCULATE";
                targetLineLengthDiffText.Text = mathLineDiff.ToString();
            }

            randomTargetLineText.Text = randomTargetLine.ToString();
        }

        public static void CalculateEstimatedDirectionLine(LineModel directionLine1, LineModel directionLine2, LineModel directionLine3, LineModel randomTargetLine, TextBox observePos1Text, TextBox observePos1TimeDiffText, TextBox observePos2TimeDiffText, ref LineModel estimatedDirectionLine, ref TextBox estimatedDirectionLineText, ref TextBox targetHeadingText)
        {
            if (directionLine1 == null || directionLine2 == null || directionLine3 == null || randomTargetLine == null)
            {
                return;
            }

            double pos2LengthRatio = 1;
            if(!string.IsNullOrEmpty(observePos1TimeDiffText.Text) && !string.IsNullOrEmpty(observePos2TimeDiffText.Text))
            {
                try
                {
                    double pos1TimeDiff = Convert.ToDouble(observePos1TimeDiffText.Text);
                    double pos2TimeDiff = Convert.ToDouble(observePos2TimeDiffText.Text);
                    pos2LengthRatio = pos2TimeDiff / pos1TimeDiff;
                }
                catch
                {  }
            }

            var intercept1 = CoordinateSystemOperation.CalculatePointByTwoLines(directionLine1, randomTargetLine);
            var intercept2 = CoordinateSystemOperation.CalculatePointByTwoLines(directionLine2, randomTargetLine);
            var intercept3 = CoordinateSystemOperation.CalculatePointByTwoLines(directionLine3, randomTargetLine);

            var length1 = CoordinateSystemOperation.CalculateLengthBetweenPoints(intercept1, intercept2);
            var length2 = CoordinateSystemOperation.CalculateLengthBetweenPoints(intercept2, intercept3);
            var length = (length1 + length2) / 2;
            length = length * pos2LengthRatio;

            LineDirection direction;
            if (intercept2.x > intercept1.x)
            {
                direction = LineDirection.LEFT_TO_RIGHT;
            }
            else
            {
                direction = LineDirection.RIGHT_TO_LEFT;
            }

            PointModel observePos = new PointModel(0, 0);
            if (!string.IsNullOrEmpty(observePos1Text.Text))
            {
                observePos = PointModel.Parse(observePos1Text.Text);
                if(observePos == null)
                {
                    return;
                }
            }

            var estimatedPoint = CoordinateSystemOperation.CalculatePointByLineAndLength(randomTargetLine, intercept3, length, direction);

            estimatedDirectionLine = CoordinateSystemOperation.CalculateLineFromTwoPoint(observePos, estimatedPoint);
            estimatedDirectionLineText.Text = estimatedDirectionLine.ToString();

            var headingLine = CoordinateSystemOperation.CalculateLineFromTwoPoint(intercept1, intercept2);
            var heading = CoordinateSystemOperation.CalculateDirectionFromLine(headingLine);
            targetHeadingText.Text = Math.Round(heading, 2).ToString();
        }

        public static void CalculateActualTargetLine(LineModel randomTargetLine, LineModel estimatedDirectionLine, LineModel directionLine1, LineModel directionLine2, LineModel directionLine3, LineModel directionLine4, TextBox observePos1TimeDiffText, ref LineModel actualTargetLine, ref TextBox actualTargetLineText, ref PointModel targetConfirmedPoint, ref DateTime targetConfirmedPointDate, ref TextBox targetSpeedText)
        {
            if(randomTargetLine == null || estimatedDirectionLine == null || directionLine4 == null)
            {
                return;
            }

            targetConfirmedPoint = CoordinateSystemOperation.CalculatePointByTwoLines(estimatedDirectionLine, directionLine4);
            targetConfirmedPointDate = DateTime.Now;
            actualTargetLine = CoordinateSystemOperation.CalculateLineFromSlopeAndPoint(randomTargetLine.a, targetConfirmedPoint, randomTargetLine.lineDirection);
            actualTargetLineText.Text = actualTargetLine.ToString();

            if (string.IsNullOrEmpty(observePos1TimeDiffText.Text) || directionLine1 == null || directionLine2 == null || directionLine3 == null)
            {
                return;
            }

            double time;
            try
            {
                time = Convert.ToDouble(observePos1TimeDiffText.Text);
            }
            catch
            {
                MessageBox.Show("invalid number: " + observePos1TimeDiffText.Text);
                return;
            }

            var intercept1 = CoordinateSystemOperation.CalculatePointByTwoLines(actualTargetLine, directionLine1);
            var intercept2 = CoordinateSystemOperation.CalculatePointByTwoLines(actualTargetLine, directionLine2);
            var intercept3 = CoordinateSystemOperation.CalculatePointByTwoLines(actualTargetLine, directionLine3);
            var length = (CoordinateSystemOperation.CalculateLengthBetweenPoints(intercept1, intercept2) + CoordinateSystemOperation.CalculateLengthBetweenPoints(intercept2, intercept3)) / 2.00;
            var speedByMeter = length / time;
            var speedByKnot = speedByMeter * 1.94;

            targetSpeedText.Text = Math.Round(speedByKnot, 2).ToString();

            var actualDirectionLine = CoordinateSystemOperation.CalculateLineFromTwoPoint(intercept1, intercept2);
            actualTargetLine.lineDirection = actualDirectionLine.lineDirection;
        }

        private static double CalculateRandomTargetLineLengthDiff(LineModel directionLine1, LineModel directionLine2, LineModel directionLine3, LineModel randomTargetLine)
        {
            var intercept1 = CoordinateSystemOperation.CalculatePointByTwoLines(directionLine1, randomTargetLine);
            var intercept2 = CoordinateSystemOperation.CalculatePointByTwoLines(directionLine2, randomTargetLine);
            var intercept3 = CoordinateSystemOperation.CalculatePointByTwoLines(directionLine3, randomTargetLine);
            var length1 = CoordinateSystemOperation.CalculateLengthBetweenPoints(intercept1, intercept2);
            var length2 = CoordinateSystemOperation.CalculateLengthBetweenPoints(intercept2, intercept3);
            var diff = Math.Abs(length1 - length2);

            return diff;
        }
    }
}
