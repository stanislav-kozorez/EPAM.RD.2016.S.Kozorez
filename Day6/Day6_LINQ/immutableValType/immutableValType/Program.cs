using System;

namespace immutableValType
{
    // Point  размерный тип.
    internal interface IChangable
    {
        void Change(int x, int y);
    }


    internal struct Point: IChangable
    {
        private int _x, _y;
        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public void Change(int x, int y)
        {
            _x = x; _y = y;
        }
        public override string ToString()
        {
            return string.Format("({0}, {1})", _x, _y);
        }
    }
    public sealed class Program
    {
        public static void Main()
        {
            Point p = new Point(1, 1);
            Console.WriteLine(p);
            p.Change(2, 2);
            Console.WriteLine(p);
            object o = p;
            IChangable c = p;
            Console.WriteLine(o);
            ((IChangable)p).Change(4, 4);
            ((Point)o).Change(3, 3);
            Console.WriteLine(p);
            Console.ReadLine();
        }
    }
}
