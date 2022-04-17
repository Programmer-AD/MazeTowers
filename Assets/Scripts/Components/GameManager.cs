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
        RoundNumber = 0;
        
        StartRound();
    }

    public UnityEvent<int> RoundStarted;
    public void StartRound()
    {
        mazeTilemap.GenerateMaze();
        castle.FixCastle();

        enemyExists = 0;
        RoundGoing = true;
        RoundStarted?.Invoke(RoundNumber);

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

    public UnityEvent<int, bool> RoundEnded;

    private void EndRound(bool win)
    {
        RoundGoing = false;
        RoundEnded?.Invoke(RoundNumber, win);

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
