using IContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RectangleEntity
{
    public class IRectanglePainter : IPaintBusiness
    {
        public UIElement Draw(IShapeEntity shape)
        {
            var rectangle = shape as RectangleEntity;
            double width = 0;
            double height = 0;
            Rectangle element = null;
            if (rectangle.RightBottom.X - rectangle.TopLeft.X >= 0 && rectangle.RightBottom.Y - rectangle.TopLeft.Y >= 0)
            {
                width = rectangle.RightBottom.X - rectangle.TopLeft.X;
                height = rectangle.RightBottom.Y - rectangle.TopLeft.Y;
                element = new Rectangle()
                {
                    Width = width,
                    Height = height,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Red)
                };
                Canvas.SetLeft(element, rectangle.TopLeft.X);
                Canvas.SetTop(element, rectangle.TopLeft.Y);
            }
            else if (rectangle.RightBottom.X - rectangle.TopLeft.X <= 0 && rectangle.RightBottom.Y - rectangle.TopLeft.Y >= 0)
            {
                width = -rectangle.RightBottom.X + rectangle.TopLeft.X;
                height = rectangle.RightBottom.Y - rectangle.TopLeft.Y;
                element = new Rectangle()
                {
                    Width = width,
                    Height = height,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Red)
                };
                Canvas.SetLeft(element, rectangle.RightBottom.X);
                Canvas.SetTop(element, rectangle.TopLeft.Y);
            }

            else if (rectangle.RightBottom.X - rectangle.TopLeft.X <= 0 && rectangle.RightBottom.Y - rectangle.TopLeft.Y <= 0)
            {
                width = -rectangle.RightBottom.X + rectangle.TopLeft.X;
                height = -rectangle.RightBottom.Y + rectangle.TopLeft.Y;
                element = new Rectangle()
                {
                    Width = width,
                    Height = height,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Red)
                };
                Canvas.SetLeft(element, rectangle.RightBottom.X);
                Canvas.SetTop(element, rectangle.RightBottom.Y);
            }
            else
            {
                width = rectangle.RightBottom.X - rectangle.TopLeft.X;
                height = -rectangle.RightBottom.Y + rectangle.TopLeft.Y;
                element = new Rectangle()
                {
                    Width = width,
                    Height = height,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Red)
                };
                Canvas.SetLeft(element, rectangle.TopLeft.X);
                Canvas.SetTop(element, rectangle.RightBottom.Y);

            }
            return element;
        }
    }
}
