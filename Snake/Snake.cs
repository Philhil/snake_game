using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Snake
    {
        public enum Direction
        {
            up,
            down,
            left,
            right
        }

        public int snakeSize { get; set; }
        public List<Point> snakeBody { get; set; }
        public Brush snakeColor { get; set; }
        public Direction currentDirection{ get; set; }
        private bool _addnewhead { get; set; }
        
        public Snake()
        {
            snakeSize = 0;
            snakeColor = Brushes.Black;
            snakeBody = new List<Point>();
            currentDirection = Direction.down;

            //First Body Point (head)
            Point p = new Point(10,10);
            snakeBody.Add(p);
        }

        public void move()
        {
            Point head = snakeBody.First();
            int aheadX = head.X;
            int aheadY = head.Y;

            switch (currentDirection)
            {
                case Direction.up:
                    aheadY -= 1;
                    break;
                case Direction.down:
                    aheadY += 1;
                    break;
                case Direction.left:
                    aheadX -= 1;
                    
                    break;
                case Direction.right:
                    aheadX += 1;
                    break;
                default:
                    break;
            }

            //if goodie was found a new head will be added instead of moving whole snake
            if (_addnewhead)
            {
                addNewHead(aheadX, aheadY);
            }
            else
            {
                //move all points of body one ahead
                //TODO: move only last element to new head (Performance)
                foreach (Point point in snakeBody)
                {
                    int currentX_temp = point.X;
                    int currentY_temp = point.Y;

                    //move to position of ahead
                    point.move(aheadX, aheadY);

                    aheadX = currentX_temp;
                    aheadY = currentY_temp;
                }
            }
            
        }

        public void addNewHead()
        {
            _addnewhead = true;
        }

        private void addNewHead(int aheadX, int aheadY)
        {
            Point head = snakeBody.First();
            head.color = snakeColor;
            Point newHead = new Point(aheadX, aheadY, Brushes.Red);
            snakeBody.Insert(0, newHead);
            _addnewhead = false;
        }
    }
}
