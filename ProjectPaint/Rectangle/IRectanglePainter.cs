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
            var solid = new double[] {  };
            var dash = new double[] { 6, 1 };
            var dot = new double[] { 1, 1 };
            var dash_dot_dot = new double[] { 4, 1, 1, 1, 1, 1 };


            var rectangle = shape as RectangleEntity;
            int color = rectangle.color;
            int thickness = rectangle.thickness;
            int stroke_type = rectangle.stroke_type;
            double width = 0;
            double height = 0;
            Rectangle element = null;
            element = new Rectangle()
            {
                Width = width,
                Height = height,
                StrokeThickness = thickness,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeDashArray = new DoubleCollection(solid)
            };

            if(stroke_type == 1)
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

            if (rectangle.RightBottom.X - rectangle.TopLeft.X >= 0 && rectangle.RightBottom.Y - rectangle.TopLeft.Y >= 0)
            {
                width = rectangle.RightBottom.X - rectangle.TopLeft.X;
                height = rectangle.RightBottom.Y - rectangle.TopLeft.Y;
                element.Width = width;
                element.Height = height;
                Canvas.SetLeft(element, rectangle.TopLeft.X);
                Canvas.SetTop(element, rectangle.TopLeft.Y);
            }
            else if (rectangle.RightBottom.X - rectangle.TopLeft.X <= 0 && rectangle.RightBottom.Y - rectangle.TopLeft.Y >= 0)
            {
                width = -rectangle.RightBottom.X + rectangle.TopLeft.X;
                height = rectangle.RightBottom.Y - rectangle.TopLeft.Y;
                element.Width = width;
                element.Height = height;
                Canvas.SetLeft(element, rectangle.RightBottom.X);
                Canvas.SetTop(element, rectangle.TopLeft.Y);
            }

            else if (rectangle.RightBottom.X - rectangle.TopLeft.X <= 0 && rectangle.RightBottom.Y - rectangle.TopLeft.Y <= 0)
            {
                width = -rectangle.RightBottom.X + rectangle.TopLeft.X;
                height = -rectangle.RightBottom.Y + rectangle.TopLeft.Y;
                element.Width = width;
                element.Height = height;
                Canvas.SetLeft(element, rectangle.RightBottom.X);
                Canvas.SetTop(element, rectangle.RightBottom.Y);
            }
            else
            {
                width = rectangle.RightBottom.X - rectangle.TopLeft.X;
                height = -rectangle.RightBottom.Y + rectangle.TopLeft.Y;
                element.Width = width;
                element.Height = height;
                Canvas.SetLeft(element, rectangle.TopLeft.X);
                Canvas.SetTop(element, rectangle.RightBottom.Y);

            }
            return element;
        }


        public int getColor(IShapeEntity entity)
        {
            var rectangle = entity as RectangleEntity;
            return rectangle.color;
        }

        public int getThickness(IShapeEntity entity)
        {
            var rectangle = entity as RectangleEntity;
            return rectangle.thickness;
        }


        public int getStrokeType(IShapeEntity entity)
        {
            var rectangle = entity as RectangleEntity;
            return rectangle.stroke_type;
        }

        public double getX1(IShapeEntity entity)
        {
            var rectangle = entity as RectangleEntity;

            // TODO: chú ý việc đảo lại rightbottom và topleft 
            return (double)(rectangle.TopLeft.X);
        }

        public double getX2(IShapeEntity entity)
        {
            var rectangle = entity as RectangleEntity;

            // TODO: chú ý việc đảo lại rightbottom và topleft 
            return (double)(rectangle.RightBottom.X);

        }

        public double getY1(IShapeEntity entity)
        {
            var rectangle = entity as RectangleEntity;

            // TODO: chú ý việc đảo lại rightbottom và topleft 
            return (double)(rectangle.TopLeft.Y);
        }

        public double getY2(IShapeEntity entity)
        {
            var rectangle = entity as RectangleEntity;

            // TODO: chú ý việc đảo lại rightbottom và topleft 
        
            return (double)(rectangle.RightBottom.Y);
        }

        public void setColor(IShapeEntity entity, int color)
        {
            var rectangle = entity as RectangleEntity;
            rectangle.color = color; 
        }

        public void setThickness(IShapeEntity entity, int thickness)
        {
            var rectangle = entity as RectangleEntity;
            rectangle.thickness = thickness;
        }

        public void setStrokeType(IShapeEntity entity, int stroketype)
        {
            var rectangle = entity as RectangleEntity;
            rectangle.stroke_type = stroketype;
        }
    }
}
