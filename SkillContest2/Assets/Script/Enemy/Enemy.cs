using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected GameObject model;
    [SerializeField] protected GameObject[] bullet;
    protected override IEnumerator AttackPattern()
    {
        return base.AttackPattern();
    }
    protected override void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    protected override void Dead()
    {
        EntityManager.Instance.DeadParticle(transform.position);
    }
}
