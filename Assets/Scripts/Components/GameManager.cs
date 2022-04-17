using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float roundStartDelay;
    [SerializeField] private MazeTilemapAdapter mazeTilemap;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Castle castle;
    [SerializeField] private TowerManager towerManager;

    private int enemyExists;

    public bool RoundGoing { get; private set; }
    public int RoundNumber { get; private set; }

    void Start()
    {
        towerManager.InitTowerSlots(mazeTilemap);
        mazeTilemap.GenerateMaze();
        castle.FixCastle();
        RoundNumber = 1;
        
        ScheduleRoundStart();
    }

    public UnityEvent<int> RoundStarted;
    public void StartRound()
    {
        enemyExists = 0;
        RoundGoing = true;
        RoundStarted?.Invoke(RoundNumber);

        var spawns = SpawnCalculator.GetSpawns(RoundNumber, mazeTilemap);
        spawner.StartSummon(spawns);
    }

    public void OnCastleHealthChanged(int health, int _)
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

    public void OnEnemyDead(Enemy _)
    {
        enemyExists--;
        if (enemyExists == 0 && RoundGoing && spawner.SpawnedAll)
        {
            EndRound(win: true);
        }
    }

    public UnityEvent<int, bool> RoundEnded;

    private void EndRound(bool win)
    {
        RoundGoing = false;
        RoundEnded?.Invoke(RoundNumber, win);

        mazeTilemap.GenerateMaze();
        castle.FixCastle();

        if (win)
        {
            RoundNumber++;
        }

        ScheduleRoundStart();
    }

    private void ScheduleRoundStart()
    {
        Invoke(nameof(StartRound), roundStartDelay);
    }
}
