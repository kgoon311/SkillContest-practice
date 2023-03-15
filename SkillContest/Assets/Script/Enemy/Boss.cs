using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class Boss : Entity
{
    [SerializeField] private GameObject bullet;
    [SerializeField] protected float atkSpeed;
    protected float atkTimer;
    protected int patternCount;

    [SerializeField] private ParticleSystem deadParticle;
    private Vector3 beforePos;
    private Vector3 movePos = new Vector3(0, 0, 13);
    private float moveTimer = 0;
    
    protected override void Awake()
    {
        base.Awake();
        beforePos = transform.position;
    }
    protected override void Move()
    {
        if (moveTimer > 1)
            return;

        moveTimer += Time.deltaTime;
        transform.position = Vector3.Lerp(beforePos, movePos, moveTimer);
    }
   
    protected override void Attack()
    {
        atkTimer += Time.deltaTime;
        if (atkTimer > atkSpeed)
        {
            StartCoroutine(AttackPattern());
            atkTimer = 0;
        }
    }
    protected virtual IEnumerator AttackPattern()
    {
        yield return null; 
    }
    protected virtual IEnumerator Dead()
    {
        GameManager.Instance.CameraShake(1, 1);

        deadParticle.Play();
        moveTimer = -0.5f;

        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            float dmg = other.GetComponent<Entity>().dmg;
            Hit(dmg);
            Destroy(other.gameObject);
        }
    }
}
