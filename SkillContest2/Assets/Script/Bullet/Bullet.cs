using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity
{
    [SerializeField] private float deadTime;
    [SerializeField] private float deadTimer;
    protected override void myUpdate()
    {
        base.myUpdate();
        DeadCountdown();
    }
    protected override void Move()
    {
        transform.position += transform.forward * speed;
    }
    protected override IEnumerator AttackPattern()
    {
        return base.AttackPattern();
    }
    protected void DeadCountdown()
    {
        deadTimer += Time.deltaTime;
        if (deadTimer > deadTime)
            Destroy(gameObject);
    }
    protected override void Dead()
    {
        Instantiate(EntityManager.Instance.hitParticle,transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
