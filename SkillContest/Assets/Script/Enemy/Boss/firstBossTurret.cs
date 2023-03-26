using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstBossTurret : Entity
{
    [SerializeField] private GameObject turretDeadParticle;
    public GameObject[] shotPos = new GameObject[2];
    public bool isDie;
    protected override void Update()
    {
        if(isDie == false)
            base.Update();
        if (hp <= 0 && isDie == false)
        {
            hp = 0;
            turretDeadParticle.SetActive(true);
            isDie = true; 
        }
    }
    protected IEnumerator Dead()
    {
        isInvi = true;
        yield return null;
    }
    protected override void Move()
    {
        transform.LookAt(Player.instance.transform.position);
    }
    protected override void Attack()
    {
    }
}
