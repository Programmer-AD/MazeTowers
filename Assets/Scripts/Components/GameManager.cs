using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MazeTilemapAdapter mazeTilemap;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Castle castle;

    private int enemyExists;

    public bool RoundGoing { get; private set; }

    void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        mazeTilemap.GenerateMaze();
        castle.FixCastle();

        enemyExists = 0;
        RoundGoing = true;

        spawner.StartSummon(10);
    }

    public void OnCastleHealthChanged(int health)
    {
        if (health <= 0 && RoundGoing)
        {
            EndRound(win: false);
        }
    }

    public void OnEnemySpawn()
    {
        enemyExists++;
    }

    public void OnEnemyDead()
    {
        enemyExists--;
        if (enemyExists == 0 && RoundGoing && spawner.SpawnedAll)
        {
            EndRound(win: true);
        }
    }

    private void EndRound(bool win)
    {
        RoundGoing = false;
    }
}
