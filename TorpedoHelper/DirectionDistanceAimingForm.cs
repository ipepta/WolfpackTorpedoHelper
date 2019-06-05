using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TorpedoHelper
{
    public partial class DirectionDistanceAimingForm : Form
    {
        public LineModel targetLine;
        private NauticalChartForm nauticalChartForm;
        private DirectionAimingForm directionAimingForm;
        private Thread currentPosThread;

        public DirectionDistanceAimingForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            directionAimingForm = new DirectionAimingForm();
            directionAimingForm.Show();

            nauticalChartForm = new NauticalChartForm(this, directionAimingForm);
            nauticalChartForm.Show();


            this.currentPosThread = new Thread(new ThreadStart(CalculateCurrentPos));
            this.currentPosThread.Start();
        }

        private void CalculateCurrentPos()
        {
            while (true)
            {
                ObservationOperation.CalculateTargetCurrentPos(targetLine, targetPosition1Text, observeDate1Text, targetPosition2Text, observeDate2Text, targetObserveSpeedText, ref targetEstimatedPositionText);

                Thread.Sleep(1000);
            }
        }

        #region observation on change
        private void targetDirection1Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateTargetObservationPos(targetDirection1Text, subHeadingText, targetObserveHeight1Text, targetHeightText, ref targetDistance1Text, ref observeDate1Text, ref targetPosition1Text);
            ObservationOperation.CalculateTargetLine(targetPosition1Text, observeDate1Text, targetPosition2Text, observeDate2Text, targetPosition3Text, ref targetLineText, ref targetLine, ref targetHeadingText);
            ObservationOperation.CalcualteAttackParameter(targetLine, targetHeadingText, subHeadingText, attackDirectionText, ref aobText, ref rangeText);
            ObservationOperation.CalculateCorrectionPointDistance(targetLine, targetPosition3Text, ref correctionPointDistanceText);
        }

        private void targetHeight1Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateTargetObservationPos(targetDirection1Text, subHeadingText, targetObserveHeight1Text, targetHeightText, ref targetDistance1Text, ref observeDate1Text, ref targetPosition1Text);
            ObservationOperation.CalculateTargetLine(targetPosition1Text, observeDate1Text, targetPosition2Text, observeDate2Text, targetPosition3Text, ref targetLineText, ref targetLine, ref targetHeadingText);
            ObservationOperation.CalcualteAttackParameter(targetLine, targetHeadingText, subHeadingText, attackDirectionText, ref aobText, ref rangeText);
            ObservationOperation.CalculateCorrectionPointDistance(targetLine, targetPosition3Text, ref correctionPointDistanceText);
        }

        private void targetDirection2Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateTargetObservationPos(targetDirection2Text, subHeadingText, targetObserveHeight2Text, targetHeightText, ref targetDistance2Text, ref observeDate2Text, ref targetPosition2Text);
            ObservationOperation.CalculateTargetLine(targetPosition1Text, observeDate1Text, targetPosition2Text, observeDate2Text, targetPosition3Text, ref targetLineText, ref targetLine, ref targetHeadingText);
            ObservationOperation.CalcualteAttackParameter(targetLine, targetHeadingText, subHeadingText, attackDirectionText, ref aobText, ref rangeText);
            ObservationOperation.CalculateCorrectionPointDistance(targetLine, targetPosition3Text, ref correctionPointDistanceText);
        }

        private void targetHeight2Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateTargetObservationPos(targetDirection2Text, subHeadingText, targetObserveHeight2Text, targetHeightText, ref targetDistance2Text, ref observeDate2Text, ref targetPosition2Text);
            ObservationOperation.CalculateTargetLine(targetPosition1Text, observeDate1Text, targetPosition2Text, observeDate2Text, targetPosition3Text, ref targetLineText, ref targetLine, ref targetHeadingText);
            ObservationOperation.CalcualteAttackParameter(targetLine, targetHeadingText, subHeadingText, attackDirectionText, ref aobText, ref rangeText);
            ObservationOperation.CalculateCorrectionPointDistance(targetLine, targetPosition3Text, ref correctionPointDistanceText);
        }

        private void targetDirection3Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateTargetObservationPos(targetDirection3Text, subHeadingText, targetObserveHeight3Text, targetHeightText, ref targetDistance3Text, ref observeDate3Text, ref targetPosition3Text);
            ObservationOperation.CalculateTargetLine(targetPosition1Text, observeDate1Text, targetPosition2Text, observeDate2Text, targetPosition3Text, ref targetLineText, ref targetLine, ref targetHeadingText);
            ObservationOperation.CalcualteAttackParameter(targetLine, targetHeadingText, subHeadingText, attackDirectionText, ref aobText, ref rangeText);
            ObservationOperation.CalculateCorrectionPointDistance(targetLine, targetPosition3Text, ref correctionPointDistanceText);
        }

        private void targetHeight3Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateTargetObservationPos(targetDirection3Text, subHeadingText, targetObserveHeight3Text, targetHeightText, ref targetDistance3Text, ref observeDate3Text, ref targetPosition3Text);
            ObservationOperation.CalculateTargetLine(targetPosition1Text, observeDate1Text, targetPosition2Text, observeDate2Text, targetPosition3Text, ref targetLineText, ref targetLine, ref targetHeadingText);
            ObservationOperation.CalcualteAttackParameter(targetLine, targetHeadingText, subHeadingText, attackDirectionText, ref aobText, ref rangeText);
            ObservationOperation.CalculateCorrectionPointDistance(targetLine, targetPosition3Text, ref correctionPointDistanceText);
        }

        #endregion

        #region basic info on change
        private void targetHeightText_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateTargetObservationPos(targetDirection1Text, subHeadingText, targetObserveHeight1Text, targetHeightText, ref targetDistance1Text, ref observeDate1Text, ref targetPosition1Text, updateObservationDate: false);

            ObservationOperation.CalculateTargetObservationPos(targetDirection2Text, subHeadingText, targetObserveHeight2Text, targetHeightText, ref targetDistance2Text, ref observeDate2Text, ref targetPosition2Text, updateObservationDate: false);

            ObservationOperation.CalculateTargetObservationPos(targetDirection3Text, subHeadingText, targetObserveHeight3Text, targetHeightText, ref targetDistance3Text, ref observeDate3Text, ref targetPosition3Text, updateObservationDate: false);

            ObservationOperation.CalculateTargetLine(targetPosition1Text, observeDate1Text, targetPosition2Text, observeDate2Text, targetPosition3Text, ref targetLineText, ref targetLine, ref targetHeadingText);
            ObservationOperation.CalcualteAttackParameter(targetLine, targetHeadingText, subHeadingText, attackDirectionText, ref aobText, ref rangeText);
            ObservationOperation.CalculateCorrectionPointDistance(targetLine, targetPosition3Text, ref correctionPointDistanceText);
        }
        #endregion

        #region target pass center on change
        private void targetPassCenterTime_TextChanged(object sender, EventArgs e)
        {
            SpeedOperation.CalculateSpeed(targetLengthText, targetHeadingText, subHeadingText, targetPassCenterTimeText, targetPassCenterDirectionText, ref targetObserveSpeedText);
        }

        private void targetPassCenterDirectionText_TextChanged(object sender, EventArgs e)
        {
            SpeedOperation.CalculateSpeed(targetLengthText, targetHeadingText, subHeadingText, targetPassCenterTimeText, targetPassCenterDirectionText, ref targetObserveSpeedText);
        }

        private void targetLengthText_TextChanged(object sender, EventArgs e)
        {
            SpeedOperation.CalculateSpeed(targetLengthText, targetHeadingText, subHeadingText, targetPassCenterTimeText, targetPassCenterDirectionText, ref targetObserveSpeedText);
        }

        private void targetHeadingText_TextChanged(object sender, EventArgs e)
        {
            SpeedOperation.CalculateSpeed(targetLengthText, targetHeadingText, subHeadingText, targetPassCenterTimeText, targetPassCenterDirectionText, ref targetObserveSpeedText);
        }

        private void subHeadingText_TextChanged(object sender, EventArgs e)
        {
            SpeedOperation.CalculateSpeed(targetLengthText, targetHeadingText, subHeadingText, targetPassCenterTimeText, targetPassCenterDirectionText, ref targetObserveSpeedText);
        }

        #endregion

        #region attack parameter on change

        private void attackDirectionText_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalcualteAttackParameter(targetLine, targetHeadingText, subHeadingText, attackDirectionText, ref aobText, ref rangeText);
        }

        #endregion


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void DirectionDistanceAimingForm_Shown(object sender, EventArgs e)
        {
            directionAimingForm.Top = this.Top + this.Height + 10;
            directionAimingForm.Left = this.Left;

            nauticalChartForm.Top = this.Top;
            nauticalChartForm.Left = this.Left + this.Width + 10;
        }
    }
}
