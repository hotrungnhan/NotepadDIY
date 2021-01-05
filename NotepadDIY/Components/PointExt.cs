using System.Drawing;

namespace NotepadDIY.Components.Ext
{
    class PointExt
    {

        //public static Point ime(Point a, Point b)
        //{
        //    Point newp = new Point();
        //    newp.X = a.X + b.X;
        //    newp.Y = a.Y + b.Y;
        //    return newp;
        //}
    }
    class SizeExt
    {
        public static Size Mult(Size s, double mul)
        {
            Size news = new Size();
            news.Width = (int)(s.Width * mul);
            news.Height = (int)(s.Height * mul);
            return news;
        }
    }
}
