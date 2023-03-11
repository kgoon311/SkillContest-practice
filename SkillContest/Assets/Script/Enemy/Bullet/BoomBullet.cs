using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BoomBullet : EnemyBullet
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float[] boomtimes = new float[2];
    private float timer;

    protected override void Start()
    {
        base.Start();
        timer = Random.Range(boomtimes[0], boomtimes[1]);
    }
    protected override void Update()
    {
        Boom();
    }
    void Boom()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            for(int i = 0; i < 9; i++)
            {
                Instantiate(bullet, transform.position, Quaternion.Euler(Vector3.up * (40 * i)));
                Destroy(gameObject);
            }
        }
    }
}
