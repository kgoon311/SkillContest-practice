using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : EnemyBullet
{
    private Transform target;
    [SerializeField] private GameObject model;
    [SerializeField] private float HomingTime;
    [SerializeField] private float HomingSpeed;
    private float timer;
    protected override void Awake()
    {
        base.Awake();
        target = Player.Instance.transform;
    }
    protected override void Move()
    {
        base.Move();
        timer += Time.deltaTime;
        model.transform.Rotate(Vector3.forward);
        if(timer < HomingTime)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion rotate = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * HomingSpeed);
        }
    }
}
