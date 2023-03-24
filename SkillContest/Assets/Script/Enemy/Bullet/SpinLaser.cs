using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpinLaser : EnemyBullet
{
    [SerializeField] private LineRenderer[] lineRenderer;
    [SerializeField] private GameObject[] attackPos;
    [SerializeField] private float attackSpeed;
    private float timer;
    private bool isAttack;
    protected override void Attack()
    {
        timer += Time.deltaTime;
        if(isAttack == false && timer > attackSpeed)
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

        float timer = 0;
        while(timer < 1)
        {
            timer += Time.deltaTime / 3;
            for(int i = 0;i<4;i++)
            {
                lineRenderer[i].SetPosition(0, transform.position);
                lineRenderer[i].SetPosition(1, attackPos[i].transform.position);
            }

            transform.Rotate(Vector3.up / 2);
            yield return null;
        }

        for (int i = 0; i < 4; i++)
            Destroy(lineRenderer[i].gameObject);
        speed = beforeSpeed;
    }
}
