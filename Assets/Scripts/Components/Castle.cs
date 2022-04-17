using UnityEngine;
using UnityEngine.Events;

public class Castle : MonoBehaviour
{
    [SerializeField] private MazeTilemapAdapter mazeTilemap;
    [SerializeField] private int MaxHealth;

    private int health;
    public int Health
    {
        get => health;
        private set
        {
            if (health != value)
            {
                health = value;
                OnHealthChanged();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            Health--;
            enemy.characteritics = null;
            Destroy(enemy.gameObject);
        }
    }

    public void FixCastle()
    {
        Health = MaxHealth;
        transform.position = mazeTilemap.CastlePosition + new Vector3(0.5f, 0.5f);
    }

    public UnityEvent<int, int> HealthChanged;
    private void OnHealthChanged()
    {
        HealthChanged?.Invoke(health, MaxHealth);
    }
}
