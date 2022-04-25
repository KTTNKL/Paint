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
            var solid = new double[] { };
            var dash = new double[] { 6, 1 };
            var dot = new double[] { 1, 1 };
            var dash_dot_dot = new double[] { 4, 1, 1, 1, 1, 1 };

            var ellipse = shape as EllipseEntity;
            int color = ellipse.color;
            int thickness = ellipse.thickness;
            int stroke_type = ellipse.stroke_type;
            double width = 0;
            double height = 0;
            Ellipse element = null;
            element = new Ellipse()
            {
                Width = width,
                Height = height,
                StrokeThickness = thickness,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeDashArray = new DoubleCollection(solid)
            };

            if (stroke_type == 1)
            {
                element.StrokeDashArray = new DoubleCollection(dash);
            }
            if (stroke_type == 2)
            {
                element.StrokeDashArray = new DoubleCollection(dot);
            }
            if (stroke_type == 3)
            {
                element.StrokeDashArray = new DoubleCollection(dash_dot_dot);
            }

            if (color == 1)
            {
                element.Stroke = new SolidColorBrush(Colors.Red);

            }
            if (color == 2)
            {
                element.Stroke = new SolidColorBrush(Colors.Green);
            }
            if (color == 3)
            {
                element.Stroke = new SolidColorBrush(Colors.Blue);
            }

            if (ellipse.RightBottom.X - ellipse.TopLeft.X >= 0 && ellipse.RightBottom.Y - ellipse.TopLeft.Y >= 0)
            {
                width = ellipse.RightBottom.X - ellipse.TopLeft.X;
                height = ellipse.RightBottom.Y - ellipse.TopLeft.Y;
                element.Width = width;
                element.Height = height;
                Canvas.SetLeft(element, ellipse.TopLeft.X);
                Canvas.SetTop(element, ellipse.TopLeft.Y);
            }
            else if (ellipse.RightBottom.X - ellipse.TopLeft.X <= 0 && ellipse.RightBottom.Y - ellipse.TopLeft.Y >= 0)
            {
                width = -ellipse.RightBottom.X + ellipse.TopLeft.X;
                height = ellipse.RightBottom.Y - ellipse.TopLeft.Y;
                element.Width = width;
                element.Height = height;
                Canvas.SetLeft(element, ellipse.RightBottom.X);
                Canvas.SetTop(element, ellipse.TopLeft.Y);
            }

            else if (ellipse.RightBottom.X - ellipse.TopLeft.X <= 0 && ellipse.RightBottom.Y - ellipse.TopLeft.Y <= 0)
            {
                width = -ellipse.RightBottom.X + ellipse.TopLeft.X;
                height = -ellipse.RightBottom.Y + ellipse.TopLeft.Y;
                element.Width = width;
                element.Height = height;
                Canvas.SetLeft(element, ellipse.RightBottom.X);
                Canvas.SetTop(element, ellipse.RightBottom.Y);
            }
            else
            {
                width = ellipse.RightBottom.X - ellipse.TopLeft.X;
                height = -ellipse.RightBottom.Y + ellipse.TopLeft.Y;
                element.Width = width;
                element.Height = height;
                Canvas.SetLeft(element, ellipse.TopLeft.X);
                Canvas.SetTop(element, ellipse.RightBottom.Y);

            }
            return element;
        }

        public int getColor(IShapeEntity entity)
        {
            var ellipse = entity as EllipseEntity;
            return ellipse.color;
        }

        public int getThickness(IShapeEntity entity)
        {
            var ellipse = entity as EllipseEntity;
            return ellipse.thickness;
        }


        public int getStrokeType(IShapeEntity entity)
        {
            var ellipse = entity as EllipseEntity;
            return ellipse.stroke_type;
        }

        public double getX1(IShapeEntity entity)
        {
            var ellipse = entity as EllipseEntity;

            // TODO: chú ý việc đảo lại rightbottom và topleft 
            return (double)(ellipse.TopLeft.X);
            
        }

        public double getX2(IShapeEntity entity)
        {
            var ellipse = entity as EllipseEntity;

            // TODO: chú ý việc đảo lại rightbottom và topleft 
            return (double)(ellipse.RightBottom.X);
            
        }

        public double getY1(IShapeEntity entity)
        {
            var ellipse = entity as EllipseEntity;

            // TODO: chú ý việc đảo lại rightbottom và topleft 
            return (double)(ellipse.TopLeft.Y);
        }

        public double getY2(IShapeEntity entity)
        {
            var ellipse = entity as EllipseEntity;

            // TODO: chú ý việc đảo lại rightbottom và topleft 
            return (double)(ellipse.RightBottom.Y);
        }

        public void setColor(IShapeEntity entity, int color)
        {
            var ellipse = entity as EllipseEntity;
            ellipse.color = color;
        }

        public void setThickness(IShapeEntity entity, int thickness)
        {
            var ellipse = entity as EllipseEntity;
            ellipse.thickness = thickness;
        }

        public void setStrokeType(IShapeEntity entity, int stroketype)
        {
            var ellipse = entity as EllipseEntity;
            ellipse.stroke_type = stroketype;
        }
    }

}
