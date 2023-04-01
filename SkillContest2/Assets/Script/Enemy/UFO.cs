using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : Enemy
{
    [SerializeField] private int shotCount;
    protected override IEnumerator AttackPattern()
    {
        for(int i = 0; i < shotCount; i++)
            Instantiate(bullet[0], transform.position, Quaternion.Euler(0, (360 / shotCount) * i , 0));

            yield return null;
    }
    protected override void Move()
    {
        base.Move();
        model.transform.Rotate(Vector3.up / 2);
    }
}
