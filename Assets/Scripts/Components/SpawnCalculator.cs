using System;
using System.Collections.Generic;
using System.Linq;

public static class SpawnCalculator
{
    public static IEnumerable<SpawnDescription> GetSpawns(int roundNum, MazeTilemapAdapter mazeTilemap)
    {
        var roundDifficulty = (roundNum - 1) / 5f;
        var difficulty = roundDifficulty;

        var baseSpawnCount = (mazeTilemap.Width + mazeTilemap.Height) / 4f;
        var spawnCount = (int)MathF.Round((1 + difficulty / 50f) * baseSpawnCount);
        var totalSpawnLevel = roundDifficulty * spawnCount;

        var baseLevel = (int)MathF.Floor(totalSpawnLevel / spawnCount);
        var nextLevelCount = (int)MathF.Round(totalSpawnLevel - baseLevel * spawnCount);

        var result = new List<SpawnDescription>
        {
            new(baseLevel, spawnCount - nextLevelCount),
            new(baseLevel + 1, nextLevelCount)
        };

        return result;
    }
}
