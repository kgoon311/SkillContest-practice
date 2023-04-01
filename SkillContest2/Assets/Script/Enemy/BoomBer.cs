using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBer : Enemy
{
    [SerializeField] private int shotCount;
    protected override IEnumerator AttackPattern()
    {
        for (int i = 0; i < shotCount; i++)
            Instantiate(bullet[i % 2], transform.position, Quaternion.Euler(0, (360 / shotCount) * i, 0));

        yield return null;
    }

    protected override void Dead()
    {
        for (int i = 0; i < shotCount * 2; i++)
            Instantiate(bullet[1], transform.position, Quaternion.Euler(0, (360 / shotCount * 2) * i, 0));
        base.Dead();
    }
}
