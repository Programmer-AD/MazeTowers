namespace ForTestsCSharp.Mazes
{
    interface IBitMap
    {
        bool this[int x, int y] { get; set; }
        int Width { get; }
        int Height { get; }
    }
}
