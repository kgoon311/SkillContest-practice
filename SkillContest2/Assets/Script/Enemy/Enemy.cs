using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected GameObject model;
    [SerializeField] protected GameObject[] bullet;

    [SerializeField] private float deadTime;
    private float deadTimer = 0;
    protected bool dontshot;
    protected override void myUpdate()
    {
        base.myUpdate();
        DeadCountdown();
    }
    protected override IEnumerator AttackPattern()
    {
        return base.AttackPattern();
    }
    protected override void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    protected override void Attack()
    {
        if(dontshot == false)
            base.Attack();
    }
    protected virtual void DeadCountdown()
    {
        deadTimer += Time.deltaTime;
        if (deadTimer > deadTime)
        {
            Destroy(gameObject);
        }

    }
    protected override void Dead()
    {
        EntityManager.Instance.DeadParticle(transform.position);
        Destroy(gameObject);
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance._hp -= dmg;
            Dead();
        }
        if (other.CompareTag("DontShot"))
        {
            dontshot = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DontShot"))
        {
            dontshot = false;
        }
    }
}
