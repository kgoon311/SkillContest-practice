using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : EnemyBullet
{
    [SerializeField] private int maxAttackCount = 0;
    public secondBoss secondBoss;
    [SerializeField] private float attackSpeed;
    private float timer;
    private bool isAttack;
    protected override void Attack()
    {
        timer += Time.deltaTime;
        if (isAttack == false && timer > attackSpeed)
            StartCoroutine(AttackPattern());
    }
    protected override void Move()
    {
        transform.position += -Vector3.forward * Time.deltaTime * speed;

        deathTimer -= Time.deltaTime;
        if (deathTimer < 0)
            Destroy(gameObject);
    }
    protected IEnumerator AttackPattern()
    {
        isAttack = true; 
        float beforeSpeed = speed;
        speed = 0;

        int count = Random.Range(3,maxAttackCount);
        for (int i = 0; i < count; i++)
        {
            Debug.Log("?");
            float rotate = Random.Range(0, 361);
            transform.rotation = Quaternion.Euler(0, rotate + 90, 0);   
            secondBoss.StartCoroutine(secondBoss.DrawWaringLine(transform.position, Quaternion.Euler(0, rotate, 0)));
            secondBoss.StartCoroutine(secondBoss.DrawWaringLine(transform.position, Quaternion.Euler(0, rotate + 180, 0)));

            yield return new WaitForSeconds(0.5f);
        }
        speed = beforeSpeed;
    }
}
