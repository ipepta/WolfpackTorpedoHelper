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
    public partial class NauticalChartForm : Form
    {
        // (0, graphicMaxLength)
        private const int maxGraphicSystemLength = 800;
        // (-coordinateMaxLength, coordinateMaxLength)
        private const int maxCoordinateSystemLength = 6000;
        private const int axisMarkHeight = 100;
        private const double entityHeight = 200;
        private const double positionHeight = 200;

        private DirectionDistanceAimingForm directionDistanceAimingForm;
        private DirectionAimingForm directionAimingForm;
        private Thread paintThread;


        public NauticalChartForm(DirectionDistanceAimingForm directionDistanceAimingForm, DirectionAimingForm directionAimingForm)
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            this.directionDistanceAimingForm = directionDistanceAimingForm;
            this.directionAimingForm = directionAimingForm;
            this.paintThread = new Thread(new ThreadStart(PaintNauticalChartThread));
            this.paintThread.Start();
        }

        private void NauticalChartForm_Paint(object sender, PaintEventArgs e)
        {
            PaintNauticalChart();
        }

        private void PaintNauticalChartThread()
        {
            while (true)
            {
                PaintNauticalChart();
                Thread.Sleep(1000);
            }
        }

        private void PaintNauticalChart()
        {
            Graphics graphic = this.CreateGraphics();
            graphic.Clear(Color.LightGray);

            PaintCoordinateSystem(ref graphic);

            #region direction distance aiming form
            if (!string.IsNullOrEmpty(directionDistanceAimingForm.subHeadingText.Text))
            {
                try
                {
                    var heading = Convert.ToDouble(directionDistanceAimingForm.subHeadingText.Text);
                    PaintEntity(ref graphic, ChartEntityType.SUB, 0, 0, heading);
                }
                catch
                { }
            }

            if (!string.IsNullOrEmpty(directionDistanceAimingForm.targetPosition1Text.Text))
            {
                PointModel point = PointModel.Parse(directionDistanceAimingForm.targetPosition1Text.Text);
                if (point != null)
                {
                    PaintPosition(ref graphic, point);
                }
            }

            if (!string.IsNullOrEmpty(directionDistanceAimingForm.targetPosition2Text.Text))
            {
                PointModel point = PointModel.Parse(directionDistanceAimingForm.targetPosition2Text.Text);
                if (point != null)
                {
                    PaintPosition(ref graphic, point);
                }
            }

            if (!string.IsNullOrEmpty(directionDistanceAimingForm.targetPosition3Text.Text))
            {
                PointModel point = PointModel.Parse(directionDistanceAimingForm.targetPosition3Text.Text);
                if (point != null)
                {
                    PaintPosition(ref graphic, point);
                }
            }

            if (directionDistanceAimingForm.targetLine != null)
            {
                PaintTargetLine(ref graphic, directionDistanceAimingForm.targetLine, ChartEntityType.TARGET);
            }
            
            if (!string.IsNullOrEmpty(directionDistanceAimingForm.targetEstimatedPositionText.Text) && !string.IsNullOrEmpty(directionDistanceAimingForm.targetHeadingText.Text))
            {
                var targetPos = PointModel.Parse(directionDistanceAimingForm.targetEstimatedPositionText.Text);
                if (targetPos != null)
                {
                    try
                    {
                        var targetHeading = Convert.ToDouble(directionDistanceAimingForm.targetHeadingText.Text);
                        PaintEntity(ref graphic, ChartEntityType.TARGET, targetPos, targetHeading);
                    }
                    catch
                    { }
                }
            }
            
            if (directionDistanceAimingForm.targetLine != null && !string.IsNullOrEmpty(directionDistanceAimingForm.subHeadingText.Text) && !string.IsNullOrEmpty(directionDistanceAimingForm.attackDirectionText.Text))
            {
                try
                {
                    var subHeading = Convert.ToDouble(directionDistanceAimingForm.subHeadingText.Text);
                    var attackAngle = Convert.ToDouble(directionDistanceAimingForm.attackDirectionText.Text);
                    PaintAttackLine(ref graphic, directionDistanceAimingForm.targetLine, subHeading, attackAngle);
                }
                catch
                { }
            }
            #endregion

            #region direction aiming form
            if(!string.IsNullOrEmpty(directionAimingForm.subPos1HeadingText.Text))
            {
                try
                {
                    double heading = Convert.ToDouble(directionAimingForm.subPos1HeadingText.Text);
                    PaintEntity(ref graphic, ChartEntityType.SUB, new PointModel(0, 0), heading);
                }
                catch
                {  }
            }

            if (directionAimingForm.directionLine1 != null)
            {
                Pen pen = new Pen(Color.Black, 2);
                DrawLineByLine(ref graphic, pen, directionAimingForm.directionLine1);
                pen.Dispose();
            }

            if (directionAimingForm.directionLine2 != null)
            {
                Pen pen = new Pen(Color.Black, 2);
                DrawLineByLine(ref graphic, pen, directionAimingForm.directionLine2);
                pen.Dispose();
            }

            if (directionAimingForm.directionLine3 != null)
            {
                Pen pen = new Pen(Color.Black, 2);
                DrawLineByLine(ref graphic, pen, directionAimingForm.directionLine3);
                pen.Dispose();
            }

            if (directionAimingForm.directionLine4 != null)
            {
                Pen pen = new Pen(Color.Black, 2);
                DrawLineByLine(ref graphic, pen, directionAimingForm.directionLine4);
                pen.Dispose();
            }

            if (directionAimingForm.targetRandomLine != null)
            {
                Pen pen = new Pen(Color.Blue, 2);
                DrawLineByLine(ref graphic, pen, directionAimingForm.targetRandomLine);
                pen.Dispose();
            }

            if (directionAimingForm.estimatedDirectionLine != null)
            {
                try
                {
                    Pen pen = new Pen(Color.Blue, 2);
                    DrawLineByLine(ref graphic, pen, directionAimingForm.estimatedDirectionLine);
                    pen.Dispose();
                }
                catch
                { }
            }

            if (directionAimingForm.targetActualLine != null)
            {
                try
                {
                    Pen pen = new Pen(Color.Black, 2);
                    DrawLineByLine(ref graphic, pen, directionAimingForm.targetActualLine);
                    pen.Dispose();
                }
                catch
                {  }
            }

            if (!string.IsNullOrEmpty(directionAimingForm.observePoint1PositionText.Text))
            {
                var observePoint1 = PointModel.Parse(directionAimingForm.observePoint1PositionText.Text);
                if (observePoint1 != null)
                {
                    PaintPosition(ref graphic, observePoint1);
                }
            }

            if(directionAimingForm.targetConfirmedPos != null)
            {
                PaintPosition(ref graphic, directionAimingForm.targetConfirmedPos);
            }

            if (!string.IsNullOrEmpty(directionAimingForm.targetEstimatedPosText.Text) && !string.IsNullOrEmpty(directionAimingForm.targetHeadingText.Text))
            {
                var targetPos = PointModel.Parse(directionAimingForm.targetEstimatedPosText.Text);
                if (targetPos != null)
                {
                    try
                    {
                        var targetHeading = Convert.ToDouble(directionAimingForm.targetHeadingText.Text);
                        PaintEntity(ref graphic, ChartEntityType.TARGET, targetPos, targetHeading);
                    }
                    catch
                    { }
                }
            }

            if (directionAimingForm.targetActualLine != null && !string.IsNullOrEmpty(directionAimingForm.subPos1HeadingText.Text) && !string.IsNullOrEmpty(directionAimingForm.attackDirectionText.Text))
            {
                try
                {
                    var subHeading = Convert.ToDouble(directionAimingForm.subPos1HeadingText.Text);
                    var attackAngle = Convert.ToDouble(directionAimingForm.attackDirectionText.Text);
                    PaintAttackLine(ref graphic, directionAimingForm.targetActualLine, subHeading, attackAngle);
                }
                catch
                { }
            }
            #endregion


            #region test paints

            #endregion

            graphic.Dispose();
        }

        private void PaintAttackLine(ref Graphics graphic, LineModel targetLine, double subHeading, double attackAngle)
        {
            Pen pen = new Pen(Color.Red, 2);
            var actualAttackAngle = DirectionOperation.CalculateSubActualDirection(subHeading, attackAngle);
            var attackLine = CoordinateSystemOperation.CalculateLineFromPointAndDirection(new PointModel(0, 0), actualAttackAngle);
            var attackInterceptPoint = CoordinateSystemOperation.CalculatePointByTwoLines(targetLine, attackLine);

            DrawLineByPoint(ref graphic, pen, new PointModel(0, 0), attackInterceptPoint);
            pen.Dispose();
        }

        private void PaintTargetLine(ref Graphics graphic, LineModel targetLine, ChartEntityType targetType)
        {
            Pen pen = new Pen(Color.Black, 2);
            if (targetType == ChartEntityType.TARGET)
            {
                pen = new Pen(Color.Black, 2);
            }
            else if (targetType == ChartEntityType.TARGET2)
            {
                pen = new Pen(Color.DarkOrange, 2);
            }
            DrawLineByLine(ref graphic, pen, targetLine);
            pen.Dispose();
        }

        private void PaintPosition(ref Graphics graphic, PointModel point)
        {
            PaintPosition(ref graphic, point.x, point.y);
        }

        private void PaintPosition(ref Graphics graphic, double x, double y)
        {
            Pen pen = new Pen(Color.Red, 2);

            DrawLineByCoordinate(ref graphic, pen, x - positionHeight / 2, y, x + positionHeight / 2, y);
            DrawLineByCoordinate(ref graphic, pen, x, y - positionHeight / 2, x, y + positionHeight / 2);

            pen.Dispose();
        }

        private void PaintEntity(ref Graphics graphic, ChartEntityType entityType, PointModel point, double heading)
        {
            PaintEntity(ref graphic, entityType, point.x, point.y, heading);
        }

        private void PaintEntity(ref Graphics graphic, ChartEntityType entityType, double x, double y, double heading)
        {
            var entity = CoordinateSystemOperation.CalculateEntityPoints(new PointModel(x, y), heading, entityHeight);

            Pen pen = new Pen(Color.Black, 2);
            if (entityType == ChartEntityType.SUB)
            {
                pen = new Pen(Color.Blue, 2);
            }
            else if (entityType == ChartEntityType.TARGET)
            {
                pen = new Pen(Color.Black, 2);
            }
            else if (entityType == ChartEntityType.TARGET2)
            {
                pen = new Pen(Color.DarkOrange, 2);
            }

            DrawLineByPoint(ref graphic, pen, entity.headPoint, entity.bottomLeftPoint);
            DrawLineByPoint(ref graphic, pen, entity.headPoint, entity.bottomRightPoint);
            DrawLineByPoint(ref graphic, pen, entity.bottomLeftPoint, entity.bottomRightPoint);

            //DrawLineByLine(ref graphic, pen, entity.centerLine);
            //DrawLineByLine(ref graphic, tempPen, entity.bottomLine);

            pen.Dispose();
        }

        private void PaintCoordinateSystem(ref Graphics graphic)
        {
            Pen axisPen = new Pen(Color.Black, 2);
            DrawLineByCoordinate(ref graphic, axisPen, -maxCoordinateSystemLength, 0, maxCoordinateSystemLength, 0);
            DrawLineByCoordinate(ref graphic, axisPen, 0, -maxCoordinateSystemLength, 0, maxCoordinateSystemLength);

            Pen axisMarkPen = new Pen(Color.Black, 1);
            for (int i = maxCoordinateSystemLength * -1; i <= maxCoordinateSystemLength; i += 1000)
            {
                DrawLineByCoordinate(ref graphic, axisMarkPen, i, axisMarkHeight, i, axisMarkHeight * -1);
                DrawLineByCoordinate(ref graphic, axisMarkPen, axisMarkHeight * -1, i, axisMarkHeight, i);
            }
            for (int i = maxCoordinateSystemLength * -1 + 500; i <= maxCoordinateSystemLength; i += 1000)
            {
                DrawLineByCoordinate(ref graphic, axisMarkPen, i, axisMarkHeight / 2, i, axisMarkHeight * -1 / 2);
                DrawLineByCoordinate(ref graphic, axisMarkPen, axisMarkHeight * -1 / 2, i, axisMarkHeight / 2, i);
            }

            axisPen.Dispose();
            axisMarkPen.Dispose();
        }

        private void DrawLineByLine(ref Graphics graphic, Pen pen, LineModel line)
        {
            var maxY = line.CalculateY(maxCoordinateSystemLength);
            PointModel maxNegativePoint, maxPositivePoint;
            if (Math.Abs(maxY) > maxCoordinateSystemLength)
            {
                maxNegativePoint = new PointModel(line.CalculateX(-1 * maxCoordinateSystemLength), -1 * maxCoordinateSystemLength);
                maxPositivePoint = new PointModel(line.CalculateX(maxCoordinateSystemLength), maxCoordinateSystemLength);
            }
            else
            {
                maxNegativePoint = new PointModel(-1 * maxCoordinateSystemLength, line.CalculateY(-1 * maxCoordinateSystemLength));
                maxPositivePoint = new PointModel(maxCoordinateSystemLength, line.CalculateY(maxCoordinateSystemLength));
            }

            DrawLineByPoint(ref graphic, pen, maxNegativePoint, maxPositivePoint);
        }

        private void DrawLineByPoint(ref Graphics graphic, Pen pen, PointModel point1, PointModel point2)
        {
            DrawLineByCoordinate(ref graphic, pen, point1.x, point1.y, point2.x, point2.y);
        }

        private void DrawLineByCoordinate(ref Graphics graphic, Pen pen, double x1, double y1, double x2, double y2)
        {
            var pointA = GetGraphicPointByCoordinate(x1, y1);
            var pointB = GetGraphicPointByCoordinate(x2, y2);

            graphic.DrawLine(pen, pointA, pointB);
        }

        private Point GetGraphicPointByCoordinate(double x, double y)
        {
            int graphicX = TranslateCoordinateXToGraphic(x);
            int graphicY = TranslateCoordinateYToGraphic(y);

            return new Point(graphicX, graphicY);
        }

        private int TranslateCoordinateXToGraphic(double input)
        {
            var result = input + maxCoordinateSystemLength;
            result = result / (2 * maxCoordinateSystemLength) * maxGraphicSystemLength;
            return Convert.ToInt32(Math.Round(result));
        }

        private int TranslateCoordinateYToGraphic(double input)
        {
            var result = input + maxCoordinateSystemLength;
            result = result / (2 * maxCoordinateSystemLength) * maxGraphicSystemLength;
            result = maxGraphicSystemLength - result;
            return Convert.ToInt32(Math.Round(result));
        }

        private void NauticalChartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void NauticalChartForm_Load(object sender, EventArgs e)
        {
            this.Text = "NauticalChartForm " + Configurations.version;
        }
    }

    public enum ChartEntityType
    {
        SUB,
        TARGET,
        TARGET2,
    }
}
