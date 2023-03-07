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
        transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
    }
    protected override void Attack()
    {
        base.Attack();
    }
    protected override IEnumerator AttackPattern()
    {
        float angle = transform.rotation.x * Mathf.Rad2Deg;
        for(int i = -1;i <2;i++)
        {
            Instantiate(bullet, transform.position, Quaternion.Euler(0, angle + (30 * i), 0));
        }
        yield return null;
    }
}
