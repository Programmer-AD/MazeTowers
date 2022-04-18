public struct PathSegment
{
    public float Length;
    public float ThenRotateBy;

    public PathSegment(int length, float thenRotateBy)
    {
        Length = length;
        ThenRotateBy = thenRotateBy;
    }
}
