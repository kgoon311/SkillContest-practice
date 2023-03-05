using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float atkSpeed;
    [SerializeField] protected float atkTimer;

    [SerializeField] private float deathTimer;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Move()
    {
        transform.Translate(Vector3.forward  * speed *Time.deltaTime);

        deathTimer -= Time.deltaTime;
        if (deathTimer < 0)
            Destroy(gameObject);
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
    protected IEnumerator AttackPattern()
    {
        yield return null; 
    }
}
