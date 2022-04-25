using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IContract
{
    public interface IPaintBusiness
    {
        UIElement Draw(IShapeEntity entity);

        double getX1(IShapeEntity entity);
        double getX2(IShapeEntity entity);
        double getY1(IShapeEntity entity);
        double getY2(IShapeEntity entity);
        int getThickness(IShapeEntity entity);
        string getColor(IShapeEntity entity);

    }
}
