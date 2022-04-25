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

namespace EllipseEntity
{
    class EllipsePainter : IPaintBusiness
    {
        public UIElement Draw(IShapeEntity shape)
        {
            var ellipse = shape as EllipseEntity;
            double width = 0;
            double height = 0;
            Ellipse element = null;
            if (ellipse.RightBottom.X - ellipse.TopLeft.X >= 0 && ellipse.RightBottom.Y - ellipse.TopLeft.Y >= 0)
            {
                width = ellipse.RightBottom.X - ellipse.TopLeft.X;
                height = ellipse.RightBottom.Y - ellipse.TopLeft.Y;
                element = new Ellipse()
                {
                    Width = width,
                    Height = height,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Red)
                };
                Canvas.SetLeft(element, ellipse.TopLeft.X);
                Canvas.SetTop(element, ellipse.TopLeft.Y);
            }
            else if (ellipse.RightBottom.X - ellipse.TopLeft.X <= 0 && ellipse.RightBottom.Y - ellipse.TopLeft.Y >= 0)
            {
                width = -ellipse.RightBottom.X + ellipse.TopLeft.X;
                height = ellipse.RightBottom.Y - ellipse.TopLeft.Y;
                element = new Ellipse()
                {
                    Width = width,
                    Height = height,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Red)
                };
                Canvas.SetLeft(element, ellipse.RightBottom.X);
                Canvas.SetTop(element, ellipse.TopLeft.Y);
            }

            else if (ellipse.RightBottom.X - ellipse.TopLeft.X <= 0 && ellipse.RightBottom.Y - ellipse.TopLeft.Y <= 0)
            {
                width = -ellipse.RightBottom.X + ellipse.TopLeft.X;
                height = -ellipse.RightBottom.Y + ellipse.TopLeft.Y;
                element = new Ellipse()
                {
                    Width = width,
                    Height = height,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Red)
                };
                Canvas.SetLeft(element, ellipse.RightBottom.X);
                Canvas.SetTop(element, ellipse.RightBottom.Y);
            }
            else
            {
                width = ellipse.RightBottom.X - ellipse.TopLeft.X;
                height = -ellipse.RightBottom.Y + ellipse.TopLeft.Y;
                element = new Ellipse()
                {
                    Width = width,
                    Height = height,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.Red)
                };
                Canvas.SetLeft(element, ellipse.TopLeft.X);
                Canvas.SetTop(element, ellipse.RightBottom.Y);

            }
            return element;
        }
       
    }

}
