using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float Precision = 0.001f;

    [SerializeField] private float Speed;
    [SerializeField] private float RotateSpeed;

    public EnemyCharacteritics characteritics;
    public IEnumerator<PathSegment> pathMover;

    private float moveBy;
    private float rotateBy;

    void FixedUpdate()
    {
        if (moveBy > Precision)
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
}
