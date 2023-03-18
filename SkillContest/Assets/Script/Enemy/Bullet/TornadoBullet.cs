using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoBullet : EnemyBullet
{
    protected override void Move()
    {
        base.Move();
        transform.Rotate(Vector3.up);
    }
}
