using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Brush color { get; set; }

        public Point ()
        {
            X = 0;
            Y = 0;
            color = Brushes.Red;
        }

        public Point(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            this.color = Brushes.Red;
        }

        public Point(int X, int Y, Brush color)
        {
            this.X = X;
            this.Y = Y;
            this.color = color;
        }

        public void move(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
