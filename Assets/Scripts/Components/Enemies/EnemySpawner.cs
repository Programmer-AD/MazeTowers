using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private MazeTilemapAdapter mazeTilemap;
    [SerializeField] private float SummonInterval;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private List<EnemyCharacteritics> characteritics = new();

    private IEnumerable<PathSegment> bakedPath;
    private Vector3 summonPositon;

    public void StartSummon(int summonCount)
    {
        summonPositon = mazeTilemap.SpawnerPosition + new Vector3(0.5f, 1.5f);
        var path = mazeTilemap.Path;
        bakedPath = BakePath(path);

        StartCoroutine(SummonCoroutine(summonCount));
    }

    private IEnumerator SummonCoroutine(int summonCount)
    {
        while (summonCount-- > 0)
        {
            SummonEnemy();
            yield return new WaitForSeconds(SummonInterval);
        }
    }

    private void SummonEnemy()
    {
        var enemy = Instantiate(enemyPrefab, summonPositon, Quaternion.identity);
        enemy.pathMover = bakedPath.GetEnumerator();
        enemy.characteritics = characteritics[0];
    }

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
