using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeTowers.Mazes
{
    class MazeGenerator
    {
        private readonly System.Random random;

        private readonly Stack<Vector2Int> targetPoints;
        private readonly List<(Vector2Int, int Length)> possibleMovements;

        private int lengthLimit;
        private Vector2Int current;
        private BitMap visitedCells;
        private int toVisit;

        public MazeGenerator(System.Random random)
        {
            this.random = random;

            targetPoints = new();
            possibleMovements = new(4);
        }

        public void Generate(IBitMap map, Vector2Int start, int lengthLimit)
        {
            CheckBitMapSize(map.Width);
            CheckBitMapSize(map.Height);

            InitInnerState(map, start, lengthLimit);

            FillGrid(map);
            DeleteWalls(map);

            ResetInnerState();
        }

        private void InitInnerState(IBitMap map, Vector2Int start, int lengthLimit)
        {
            this.lengthLimit = lengthLimit;

            current = start / 2;

            visitedCells = new BitMap(map.Width / 2, map.Height / 2);
            toVisit = visitedCells.Width * visitedCells.Height;

            targetPoints.Push(current);
            visitedCells[current.x, current.y] = true;
            toVisit--;
        }

        private void ResetInnerState()
        {
            visitedCells = null;
            targetPoints.Clear();
        }

        private static void CheckBitMapSize(int size)
        {
            if ((size & 1) == 0 && size > 3)
            {
                throw new ArgumentException("Size of bitmap must be odd and greater than 3");
            }
        }

        private static void FillGrid(IBitMap map)
        {
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var value = (x & y & 1) == 0;
                    map[x, y] = value;
                }
            }
        }

        private void DeleteWalls(IBitMap map)
        {
            while (toVisit > 0)
            {
                GetPossibleMovements();

                if (possibleMovements.Count > 1)
                {
                    targetPoints.Push(current);
                }

                if (possibleMovements.Count > 0)
                {
                    var index = random.Next(possibleMovements.Count);
                    var (delta, maxLength) = possibleMovements[index];
                    var length = random.Next(1, maxLength + 1);

                    while (length-- > 0)
                    {
                        current += delta;

                        var mapX = GetMapIndex(current.x);
                        var mapY = GetMapIndex(current.y);
                        map[mapX - delta.x, mapY - delta.y] = false;

                        visitedCells[current.x, current.y] = true;
                        toVisit--;
                    }
                }
                else
                {
                    current = targetPoints.Pop();
                }
            }
        }

        private void GetPossibleMovements()
        {
            possibleMovements.Clear();
            AddPossibleMovement(new(-1, 0));
            AddPossibleMovement(new(1, 0));
            AddPossibleMovement(new(0, -1));
            AddPossibleMovement(new(0, 1));
        }

        private void AddPossibleMovement(Vector2Int delta)
        {
            var length = MaxLength(delta);

            if (length > 0)
            {
                possibleMovements.Add((delta, length));
            }
        }

        private int MaxLength(Vector2Int delta)
        {
            int result = 0;

            var point = current + delta;

            while (CheckIndex(point.x, visitedCells.Width)
                && CheckIndex(point.y, visitedCells.Height)
                && !visitedCells[point.x, point.y]
                && result < lengthLimit)
            {
                result++;
                point += delta;
            }

            return result;
        }

        private static bool CheckIndex(int index, int limit)
        {
            return index >= 0 && index < limit;
        }

        private static int GetMapIndex(int localIndex)
        {
            return 2 * localIndex + 1;
        }
    }
}
