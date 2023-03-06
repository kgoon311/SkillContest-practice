using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMonster : Enemy
{
    protected override void Move()
    {
        base.Move();
    }
    protected override void Attack()
    {
        base.Attack();
    }
    protected override IEnumerator AttackPattern()
    {
        
        yield return null;
    }
}
