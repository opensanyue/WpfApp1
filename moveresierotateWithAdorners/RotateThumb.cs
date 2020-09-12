using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace moveresierotateWithAdorners
{
    public class RotateThumb : Thumb
    {
        private Point centerPoint; //中心点
        private Vector startVector; //起始矢量
        private double initialAngle;  //初始角度
        private Canvas designerCanvas; //设计师画布
        private ContentControl designerItem; //设计师项
        private RotateTransform rotateTransform; //旋转变换

        public RotateThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.RotateThumb_DragDelta);
            DragStarted += new DragStartedEventHandler(this.RotateThumb_DragStarted);
        }

        private void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as ContentControl;

            if (this.designerItem != null)
            {
                //获取画布
                this.designerCanvas = VisualTreeHelper.GetParent(this.designerItem) as Canvas;

                if (this.designerCanvas != null)
                {
                    //获取中心点
                    this.centerPoint = this.designerItem.TranslatePoint(
                        new Point(this.designerItem.Width * this.designerItem.RenderTransformOrigin.X,
                                  this.designerItem.Height * this.designerItem.RenderTransformOrigin.Y),
                                  this.designerCanvas);
                    //获取开始点
                    Point startPoint = Mouse.GetPosition(this.designerCanvas);
                    //获取开始矢量
                    this.startVector = Point.Subtract(startPoint, this.centerPoint);
                    //获取旋转类
                    this.rotateTransform = this.designerItem.RenderTransform as RotateTransform;
                    //设置旋转
                    if (this.rotateTransform == null)
                    {
                        this.designerItem.RenderTransform = new RotateTransform(0);
                        this.initialAngle = 0;
                    }
                    else
                    {
                        this.initialAngle = this.rotateTransform.Angle;
                    }
                }
            }
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null && this.designerCanvas != null)
            {
                Point currentPoint = Mouse.GetPosition(this.designerCanvas);
                Vector deltaVector = Point.Subtract(currentPoint, this.centerPoint);

                double angle = Vector.AngleBetween(this.startVector, deltaVector);

                RotateTransform rotateTransform = this.designerItem.RenderTransform as RotateTransform;
                rotateTransform.Angle = this.initialAngle + Math.Round(angle, 0);
                this.designerItem.InvalidateMeasure();
            }
        }
    }
}
