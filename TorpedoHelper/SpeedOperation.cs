using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TorpedoHelper
{
    public static class SpeedOperation
    {
        public static void CalculateSpeed(TextBox targetLengthText, TextBox targetHeadingText, TextBox subHeadingText, TextBox targetPassCenterTimeText, TextBox targetPassCenterDicretionText, ref TextBox targetSpeedText)
        {
            if (string.IsNullOrEmpty(targetLengthText.Text) || string.IsNullOrEmpty(targetPassCenterDicretionText.Text) || string.IsNullOrEmpty(targetHeadingText.Text) || string.IsNullOrEmpty(targetPassCenterTimeText.Text) || string.IsNullOrEmpty(subHeadingText.Text))
            {
                return;
            }

            double targetLength, targetPassCenterDirection, targetHeading, targetPassCenterTime, subHeading = 0;
            try
            {
                targetLength = Convert.ToDouble(targetLengthText.Text);
                targetPassCenterDirection = Convert.ToDouble(targetPassCenterDicretionText.Text);
                targetHeading = Convert.ToDouble(targetHeadingText.Text);
                targetPassCenterTime = Convert.ToDouble(targetPassCenterTimeText.Text);
                subHeading = Convert.ToDouble(subHeadingText.Text);
            }
            catch
            {
                MessageBox.Show("invalid number: " + targetLengthText.Text + ", " + targetPassCenterDicretionText.Text + ", " + targetHeadingText.Text + ", " + targetPassCenterTimeText.Text + ", " + subHeadingText.Text);
                return;
            }

            double actualTargetPassCenterDirection = DirectionOperation.CalculateSubActualDirection(subHeading, targetPassCenterDirection);
            //if (targetPassCenterDirection > 180)
            //{
            //    actualTargetPassDirection = subHeading - (360 - targetPassCenterDirection);
            //}
            //else
            //{
            //    actualTargetPassDirection = subHeading + targetPassCenterDirection;
            //}
            double standardTargetHeading = actualTargetPassCenterDirection - 90;
            standardTargetHeading = DirectionOperation.CorrectDirection(standardTargetHeading);

            double targetTiltAngle = Math.Abs(targetHeading - standardTargetHeading);
            if (targetTiltAngle > 180)
            {
                targetTiltAngle = Math.Min(targetHeading, standardTargetHeading) + 360 - Math.Max(targetHeading, standardTargetHeading);
                DirectionOperation.CorrectDirection(targetTiltAngle);
            }
            double actualLength = MathOperation.Cos(targetTiltAngle) * targetLength;
            double standardSpeed = Math.Round(actualLength / targetPassCenterTime * 1.94, 2);
            double targetSpeed = standardSpeed / MathOperation.Cos(targetTiltAngle);

            targetSpeedText.Text = targetSpeed.ToString();
        }
    }
}
