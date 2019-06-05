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
    public partial class DirectionAimingForm : Form
    {
        public LineModel directionLine1, directionLine2, directionLine3, directionLine4, targetRandomLine, estimatedDirectionLine, targetActualLine;
        public PointModel targetConfirmedPos;
        public DateTime targetConfirmedPosDate;
        private Thread currentPosThread;

        public DirectionAimingForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            this.currentPosThread = new Thread(new ThreadStart(CalculateCurrentPos));
            this.currentPosThread.Start();
        }

        private void CalculateCurrentPos()
        {
            while (true)
            {
                ObservationOperation.CalculateTargetCurrentPos(targetActualLine, targetConfirmedPos, targetConfirmedPosDate, targetSpeedText, ref targetEstimatedPosText);

                Thread.Sleep(1000);
            }
        }

        #region sub move on change
        private void subHeadingText_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateSubObservePosition1(subMoveHeadingText, odometer1Text, odometer2Text, ref observePoint1PositionText);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection1Text, observePoint1PositionText, ref directionLine1, ref targetDirectionLine1Text);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection2Text, observePoint1PositionText, ref directionLine2, ref targetDirectionLine2Text);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection3Text, observePoint1PositionText, ref directionLine3, ref targetDirectionLine3Text);
            ObservationOperation.CalculateRandomTargetLine(directionLine1, directionLine2, directionLine3, ref targetRandomLine, ref randomTargetLineText, ref targetLineLengthDiffText, ref randomTargetLineCalculationMethodText);
            ObservationOperation.CalculateEstimatedDirectionLine(directionLine1, directionLine2, directionLine3, targetRandomLine, observePoint1PositionText, observePos1TimeDiffText, observePos2TimeDiffText, ref estimatedDirectionLine, ref estimatedDirectionLineText, ref targetHeadingText);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }

        private void odometer1Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateSubObservePosition1(subMoveHeadingText, odometer1Text, odometer2Text, ref observePoint1PositionText);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection1Text, observePoint1PositionText, ref directionLine1, ref targetDirectionLine1Text);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection2Text, observePoint1PositionText, ref directionLine2, ref targetDirectionLine2Text);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection3Text, observePoint1PositionText, ref directionLine3, ref targetDirectionLine3Text);
            ObservationOperation.CalculateRandomTargetLine(directionLine1, directionLine2, directionLine3, ref targetRandomLine, ref randomTargetLineText, ref targetLineLengthDiffText, ref randomTargetLineCalculationMethodText);
            ObservationOperation.CalculateEstimatedDirectionLine(directionLine1, directionLine2, directionLine3, targetRandomLine, observePoint1PositionText, observePos1TimeDiffText, observePos2TimeDiffText, ref estimatedDirectionLine, ref estimatedDirectionLineText, ref targetHeadingText);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }

        private void odometer2Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateSubObservePosition1(subMoveHeadingText, odometer1Text, odometer2Text, ref observePoint1PositionText);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection1Text, observePoint1PositionText, ref directionLine1, ref targetDirectionLine1Text);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection2Text, observePoint1PositionText, ref directionLine2, ref targetDirectionLine2Text);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection3Text, observePoint1PositionText, ref directionLine3, ref targetDirectionLine3Text);
            ObservationOperation.CalculateRandomTargetLine(directionLine1, directionLine2, directionLine3, ref targetRandomLine, ref randomTargetLineText, ref targetLineLengthDiffText, ref randomTargetLineCalculationMethodText);
            ObservationOperation.CalculateEstimatedDirectionLine(directionLine1, directionLine2, directionLine3, targetRandomLine, observePoint1PositionText, observePos1TimeDiffText, observePos2TimeDiffText, ref estimatedDirectionLine, ref estimatedDirectionLineText, ref targetHeadingText);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }

        private void subMoveHeadingText_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateSubObservePosition1(subMoveHeadingText, odometer1Text, odometer2Text, ref observePoint1PositionText);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection1Text, observePoint1PositionText, ref directionLine1, ref targetDirectionLine1Text);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection2Text, observePoint1PositionText, ref directionLine2, ref targetDirectionLine2Text);
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection3Text, observePoint1PositionText, ref directionLine3, ref targetDirectionLine3Text);
            ObservationOperation.CalculateRandomTargetLine(directionLine1, directionLine2, directionLine3, ref targetRandomLine, ref randomTargetLineText, ref targetLineLengthDiffText, ref randomTargetLineCalculationMethodText);
            ObservationOperation.CalculateEstimatedDirectionLine(directionLine1, directionLine2, directionLine3, targetRandomLine, observePoint1PositionText, observePos1TimeDiffText, observePos2TimeDiffText, ref estimatedDirectionLine, ref estimatedDirectionLineText, ref targetHeadingText);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }


        #endregion

        #region direction on change
        private void targetDirection1Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection1Text, observePoint1PositionText, ref directionLine1, ref targetDirectionLine1Text);
            ObservationOperation.CalculateRandomTargetLine(directionLine1, directionLine2, directionLine3, ref targetRandomLine, ref randomTargetLineText, ref targetLineLengthDiffText, ref randomTargetLineCalculationMethodText);
            ObservationOperation.CalculateEstimatedDirectionLine(directionLine1, directionLine2, directionLine3, targetRandomLine, observePoint1PositionText, observePos1TimeDiffText, observePos2TimeDiffText, ref estimatedDirectionLine, ref estimatedDirectionLineText, ref targetHeadingText);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }

        private void targetDirection2Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection2Text, observePoint1PositionText, ref directionLine2, ref targetDirectionLine2Text);
            ObservationOperation.CalculateRandomTargetLine(directionLine1, directionLine2, directionLine3, ref targetRandomLine, ref randomTargetLineText, ref targetLineLengthDiffText, ref randomTargetLineCalculationMethodText);
            ObservationOperation.CalculateEstimatedDirectionLine(directionLine1, directionLine2, directionLine3, targetRandomLine, observePoint1PositionText, observePos1TimeDiffText, observePos2TimeDiffText, ref estimatedDirectionLine, ref estimatedDirectionLineText, ref targetHeadingText);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }

        private void targetDirection3Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateDirectionLine(subPos1HeadingText, targetDirection3Text, observePoint1PositionText, ref directionLine3, ref targetDirectionLine3Text);
            ObservationOperation.CalculateRandomTargetLine(directionLine1, directionLine2, directionLine3, ref targetRandomLine, ref randomTargetLineText, ref targetLineLengthDiffText, ref randomTargetLineCalculationMethodText);
            ObservationOperation.CalculateEstimatedDirectionLine(directionLine1, directionLine2, directionLine3, targetRandomLine, observePoint1PositionText, observePos1TimeDiffText, observePos2TimeDiffText, ref estimatedDirectionLine, ref estimatedDirectionLineText, ref targetHeadingText);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }

        private void subPos2HeadingText_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateDirectionLine(subPos2HeadingText, targetDirection4Text, observePoint1PositionText, ref directionLine4, ref targetDirectionLine4Text, isObservePos1DirectionLine: false);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }

        private void targetDirection4Text_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateDirectionLine(subPos2HeadingText, targetDirection4Text, observePoint1PositionText, ref directionLine4, ref targetDirectionLine4Text, isObservePos1DirectionLine: false);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }
        
        private void observePos1TimeDiffText_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateEstimatedDirectionLine(directionLine1, directionLine2, directionLine3, targetRandomLine, observePoint1PositionText, observePos1TimeDiffText, observePos2TimeDiffText, ref estimatedDirectionLine, ref estimatedDirectionLineText, ref targetHeadingText);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }

        private void observePos2TimeDiffText_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalculateEstimatedDirectionLine(directionLine1, directionLine2, directionLine3, targetRandomLine, observePoint1PositionText, observePos1TimeDiffText, observePos2TimeDiffText, ref estimatedDirectionLine, ref estimatedDirectionLineText, ref targetHeadingText);
            ObservationOperation.CalculateActualTargetLine(targetRandomLine, estimatedDirectionLine, directionLine1, directionLine2, directionLine3, directionLine4, observePos1TimeDiffText, ref targetActualLine, ref actualTargetLineText, ref targetConfirmedPos, ref targetConfirmedPosDate, ref targetSpeedText);
        }
        #endregion

        #region attack parameter on change
        private void attackDirectionText_TextChanged(object sender, EventArgs e)
        {
            ObservationOperation.CalcualteAttackParameter(targetActualLine, targetHeadingText, subPos2HeadingText, attackDirectionText, ref aobText, ref rangeText);
        }
        #endregion
    }
}
