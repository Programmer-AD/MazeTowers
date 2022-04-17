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
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            Health--;
            Destroy(enemy.gameObject);
        }
    }

    public void FixCastle()
    {
        Health = MaxHealth;
        transform.position = mazeTilemap.CastlePosition + new Vector3(0.5f, 1.5f);
    }

    public UnityEvent<int> HealthChanged;
    private void OnHealthChanged()
    {
        HealthChanged?.Invoke(health);
    }
}
