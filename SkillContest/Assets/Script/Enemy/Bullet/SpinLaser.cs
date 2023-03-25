using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

        Vector3 startPos; 
        Vector3 endPos; 
        Quaternion rotate;
        LayerMask playerLayerMask = LayerMask.GetMask("Player");
        

        float beforeSpeed = speed;
        speed = 0;

        float timer = 0;
        while(timer < 1)
        {
            timer += Time.deltaTime / 3;
            startPos = transform.position;

            for(int i = 0;i<4;i++)
            {
                endPos = attackPos[i].transform.position;
                rotate = Quaternion.LookRotation(startPos, endPos);

                lineRenderer[i].SetPosition(0, transform.position);
                lineRenderer[i].SetPosition(1, attackPos[i].transform.position);

                if (Physics.BoxCast(startPos, Vector3.one / 2, endPos, rotate, 20, playerLayerMask))
                    Player.instance.Hit(dmg);
            }

            transform.Rotate(Vector3.up /4);
            yield return null;
        }

        for (int i = 0; i < 4; i++)
            Destroy(lineRenderer[i].gameObject);
        speed = beforeSpeed;
    }
}
