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
        int getColor(IShapeEntity entity);
        int getStrokeType(IShapeEntity entity);

        void setColor(IShapeEntity entity, int color);

        void setThickness(IShapeEntity entity, int thickness);

        void setStrokeType(IShapeEntity entity, int stroketype);

    }
}
