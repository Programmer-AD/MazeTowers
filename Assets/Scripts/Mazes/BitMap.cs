using System;
using System.Collections;

namespace ForTestsCSharp.Mazes
{
    class BitMap : IBitMap
    {
        private readonly BitArray bitArray;
        public int Width { get; }
        public int Height { get; }

        public BitMap(int width, int height)
        {
            bitArray = new BitArray(width * height);
            Width = width;
            Height = height;
        }

        public bool this[int x, int y]
        {
            get
            {
                var index = GetIndex(x, y);
                var result = bitArray[index];
                return result;
            }
            set
            {
                var index = GetIndex(x, y);
                bitArray[index] = value;
            }
        }

        public void Clear()
        {
            bitArray.SetAll(false);
        }

        private int GetIndex(int x, int y)
        {
            CheckFlatIndex(x, Width);
            CheckFlatIndex(y, Height);

            var result = y * Width + x;
            return result;
        }

        private static void CheckFlatIndex(int index, int limit)
        {
            if (index < 0 || index >= limit)
            {
                throw new IndexOutOfRangeException("Flat index out of range");
            }
        }
    }
}
