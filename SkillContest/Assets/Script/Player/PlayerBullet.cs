using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Entity
{
    [SerializeField] private float deathTimer;
    protected override void Update()
    {
        base.Update();
    }
    protected override void Move()
    {
        transform.position += Vector3.forward * Time.deltaTime * speed;

        deathTimer -= Time.deltaTime;
        if (deathTimer < 0)
            Destroy(gameObject);
    }
    protected override void Attack()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Entity script = other.GetComponent<Entity>();
            script.Hit(dmg);
            Destroy(gameObject);
        }
    }
}
