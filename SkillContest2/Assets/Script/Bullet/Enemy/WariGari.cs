using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WariGari : EnemyBullet
{
    [SerializeField] private float rotation;
    private bool turn;
    protected override void myUpdate()
    {
        base.myUpdate();
    }
    protected override IEnumerator AttackPattern()
    {
        turn = !turn;
        transform.Rotate(Vector3.up * rotation * (turn ? -1 : 1));
        yield return null;
    }
}
