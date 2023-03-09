using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected Renderer meshRenderer;
    public Material hitMaterial;

    private bool isPlayingHitAnim;
    public bool isInvi;

    [Header("Inspector")]
    [SerializeField]
    protected float hp;
    protected float maxHp;
    public float _hp { get { return hp; } set { hp = value; } }
    public float _maxHp { get { return maxHp; } set { maxHp = value; } }

    [SerializeField]
    protected float speed;
    public float dmg;

    protected virtual void Awake()
    {
        meshRenderer = gameObject.GetComponent<Renderer>();
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
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(HitAnime());
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
        if (isPlayingHitAnim == true)
            yield break;

        isPlayingHitAnim = true;
        meshRenderer.material = hitMaterial;

        yield return new WaitForSeconds(0.6f);

        isPlayingHitAnim = false;
        meshRenderer.materials = new Material[0];
    }
    protected IEnumerator Invi(float inviTime)
    {
        isInvi = true;
        yield return new WaitForSeconds(inviTime);
        isInvi = false;
    }
}
