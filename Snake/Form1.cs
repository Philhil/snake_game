using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace Snake
{
    public partial class Form1 : Form
    {
        enum GameMode
        {
            human,
            wavefront
        }

        Snake _snake;
        Point _goodie;
        System.Timers.Timer _timer;
        GameMode _currentGameMode;
        List<Tuple<int, int>> _path = null;
        bool _isRunning = false;

        public Form1()
        {
            InitializeComponent();

            _currentGameMode = GameMode.wavefront;

            initGame();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    //start/break game
                    if (_currentGameMode == GameMode.human)
                    {
                        if (_timer.Enabled)
                        {
                            _isRunning = false;
                            _timer.Stop();
                        }
                        else
                        {
                            _isRunning = true;
                            _timer.Start();
                        }
                    }
                    else
                    {
                        if (_isRunning)
                        {
                            stopComputerAlgo();
                        }
                        else
                        {
                            runComputerAlgo();
                        }
                    }
                    
                    break;
                case Keys.Left:
                case Keys.A:
                    if (_snake.currentDirection != Snake.Direction.right)
                    {
                        _snake.currentDirection = Snake.Direction.left;
                    }
                    break;
                case Keys.Right:
                case Keys.D:
                    if (_snake.currentDirection != Snake.Direction.left)
                    {
                        _snake.currentDirection = Snake.Direction.right;
                    }
                    break;
                case Keys.Up:
                case Keys.W:
                    if (_snake.currentDirection != Snake.Direction.down)
                    {
                        _snake.currentDirection = Snake.Direction.up;
                    }
                    break;
                case Keys.Down:
                case Keys.S:
                    if (_snake.currentDirection != Snake.Direction.up)
                    {
                        _snake.currentDirection = Snake.Direction.down;
                    }
                    break;
                case Keys.R:
                    _timer.Stop();
                    initGame();
                    break;
                case Keys.H:
                    //TODO: only when game is not running
                    _currentGameMode = GameMode.human;
                    break;
                case Keys.C:
                    //TODO: only when game is not running
                    _currentGameMode = GameMode.wavefront;
                    break;
                default:
                    break;
            }
        }

        private void initGame()
        {
            _snake = new Snake();
            _goodie = createGoodie();

            _timer = new System.Timers.Timer(200);
            _timer.Elapsed += gameStep;
            _timer.Enabled = false;

            pictureBox1.Refresh();
            label_gameOver.Visible = false;
        }

        private void runComputerAlgo()
        {
            _isRunning = true;

            //TODO thread and run gamestep so gui is not freezed
            while (_isRunning)
            {
                gameStep(null, null);
//              System.Threading.Thread.Sleep(10);
            }
        }

        private void stopComputerAlgo()
        {
            _isRunning = false;
        }

        private void gameStep(Object source, ElapsedEventArgs e)
        {
            if (!foundGoodie())
            {
                if (_currentGameMode != GameMode.human)
                {
                    
                    if (_path == null || _path.Count == 0)
                    {
                        _path = wavefront.getRoute(wavefront.createMap(pictureBox1.Width, pictureBox1.Height, _snake), _goodie, _snake.snakeBody.First());
                    }

                    if (_path != null)
                    {

                        //use human move mode and set only direction by next path point
                        Point head = _snake.snakeBody.First();
                        Tuple<int, int> nexthead = _path.First();

                        //left
                        if (head.X > nexthead.Item1)
                        {
                            _snake.currentDirection = Snake.Direction.left;
                        }
                        //right
                        if (nexthead.Item1 > head.X)
                        {
                            _snake.currentDirection = Snake.Direction.right;
                        }

                        //up
                        if (head.Y > nexthead.Item2)
                        {
                            _snake.currentDirection = Snake.Direction.up;
                        }

                        //down
                        if (nexthead.Item2 > head.Y)
                        {
                            _snake.currentDirection = Snake.Direction.down;
                        }

                        _path.RemoveAt(0);
                    }
                    else
                    {
                        //No path found -> simple way: just go further in same direaction and pray to find a way later :)

                        //TODO: proper strategy
                    }
                }

                _snake.move();
            }

            if(hasCollition())
            {
                label_gameOver.Invoke(new MethodInvoker(
                delegate ()
                {
                    label_gameOver.Visible = true;
                }));

                _timer.Stop();
                _isRunning = false;
            }
            else
            {
                pictureBox1.Invoke(new MethodInvoker(
                delegate ()
                {
                    pictureBox1.Refresh();
                }));
            }

        }

        private bool foundGoodie()
        {
            Point head = _snake.snakeBody.First();

            //is Goodie in Range of head? // Head +- 6 pixels
            if (head.X.Equals(_goodie.X) && head.Y.Equals(_goodie.Y))
            {
                //add new head of snake
                _snake.addNewHead();

                _goodie = createGoodie();
                return true;
            }

            return false;
        }

        private bool hasCollition()
        {
            Point head = _snake.snakeBody.First();
            //is first point (head) over border?
            if (head.X < 0 || head.Y < 0 || pictureBox1.Width <= head.X || pictureBox1.Height <= head.Y)
            {
                return true;
            }

            //is first point (head) on own body?
            foreach (Point bodyPoint in _snake.snakeBody)
            {
                if (bodyPoint != head)
                {
                    if (head.X.Equals(bodyPoint.X) && head.Y.Equals(bodyPoint.Y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Point createGoodie()
        {
            //get random position (no collition with snake)
            Random rnd = new Random();
            bool collition = false;
            int X = 0;
            int Y = 0;

            do
            {
                collition = false;
                X = rnd.Next(0, pictureBox1.Width - 1);
                Y = rnd.Next(0, pictureBox1.Height - 1);

                foreach (Point point in _snake.snakeBody)
                {
                    if (point.X.Equals(X) && point.Y.Equals(Y))
                    {
                        collition = true;
                        break;
                    }
                }

            } while (collition);
            

            return new Point(X, Y, Brushes.Blue);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            textBox_snakeLength.Text = _snake.snakeBody.Count.ToString();
            textBox_snakeLength.Refresh();

            Graphics canvas = e.Graphics;

            //goodie
            Rectangle goodie = new Rectangle(_goodie.X, _goodie.Y, 1, 1);
            canvas.FillRectangle(_goodie.color, goodie);


            //snake
            foreach (Point point in _snake.snakeBody)
            {
                Rectangle rectangle = new Rectangle(point.X, point.Y, 1, 1);
                canvas.FillRectangle(point.color, rectangle);
            }
        }
    }
}
