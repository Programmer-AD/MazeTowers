using System.Collections.Generic;
using UnityEngine;

namespace ForTestsCSharp.Mazes
{
    class MazeSolver
    {
        private readonly IBitMap map;

        private readonly BitMap visitedCells;
        private readonly Vector2Int[,] wentFrom;

        public MazeSolver(IBitMap map)
        {
            this.map = map;

            visitedCells = new BitMap(map.Width, map.Height);
            wentFrom = new Vector2Int[map.Width, map.Height];
        }

        public IEnumerable<Vector2Int> GetPath(Vector2Int start, Vector2Int end)
        {
            visitedCells.Clear();
            var toCheck = new Stack<(Vector2Int, Vector2Int)>();

            var current = start;
            toCheck.Push((current, start));

            while (!current.Equals(end))
            {
                Vector2Int previous;
                (current, previous) = toCheck.Pop();

                var currentX = current.x;
                var currentY = current.y;

                if (CheckIndex(currentX, map.Width)
                    && CheckIndex(currentY, map.Height)
                    && !visitedCells[currentX, currentY]
                    && !map[currentX, currentY])
                {
                    visitedCells[currentX, currentY] = true;
                    wentFrom[currentX, currentY] = previous;

                    toCheck.Push((new (currentX + 1, currentY), current));
                    toCheck.Push((new (currentX - 1, currentY), current));
                    toCheck.Push((new (currentX, currentY + 1), current));
                    toCheck.Push((new (currentX, currentY - 1), current));
                }
            }

            var path = new Stack<Vector2Int>();
            while (!current.Equals(start))
            {
                path.Push(current);
                current = wentFrom[current.x, current.y];
            }
            path.Push(start);

            return path;
        }

        private static bool CheckIndex(int index, int limit)
        {
            return index >= 0 && index < limit;
        }
    }
}
