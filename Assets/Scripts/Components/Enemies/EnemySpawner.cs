using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MazeTilemapAdapter mazeTilemap;
    [SerializeField] private float summonInterval;
    [SerializeField] private float startDelay;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private List<EnemyCharacteritics> characteritics = new();

    private IEnumerable<PathSegment> bakedPath;
    private Vector3 summonPositon;

    public bool SpawnedAll { get; private set; }

    public void StartSummon(int summonCount)
    {
        summonPositon = mazeTilemap.SpawnerPosition + new Vector3(0.5f, 1.5f);
        var path = mazeTilemap.Path;
        bakedPath = BakePath(path);
        SpawnedAll = false;

        StartCoroutine(SummonCoroutine(summonCount));
    }

    private IEnumerator SummonCoroutine(int summonCount)
    {
        yield return new WaitForSeconds(startDelay);
        while (summonCount-- > 0)
        {
            yield return new WaitForSeconds(summonInterval);
            SummonEnemy();
        }
        SpawnedAll = true;
    }

    private void SummonEnemy()
    {
        var enemy = Instantiate(enemyPrefab, summonPositon, Quaternion.identity);
        enemy.pathMover = bakedPath.GetEnumerator();
        enemy.gameManager = gameManager;
        enemy.Dead = EnemyDead;
        enemy.characteritics = characteritics[0];

        EnemySpawned?.Invoke();
    }

    public UnityEvent EnemySpawned;
    public UnityEvent<Enemy> EnemyDead;

    private static IEnumerable<PathSegment> BakePath(IEnumerable<Vector2Int> path)
    {
        var result = new List<PathSegment>();
        var deltas = GetDeltas(path);

        int length = 0;
        var previousDelta = Vector2Int.up;
        foreach (var delta in deltas)
        {
            if (delta != previousDelta)
            {
                var targetRotation = Quaternion.FromToRotation((Vector3Int)previousDelta, (Vector3Int)delta);
                var targetRotationAngle = targetRotation.eulerAngles.z;
                result.Add(new(length, targetRotationAngle));

                length = 0;
            }
            length++;
            previousDelta = delta;
        }
        result.Add(new(length, 0));
        return result;
    }

    private static IEnumerable<Vector2Int> GetDeltas(IEnumerable<Vector2Int> path)
    {
        Vector2Int? previousPoint = null;
        foreach (var point in path)
        {
            if (previousPoint.HasValue)
            {
                yield return point - previousPoint.Value;
            }
            previousPoint = point;
        }
    }
}
