using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
namespace MovingRectangleGame;

class MazeGenerator
{
    public static int width = 42;
    public static int height = 25;
    public static int[,] maze = new int[width, height];
    public static Random rand = new Random();

    public static void GenerateMaze()
    {
        // Initialize the maze with walls (1)
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // if (i==width-1) {
                //     maze[i, j] = 0;
                // } else {
                    maze[i, j] = 1;
                // }
            }
        }

        // Start the maze generation from the top-left corner
        DFS(1, 1);
                // ensure the start point is open
        maze[1, 1] = 0;
        // Ensure the exit point is open
        maze[width - 2, height - 2] = 0;
    }

    public static void DFS(int x, int y)
    {
        // Directions: right, down, left, up
        int[] dx = { 1, 0, -1, 0 };
        int[] dy = { 0, 1, 0, -1 };

        // Shuffle directions to create a random maze
        for (int i = 0; i < 4; i++)
        {
            int r = rand.Next(4);
            int temp = dx[i];
            dx[i] = dx[r];
            dx[r] = temp;
            temp = dy[i];
            dy[i] = dy[r];
            dy[r] = temp;
        }

        // Visit each direction
        for (int i = 0; i < 4; i++)
        {
            // for (int j = 0; j < 2; j++)
            // {
            //     int r = rand.Next(4);
            //     int temp = dx[j];
            //     dx[j] = dx[r];
            //     dx[r] = temp;
            //     temp = dy[j];
            //     dy[j] = dy[r];
            //     dy[r] = temp;
            // }
            int nx = x + dx[i] * 2;
            int ny = y + dy[i] * 2;

            if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1 && maze[nx, ny] == 1)
            {
                maze[nx, ny] = 0;
                maze[x + dx[i], y + dy[i]] = 0;
                DFS(nx, ny);
            }
        }
    }

    public static int[,] GetMaze()
    {
        return maze;
    }

    public static void PrintMaze() {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Console.Write(maze[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
    
}
