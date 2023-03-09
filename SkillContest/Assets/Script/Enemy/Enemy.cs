using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float atkSpeed;
    protected float atkTimer;

    [SerializeField] private float deathTimer;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Deadtimer();
    }
    protected override void Move()
    {
        transform.Translate(Vector3.forward  * speed *Time.deltaTime);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            float dmg = other.GetComponent<Entity>().dmg;
            Hit(dmg);
            Destroy(other.gameObject);
        }
    }
    protected abstract IEnumerator AttackPattern();
}
