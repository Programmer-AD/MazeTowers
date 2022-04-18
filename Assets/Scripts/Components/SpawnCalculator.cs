using System;
using System.Collections.Generic;
using System.Linq;

public static class SpawnCalculator
{
    public static IEnumerable<SpawnDescription> GetSpawns(int roundNum, MazeTilemapAdapter mazeTilemap)
    {
        var mazeDifficulty = GetMazeDifficulty(mazeTilemap);
        var roundDifficulty = (roundNum - 1) / 10f;
        var difficulty = roundDifficulty / mazeDifficulty;

        var baseSpawnCount = (mazeTilemap.Width + mazeTilemap.Height) / 4f;
        var spawnCount = (int)MathF.Round((1 + difficulty / 100) * baseSpawnCount);
        var totalSpawnLevel = roundDifficulty * spawnCount;

        var baseLevel = (int)MathF.Floor(totalSpawnLevel / spawnCount);
        var nextLevelCount = (int)MathF.Round((totalSpawnLevel - baseLevel * spawnCount) * spawnCount);

        var result = new List<SpawnDescription>
        {
            new(baseLevel, spawnCount - nextLevelCount),
            new(baseLevel + 1, nextLevelCount)
        };

        return result;
    }

    private static float GetMazeDifficulty(MazeTilemapAdapter mazeTilemap)
    {
        var mazeWidth = mazeTilemap.Width - 2;
        var mazeHeight = mazeTilemap.Height - 2;

        var minLength = mazeWidth + mazeHeight - 1;
        var maxLength = (mazeWidth * mazeHeight) / 2f;
        var averageLength = (maxLength + minLength) / 2f;

        var pathLength = mazeTilemap.Path.Count();
        var result = averageLength / pathLength;
        return result;
    }
}
