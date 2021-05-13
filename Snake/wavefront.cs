using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    static class wavefront
    {
        public static List<Tuple<int,int>> getRoute(int [,] map, Point goodie, Point head)
        {
            map[goodie.X, goodie.Y] = 1;

            //Working List of Pixels
            List<Tuple<int,int>> workingList = new List<Tuple<int,int>>();
            List<Tuple<int, int>> neighbors;

            //Add initialy goodie
            workingList.Add(new Tuple<int, int>(goodie.X, goodie.Y));

            //calculating wavefront till all reachable pixels are done
            while (workingList.Count > 0)
            {
                Tuple<int, int> workingPixel = workingList[0];
                workingList.RemoveAt(0);

                neighbors = wavefront.getNeighbors(map, workingPixel);
                foreach (Tuple<int,int> neighbor in neighbors)
                {
                    //If neighbour 0 or gt costs + 1
                    if (map[neighbor.Item1, neighbor.Item2] == 0 || map[neighbor.Item1, neighbor.Item2] > map[workingPixel.Item1, workingPixel.Item2] +1 )  
                    {
                        //-> Setze neue Kosten und füge element working List hinzu
                        map[neighbor.Item1, neighbor.Item2] = map[workingPixel.Item1, workingPixel.Item2] + 1;
                        workingList.Add(new Tuple<int, int>(neighbor.Item1, neighbor.Item2));
                    }
                }
                
            }

            //calc route
            List<Tuple<int, int>> route = new List<Tuple<int, int>>();
            int costs = int.MaxValue;
            Tuple<int, int> currPos = new Tuple<int, int>(head.X, head.Y);
            int selectedNeighbor = -1;

            do
            {
                selectedNeighbor = -1;
                neighbors = getNeighbors(map, currPos);

                for (int i = 0; i < neighbors.Count; i++)
                {
                    //if neighbor is no obstacle and costs are lower
                    if (map[neighbors[i].Item1, neighbors[i].Item2] > 0 && map[neighbors[i].Item1, neighbors[i].Item2] < costs)
                    {
                        costs = map[neighbors[i].Item1, neighbors[i].Item2];
                        selectedNeighbor = i;
                    }
                }

                if (selectedNeighbor > -1)
                {
                    route.Add(neighbors[selectedNeighbor]);
                    currPos = neighbors[selectedNeighbor];
                }

            } while (costs > 1 && selectedNeighbor > -1);

            //no route found
            if (route.Count == 0)
            {
                return null;
            }

            return route;
        }

        public static int[,] createMap( int width, int height, Snake snake)
        {
            int[,] map = new int[width, height];

            foreach (Point point in snake.snakeBody)
            {
                if (point.X > -1 && width > point.X && point.Y > -1 && height > point.Y)
                {
                    map[point.X, point.Y] = -1;
                }
            }
            return map;
        }

        private static List<Tuple<int,int>> getNeighbors(int[,] world, Tuple<int,int> q)
        {
            if (world.Rank != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(world), world.Rank, "Rang von world nicht 2");
            }
            List<Tuple<int, int>> neighborsList = new List<Tuple<int, int>>();
            int x_dim = world.GetLength(0);
            int y_dim = world.GetLength(1);

            //left -x
            if (q.Item1 -1 >= 0)
            {
                neighborsList.Add(new Tuple<int, int>(q.Item1 -1, q.Item2));
            }
            //right +x
            if (x_dim > q.Item1 + 1)
            {
                neighborsList.Add(new Tuple<int, int>(q.Item1 + 1, q.Item2));
            }

            //up -y
            if (q.Item2 -1 >= 0)
            {
                neighborsList.Add(new Tuple<int, int>(q.Item1, q.Item2 -1));
            }

            //down +y
            if (y_dim > q.Item2 +1)
            {
                neighborsList.Add(new Tuple<int, int>(q.Item1, q.Item2 +1));
            }

            return neighborsList;
        }
    }
}
