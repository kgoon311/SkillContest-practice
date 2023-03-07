using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Entity
{
    [SerializeField] private float deathTimer;
    protected override void Update()
    {
        base.Update();
    }
    protected override void Move()
    {
        transform.Translate(Vector3.forward * -1 * Time.deltaTime * speed);

        deathTimer -= Time.deltaTime;
        if (deathTimer < 0)
            Destroy(gameObject);
    }
    protected override void Attack()
    {

    }
}