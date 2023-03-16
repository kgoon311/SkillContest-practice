using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstBossTurret : Boss
{
    [SerializeField] private ParticleSystem turretDeadParticle;
    public bool isDie;
    protected override void Update()
    {
        if (hp <= 0 && isDie == false)
        {
            hp = 0;
            turretDeadParticle.Play();
            isDie = true; 
        }
    }
    protected override IEnumerator Dead()
    {
        deadParticle.Play();
        isInvi = true;
        yield return null;
    }
    protected override void Move()
    {
    }
    protected override void Attack()
    {
    }
}
