using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMonster : Enemy
{
    private Transform playerPos;

    protected override void Start()
    {
        base.Start();
        playerPos = Player.instance.transform;
    }
    protected override void Move()
    {
        transform.LookAt(playerPos.position);
        transform.position += Vector3.forward * -speed * Time.deltaTime;
    }                              
    protected override IEnumerator AttackPattern()
    {
        for(int i = -1;i <2;i++)
        {
            Instantiate(bullet, transform.position, Quaternion.Euler(0, transform.rotation.eulerAngles.y +(30 * i), 0));
        }
        yield return null;
    }
}
