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
            var line = shape as LineEntity;

            var element = new Line()
            {
                X1 = line.Start.X,
                Y1 = line.Start.Y,
                X2 = line.End.X,
                Y2 = line.End.Y,
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Black)
            };

            return element;
        }

        public string getColor(IShapeEntity entity)
        {
            return "black";
        }

        public int getThickness(IShapeEntity entity)
        {
            return 1;
        }

        public float getWidth(IShapeEntity entity)
        {
            throw new NotImplementedException();
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
    }
}
