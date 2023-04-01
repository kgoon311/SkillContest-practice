using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossTurret : Entity
{
    public bool isDie;
    public GameObject[] shotPos;
    protected override void myUpdate()
    {
        base.myUpdate();
        transform.LookAt(Player.Instance.transform);
    }
    protected override void Dead()
    {
        Instantiate(EntityManager.Instance.bossDeadParticle, transform.position, transform.rotation);
    }
    protected override void Move()
    {
    }
}
