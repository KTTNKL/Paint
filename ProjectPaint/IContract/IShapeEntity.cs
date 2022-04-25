using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IContract
{
    public interface IShapeEntity: ICloneable
    {
        string Name { get; }
        
        int color { get; set; }
        int thickness { get; set; }
        int stroke_type { get; set; }

        BitmapImage Icon { get; }

        void HandleStart(Point point);
        void HandleEnd(Point point);
    }
}
