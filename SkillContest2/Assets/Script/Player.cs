using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private Vector3[] moveRemete;
    [SerializeField] private Vector3[] lsatBossMoveRemete;
    
    [Header("Attack")]
    [SerializeField] private Vector3[] shotPos;
    [SerializeField] private GameObject[] bullet;
    [SerializeField] private int attackLv;
    [SerializeField] private GameObject[] droneGroup;
         
    private Renderer renderer;
    private Material hitMaterial;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        hitMaterial = EntityManager.Instance.hitMatarial;
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void myUpdate()
    {
        base.myUpdate();
    }
    protected override void AttackPattern()
    {
        switch (attackLv)
        { 
            case 0:
                {
                    Instantiate(bullet[0], transform.position, transform.rotation);
                    break;
                }
            case 1:
                {
                    Instantiate(bullet[0], transform.position + shotPos[0], transform.rotation);
                    Instantiate(bullet[0], transform.position + shotPos[2], transform.rotation);
                    break;
                }
            case 2:
                {
                    Instantiate(bullet[0], transform.position + shotPos[0], transform.rotation);
                    Instantiate(bullet[1], transform.position + shotPos[1], transform.rotation);
                    Instantiate(bullet[0], transform.position + shotPos[2], transform.rotation);
                    break;
                }
            case 3:
                {
                    Instantiate(bullet[1], transform.position + shotPos[0], transform.rotation);
                    Instantiate(bullet[2], transform.position + shotPos[1], transform.rotation);
                    Instantiate(bullet[1], transform.position + shotPos[2], transform.rotation);
                    break;
                }
            
        }
    }
    protected override void Dead()
    {
    }
    protected override void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(x, 0, y);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, moveRemete[0].x, moveRemete[1].x), 0, 
                                         Mathf.Clamp(transform.position.z, moveRemete[0].z, moveRemete[1].z));
    }
    protected override void HitAnim()
    {
        base.HitAnim();
    }
}
