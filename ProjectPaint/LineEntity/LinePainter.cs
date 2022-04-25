using IContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LineEntity
{
    public class LinePainter: IPaintBusiness
    {
        public UIElement Draw(IShapeEntity shape)
        {
            var solid = new double[] { };
            var dash = new double[] { 6, 1 };
            var dot = new double[] { 1, 1 };
            var dash_dot_dot = new double[] { 4, 1, 1, 1, 1, 1 };
            var line = shape as LineEntity;
            int color = line.color;
            int thickness = line.thickness;
            int stroke_type = line.stroke_type;

            var element = new Line()
            {
                X1 = line.Start.X,
                Y1 = line.Start.Y,
                X2 = line.End.X,
                Y2 = line.End.Y,
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

            return element;
        }

        public int getColor(IShapeEntity entity)
        {
            var line = entity as IShapeEntity;
            return line.color;
        }

        public int getThickness(IShapeEntity entity)
        {
            var line = entity as IShapeEntity;
            return line.thickness;
        }


        public int getStrokeType(IShapeEntity entity)
        {
            var line = entity as IShapeEntity;
            return line.stroke_type;
        }


        public double getX1(IShapeEntity entity)
        {
            var line = entity as LineEntity;
            return (float)line.Start.X;
        }

        public double getX2(IShapeEntity entity)
        {
            var line = entity as LineEntity;
            return (float)line.End.X;
        }

        public double getY1(IShapeEntity entity)
        {
            var line = entity as LineEntity;
            return (float)line.Start.Y;
        }

        public double getY2(IShapeEntity entity)
        {
            var line = entity as LineEntity;
            return (float)line.End.Y;
        }

        public void setColor(IShapeEntity entity, int color)
        {
            var line = entity as IShapeEntity;
            line.color = color;
        }

        public void setThickness(IShapeEntity entity, int thickness)
        {
            var line = entity as IShapeEntity;
            line.thickness = thickness;
        }

        public void setStrokeType(IShapeEntity entity, int stroketype)
        {
            var line = entity as IShapeEntity;
            line.stroke_type = stroketype;
        }
    }
}
