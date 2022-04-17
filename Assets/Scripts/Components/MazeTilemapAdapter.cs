using System.Collections.Generic;
using System.Linq;
using MazeTowers.Mazes;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;


internal class MazeTilemapAdapter : MonoBehaviour, IBitMap
{
    private const int WallLayerZ = 0;

    private static readonly Vector2Int MazeStartPoint = new(1, 1);
    private Vector2Int MazeEndPoint => new(width - 2, height - 2);

    [SerializeField, Range(3, 1001)] private int width;
    [SerializeField, Range(3, 1001)] private int height;
    [SerializeField, Range(1, 1001)] private int mazeCurvity;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase spawnerTile;
    [SerializeField] private TileBase castleTile;

    private Tilemap tilemap;
    private MazeGenerator mazeGenerator;
    private MazeSolver mazeSolver;

    public IEnumerable<Vector2Int> Path { get; private set; }

    public Vector3Int SpawnerPosition => GetTilePosition(MazeStartPoint);
    public Vector3Int CastlePosition => GetTilePosition(MazeEndPoint);

    void Start()
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
        get => tilemap.HasTile(GetTilePosition(x, y));
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
        var path = mazeSolver.GetPath(MazeStartPoint, MazeEndPoint);
        Path = MakeGlobal(path);

        SetSpecialTiles();
    }

    private void SetSpecialTiles()
    {
        tilemap.SetTile(SpawnerPosition, spawnerTile);
        tilemap.SetTile(CastlePosition, castleTile);
    }

    private IEnumerable<Vector2Int> MakeGlobal(IEnumerable<Vector2Int> path)
    {
        var result = path
            .Select(x => (Vector2Int)GetTilePosition(x))
            .ToList();
        return result;
    }

    private Vector3Int GetTilePosition(Vector2Int point)
    {
        return GetTilePosition(point.x, point.y);
    }

    private Vector3Int GetTilePosition(int x, int y)
    {
        return new Vector3Int(x - Width / 2, Height / 2 - y, WallLayerZ);
    }
}
