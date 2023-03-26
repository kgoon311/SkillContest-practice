using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float atkSpeed;
    [SerializeField] private GameObject deadParticle;
    protected float atkTimer;

    [SerializeField] private float deathTimer;
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.enemys.Add(this); 
    }

    protected override void Update()
    {
        base.Update();
        if(EntityMnager.instance.isStop == false)
        Deadtimer();
        if(hp <= 0)
        {
            Dead();
        }
    }
    protected override void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    protected override void Attack()
    {
        atkTimer += Time.deltaTime;
        if(atkTimer > atkSpeed)
        {
            StartCoroutine(AttackPattern());
            atkTimer = 0;
        }
    }
    protected void Deadtimer()
    {
        deathTimer -= Time.deltaTime;
        if (deathTimer < 0)
            Destroy(gameObject);
    }
    protected void Dead()
    {
        Instantiate(deadParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    protected virtual IEnumerator AttackPattern()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        yield return null; 
    }
}
