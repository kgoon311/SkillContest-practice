using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarigariBullet : EnemyBullet
{
    [SerializeField] private float turnValue;
    [SerializeField] private float turnTime;
    private float t_timer;
    private bool isTurn;

    protected override void Move()
    {
        base.Move();
        t_timer += Time.deltaTime;
        if(t_timer > turnTime)
        {
            transform.Rotate((isTurn) ? Vector3.up * turnValue : Vector3.down * turnValue);
            t_timer = 0;
            isTurn = !isTurn;
        }
    }
}
