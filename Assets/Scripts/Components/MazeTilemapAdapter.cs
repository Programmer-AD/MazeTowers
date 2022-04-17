using System.Collections.Generic;
using System.Linq;
using MazeTowers.Mazes;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeTilemapAdapter : MonoBehaviour, IBitMap
{
    private const int WallLayerZ = 0;

    private Vector2Int MazeStartPoint => new(1, height - 2);
    private Vector2Int MazeEndPoint => new(width - 2, 1);

    [SerializeField, Range(3, 1001)] private int width;
    [SerializeField, Range(3, 1001)] private int height;
    [SerializeField, Range(1, 1001)] private int mazeCurvity;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase backgroundTile;

    private Tilemap tilemap;
    private MazeGenerator mazeGenerator;
    private MazeSolver mazeSolver;

    public IEnumerable<Vector2Int> Path { get; private set; }

    public Vector3Int SpawnerPosition => GetTilePosition(MazeStartPoint);
    public Vector3Int CastlePosition => GetTilePosition(MazeEndPoint);

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        var random = new System.Random();
        mazeGenerator = new(random);
        mazeSolver = new(this);
    }

    public int Width => width;
    public int Height => height;

    public bool this[int x, int y]
    {
        get
        {
            var tileCollider = tilemap.GetColliderType(GetTilePosition(x, y));
            return tileCollider != Tile.ColliderType.None;
        }
        set
        {
            var tile = value ? wallTile : null;
            var tilePosition = GetTilePosition(x, y);
            tilemap.SetTile(tilePosition, tile);
        }
    }

    public void GenerateMaze()
    {
        mazeGenerator.Generate(this, MazeStartPoint, mazeCurvity);
        MakeBackground();

        Path = mazeSolver.GetPath(MazeStartPoint, MazeEndPoint);
    }

    private void MakeBackground()
    {
        tilemap.FloodFill(SpawnerPosition, backgroundTile);
    }

    public Vector3Int GetTilePosition(Vector2Int point)
    {
        return GetTilePosition(point.x, point.y);
    }

    public Vector3Int GetTilePosition(int x, int y)
    {
        return new Vector3Int(x - Width / 2, y - Height / 2, WallLayerZ);
    }
}
