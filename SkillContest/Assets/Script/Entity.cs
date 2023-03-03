using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected MeshRenderer meshRenderer;
    private Material hitMaterial;

    [Header("Inspector")]
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float dmg;
    [SerializeField]
    protected float speed;

    protected bool isInvi;

    
    protected virtual void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.materials = new Material[2];
    }
    protected virtual void Start()
    {
        hitMaterial = EntityMnager.instance.hitMaterial;
    }
    protected virtual void Update()
    {
        if(EntityMnager.instance.isStop == false)
        {
            Move();
            Attack();
        }
    }

    protected abstract void Move();
    protected abstract void Attack();


    public virtual void Hit(float hitDmg)
    {
        if(isInvi == false)
        {
            hp -= hitDmg;
            StartCoroutine(HitAnime());
        }
    }
    protected IEnumerator HitAnime()
    {
        meshRenderer.materials[1] = hitMaterial;
        yield return new WaitForSeconds(0.3f);
        meshRenderer.materials[0] = null;
    }
    protected IEnumerator Inui(float inviTime)
    {
        isInvi = true;
        yield return new WaitForSeconds(inviTime);
        isInvi = false;
    }
}
