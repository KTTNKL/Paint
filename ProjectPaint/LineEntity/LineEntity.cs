using IContract;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LineEntity
{
    public class LineEntity : IShapeEntity, ICloneable
    {
        public Point Start { get; set; }
        public Point End { get; set; }


        public string Name => "Line";

        public BitmapImage Icon => new BitmapImage(new Uri("", UriKind.Relative));
        public int color { get; set; }
        public int thickness { get; set; }
        public int stroke_type { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }

        public void HandleEnd(Point point)
        {
            End = point;
        }

        public void HandleStart(Point point)
        {
            Start = point;
        }
    }
}
