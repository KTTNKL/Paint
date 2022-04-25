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

            // TODO: chú ý việc đảo lại rightbottom và topleft 
            double width = ellipse.RightBottom.X - ellipse.TopLeft.X;
            double height = ellipse.RightBottom.Y - ellipse.TopLeft.Y;

            var element = new Ellipse()
            {
                Width = width,
                Height = height,
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Red)
            };
            Canvas.SetLeft(element, ellipse.TopLeft.X);
            Canvas.SetTop(element, ellipse.TopLeft.Y);

            return element;
        }
       
    }

}
