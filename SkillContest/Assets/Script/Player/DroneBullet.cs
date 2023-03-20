using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet : Entity
{
    [SerializeField] private float deathTimer;

    public GameObject targetObject;

    protected override void Update()
    {
        base.Update();

        
    }
    protected override void Move()
    {
        if (targetObject == null)
            transform.position += Vector3.forward * Time.deltaTime * speed;
        else
        {
            Vector3 dist = targetObject.transform.position - transform.position;
            Vector3 dir = dist.normalized;

            transform.LookAt(targetObject.transform);
            transform.position += dir * Time.deltaTime * speed;
        }

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
