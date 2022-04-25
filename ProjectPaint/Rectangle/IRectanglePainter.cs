﻿using IContract;
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
            var rectangle = shape as RectangleEntity;
            double width = 0;
            double height = 0;
            Rectangle element = null;
            element = new Rectangle()
            {
                Width = width,
                Height = height,
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Red)
            };

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

        public string getColor(IShapeEntity entity)
        {
            return "red";
        }

        public int getThickness(IShapeEntity entity)
        {
            return 1;
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
    }
}
