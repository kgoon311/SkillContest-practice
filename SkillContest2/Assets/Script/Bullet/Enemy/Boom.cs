using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : EnemyBullet
{
    [SerializeField] private int shotCount;
    [SerializeField] private GameObject[] model;
    [SerializeField] private GameObject bullet;
    protected override IEnumerator AttackPattern()
    {
        for (int i = 0; i < shotCount; i++)
        {
            Instantiate(bullet, transform.position, Quaternion.Euler(0, (360 / shotCount) * i, 0));
            Destroy(gameObject);
        }

        yield return null;
    }
    protected override void Move()
    {
        base.Move();
        for(int i = 0; i < 4; i++)
        {
            model[i].transform.Rotate(Vector3.forward * Random.Range(1f,5f));
        }
    }
}
