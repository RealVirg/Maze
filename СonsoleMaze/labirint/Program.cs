using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace labirint
{
    class MainClass
    {
        public class Player
        {
            public Player(Maze maze, int x, int y)
            {
                point = maze.points[x, y];
            }
            public Point point;

            public int Score = 0;

            public void Move(KeyEventArgs key, Form form, Maze maze)
            {
                if (key.KeyCode == Keys.Up)
                    if (!(point.X <= 0))
                        if (maze.points[point.X - 1, point.Y].isEmpty)
                            point = maze.points[point.X - 1, point.Y];
                if (key.KeyCode == Keys.Down)
                    if (!(point.X >= size - 1))
                        if (maze.points[point.X + 1, point.Y].isEmpty)
                            point = maze.points[point.X + 1, point.Y];
                if (key.KeyCode == Keys.Left)
                    if (!(point.Y <= 0))
                        if (maze.points[point.X, point.Y - 1].isEmpty)
                            point = maze.points[point.X, point.Y - 1];
                if (key.KeyCode == Keys.Right)
                    if (!(point.Y >= size - 1))
                        if (maze.points[point.X, point.Y + 1].isEmpty)
                            point = maze.points[point.X, point.Y + 1];
                if (this.point.Y == size - 1)
                    Score++;
            }

            public void MoveConcole(ConsoleKeyInfo key, Maze maze)
            {
                if (key.Key == ConsoleKey.LeftArrow)
                    if (!(point.Y <= 0))
                        if (maze.points[point.X, point.Y - 1].isEmpty)
                            point = maze.points[point.X, point.Y - 1];
                if (key.Key == ConsoleKey.RightArrow)
                    if (!(point.Y >= size - 1))
                        if (maze.points[point.X, point.Y + 1].isEmpty)
                            point = maze.points[point.X, point.Y + 1];
                if (key.Key == ConsoleKey.UpArrow)
                    if (!(point.X <= 0))
                        if (maze.points[point.X - 1, point.Y].isEmpty)
                            point = maze.points[point.X - 1, point.Y];
                if (key.Key == ConsoleKey.DownArrow)
                    if (!(point.X >= size - 1))
                        if (maze.points[point.X + 1, point.Y].isEmpty)
                            point = maze.points[point.X + 1, point.Y];
                if (this.point.Y == size - 1)
                    Score++;
            }
        }
        const int size = 22;
        public class Path
        {
            public LinkedList<Point> pathPoint = new LinkedList<Point>();
            public void makePath(Maze maze, Point startPoint)
            {
                var random = new Random();
                pathPoint.AddFirst(startPoint);
                var visited = new List<Point>();
                visited.Add(pathPoint.Last.Value);
                var prevPoint = new Point(-1, -1);
                while (true)
                {
                    if (prevPoint == pathPoint.Last.Value)
                    {
                        pathPoint.RemoveLast();
                        continue;
                    }
                    prevPoint = pathPoint.Last.Value;
                    if (pathPoint.Last.Value.Neighbour.Count == 0)
                        pathPoint.Last.Value.getNeighbour(maze);
                    var pointsToVisit = new List<Point>();
                    foreach (var p in pathPoint.Last.Value.Neighbour)
                        pointsToVisit.Add(p);
                    for (var i = 0; i < pointsToVisit.Count; i++)
                    {
                        if (visited.Contains(pointsToVisit[i]) || !pointsToVisit[i].isEmpty)
                        {
                            pointsToVisit.Remove(pointsToVisit[i]);
                        }
                    }
                    if (pointsToVisit.Count == 0)
                    {
                        visited.Add(pathPoint.Last.Value);
                        pathPoint.RemoveLast();
                        continue;
                    }
                    var r = random.Next() % pointsToVisit.Count;
                    if (!visited.Contains(pointsToVisit[r])) 
                    {
                        visited.Add(pointsToVisit[r]);
                        pathPoint.AddLast(pointsToVisit[r]);
                    }
                    if (pathPoint.Last.Value.Y == size - 1)
                        break; 
                }
            }

        }

        public class Maze
        {
            public Point[,] points;
            public Maze()
            {
                points = new Point[size, size];
                for (var x = 0; x < size; x++)
                    for (var y = 0; y < size; y++)
                    {
                        points[x, y] = new Point(x, y);
                    }
            }

            public Maze MakePatern(Player player, Maze maze)
            {
                var result = new Maze();
                for (var i = 0; i < size; i++)
                {
                    result.points[i, 0].isEmpty = maze.points[i, player.point.Y].isEmpty;
                }
                for (var i = 0; i < size; i++)
                {;
                    result.points[i, 0].isInPath = false;
                }
                player.point = result.points[player.point.X, 0];
                return result;
            }

            public Maze MakeLabirint(Path path, Maze maze)
            {

                while (path.pathPoint.Count != 0)
                {
                    maze.points[path.pathPoint.Last.Value.X, path.pathPoint.Last.Value.Y].isInPath = true;
                    path.pathPoint.RemoveLast();
                }
                var random = new Random();
                for (var x = 0; x < size; x++)
                    for (var y = 1; y < size; y++)
                    {
                        if (!maze.points[x, y].isInPath)
                        {
                            if (random.Next() % 2 == 0)
                                maze.points[x, y].isEmpty = true;
                            else
                                maze.points[x, y].isEmpty = false;
                        }
                    }

                return maze;
            }
        }

        public class Point
        {
            public bool isEmpty = true;
            public bool isInPath = false;
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
            public void getNeighbour(Maze maze)
            {
                for (var x = -1; x <= 1; x++)
                    for (var y = -1; y <= 1; y++)
                    {
                        if ((x != 0 && y != 0) || (x == 0 && y == 0))
                            continue;
                        if (this.X + x < 0 || this.X + x > size - 1 || this.Y + y < 0 || this.Y + y > size - 1)
                            continue;
                        this.Neighbour.Add(maze.points[this.X + x, this.Y + y]);
                    }
            }

            public List<Point> Neighbour = new List<Point>();
        }

        public static void Main()
        {
            var maze = new Maze();
            var player = new Player(maze, 0, 0);
            var path = new Path();
            path.makePath(maze, player.point);
            var currentMaze = maze.MakeLabirint(path, maze);
            while (true)
            {
                if (player.point.Y == size - 1)
                {
                    currentMaze = currentMaze.MakePatern(player, currentMaze);
                    path = new Path();
                    path.makePath(currentMaze, player.point);
                    currentMaze.MakeLabirint(path, currentMaze);
                }
                for (var x = 0; x < size; x++)
                {
                    for (var y = 0; y < size; y++)
                    {
                        if (x == player.point.X && y == player.point.Y)
                            Console.Write(0);
                        else if (currentMaze.points[x, y].isInPath)
                            Console.Write(' ');
                        else if (currentMaze.points[x, y].isEmpty)
                            Console.Write(' ');
                        else
                            Console.Write('#');
                    }
                    Console.WriteLine();
                }
                ConsoleKeyInfo kb = Console.ReadKey();
                player.MoveConcole(kb, currentMaze);
                Console.WriteLine(player.Score);
            }
        }
    }
}
