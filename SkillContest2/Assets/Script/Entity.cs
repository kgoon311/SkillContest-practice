using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public abstract class Entity : MonoBehaviour
{
    [Header("Info")]
    private float hp;
    public float _hp
    {
        get { return hp; }
        set
        {
            hp = value;
            StartCoroutine(HitAnim());
            if (hp <= 0)
                Dead();
        }
    }
    public float maxHp;
    [SerializeField] protected float dmg;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackSpeed;
    protected float attackTimer;

    protected virtual void Awake()
    {
        hp = maxHp;
    }
    protected virtual void Update()
    {
        myUpdate();
    }
    protected virtual void myUpdate()
    {
        if (EntityManager.Instance.isStop)
            return;

        Move();
        Attack();
    }
    protected virtual void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackSpeed)
        {
            attackTimer = 0;
            StartCoroutine(AttackPattern());
        }
    }
    protected virtual IEnumerator AttackPattern()
    {

        yield return null;
    }
    protected virtual IEnumerator HitAnim()
    {
        yield return null;
    }
    protected abstract void Move();
    protected abstract void Dead();
    
}