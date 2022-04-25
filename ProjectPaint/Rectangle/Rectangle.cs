using IContract;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RectangleEntity
{
    public class RectangleEntity: IShapeEntity, ICloneable
    {
        public Point TopLeft { get; set; }
        public Point RightBottom { get; set; }

        public string Name => "Rectangle";

        public BitmapImage Icon => throw new NotImplementedException();

        public int color { get ; set ; }
        public int thickness { get ; set; }
        public int stroke_type { get ; set; }

        public void HandleStart(Point point)
        {
            TopLeft = point;
        }
        public void HandleEnd(Point point)
        {
            RightBottom = point;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
