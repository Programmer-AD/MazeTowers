using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private TowerCharacteristics characteristics;
    private CircleCollider2D rangeTrigger;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int shootInterval;

    [NonSerialized] public GameManager gameManager;

    private int shootCooldown;
    private bool enemyNearby;

    void Awake()
    {
        rangeTrigger = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetCharacteristics(TowerCharacteristics characteristics)
    {
        this.characteristics = characteristics;
        rangeTrigger.radius = characteristics.Range + 1;
        spriteRenderer.sprite = characteristics.sprite;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out var enemy))
        {
            var lookDirection = enemy.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(lookDirection, Vector3.back);
            transform.rotation = rotation;
            enemyNearby = true;
        }
    }

    void FixedUpdate()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (characteristics != null && shootCooldown-- <= 0 && enemyNearby)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.Damage = characteristics.Damage;
            bullet.FlyDistance = characteristics.Range;

            shootCooldown = shootInterval;
            enemyNearby = false;
        }
    }
}
