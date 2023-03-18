using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : EnemyBullet
{
    [SerializeField] private float homingTime;
    [SerializeField] private float homingspeed;
    private float timer = 0;
    protected override void Update()
    {
        base.Update();
        Homing();
    }
    private void Homing()
    {
        if (timer > 1)
            return;

        timer += Time.deltaTime / homingTime;
        Vector3 dir = Player.instance.transform.position - transform.position;
        Quaternion rotate = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation , rotate , Time.deltaTime * homingspeed);
    }
}
