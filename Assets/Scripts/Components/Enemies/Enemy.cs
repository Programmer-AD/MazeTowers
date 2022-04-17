using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private const float Precision = 0.001f;

    [SerializeField] private float Speed;
    [SerializeField] private float RotateSpeed;

    public GameManager gameManager;
    public EnemyCharacteritics characteritics;
    public IEnumerator<PathSegment> pathMover;

    private float moveBy;
    private float rotateBy;

    public float Health { get; private set; }

    void Start()
    {
        Health = characteritics.MaxHealth;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = characteritics.sprite;
    }

    void FixedUpdate()
    {
        if (!gameManager.RoundGoing)
        {
            Destroy(gameObject);
        }else if (moveBy > Precision)
        {
            var move = Math.Min(moveBy, Speed);
            transform.Translate(move * Vector3.up);
            moveBy -= move;
        }
        else if (Math.Abs(rotateBy) > Precision)
        {
            var rotate = Math.Sign(rotateBy) * Math.Min(Math.Abs(rotateBy), RotateSpeed);
            transform.Rotate(rotate * Vector3.forward);
            rotateBy -= rotate;
        }
        else if (pathMover != null)
        {
            if (pathMover.MoveNext())
            {
                var currentSegment = pathMover.Current;
                moveBy = currentSegment.Length;
                rotateBy = currentSegment.ThenRotateBy;
                FixedUpdate();
            }
            else
            {
                pathMover = null;
            }
        }
    }

    public UnityEvent<Enemy> Dead;

    void OnDestroy()
    {
        Dead?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Bullet>(out var bullet))
        {
            Health -= bullet.Damage;
            if (Health <= 0)
            {
                Destroy(gameObject);
                Destroy(bullet.gameObject);
            }
        }
    }
}
