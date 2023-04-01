using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tonado : EnemyBullet
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float tonadoTime;
    private float timer = 0;
    protected override void myUpdate()
    {
        base.myUpdate();
        timer += Time.deltaTime;
        if(timer < tonadoTime)
        {
            transform.Rotate(Vector3.up * rotateSpeed);
        }
    }
}
