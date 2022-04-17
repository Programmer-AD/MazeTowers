using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MazeTilemapAdapter mazeTilemap;
    [SerializeField] private EnemySpawner spawner;

    public bool RoundGoing { get; private set; }

    void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        mazeTilemap.GenerateMaze();
        spawner.StartSummon(10);
        RoundGoing = true;
    }
}
