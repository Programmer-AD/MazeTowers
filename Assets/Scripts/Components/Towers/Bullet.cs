using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float Speed;

    [NonSerialized] public float Damage;
    [NonSerialized] public float FlyDistance;

    void FixedUpdate()
    {
        if (FlyDistance > 0)
        {
            var move = Math.Min(Speed, FlyDistance) * Vector3.up;
            transform.Translate(move);

            if (FlyDistance > Speed)
            {
                FlyDistance -= Speed;
            }
            else
            {
                FlyDistance = 0;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
