using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : Enemy
{
    [SerializeField] private int shotCount;
    [SerializeField] private float shotDis;
    protected override IEnumerator AttackPattern()
    {
        for(int i = -shotCount / 2; i <= shotCount / 2; i++)
        {
            Quaternion rotete = Quaternion.Euler(0, model.transform.rotation.y - shotDis * i , 0);
            Instantiate(bullet[0], transform.position, rotete); 
        }
        yield return null; 
    }
    protected override void Move()
    {
        base.Move();
        model.transform.LookAt(GameManger.Instance.player.transform);
    }
}
