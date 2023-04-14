using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : Entity 
{
    public static Player Instance { get; set; }
    private float oil;
    public float _oil
    {
        get { return oil; }
        set
        {
            oil = value;
            if (oil <= 0)
                Dead();
        }
    }
    public float maxOil;
    [SerializeField] private Vector3[] moveRemete;
    [SerializeField] private Vector3[] lsatBossMoveRemete;
    [SerializeField] private float rotationSpeed;
    private float rotationTimer;
    public float[] skillCool;
    public float[] skillCoolTimer;
   
    [Header("Attack")]
    [SerializeField] private GameObject[] shotPos;
    [SerializeField] private int attackLv;
    [SerializeField] private GameObject[] bullet;
    [Header("Drone")]
    [SerializeField] private GameObject[] droneGroup;
    [SerializeField] private Vector3[] dronePos;
    [SerializeField] private float droneSpeed;
    [SerializeField] private int droneIdx = 0;
    [Header("Skill")]
    [SerializeField] private ParticleSystem HealEffect;
    [SerializeField] private ParticleSystem BoomEffect;
    [SerializeField] private GameObject targetParticle;
    public GameObject targetObject;
    
    private string inviTag;
    private string playerTag;

    private float inviTimer;
    private GameObject hitObject;
    private GameObject inviObject;
    protected override void Awake()
    {
        base.Awake();
        Instance = this; 
        inviTag = "Invi";
        playerTag = "Player";
    }
    private void Start()
    {
        
    }
    protected override void Update()
    {
        base.Update();
        moveRemete[0].x = -(5f + (3f / 8f) * transform.position.z);
        moveRemete[1].x = 5f + (3f / 8f) * transform.position.z;
    }
    protected override void myUpdate()
    {
        base.myUpdate();
        Skill();
        for (int i = 0; i < 3; i++)
            skillCoolTimer[i] -= Time.deltaTime;
    }
    protected override void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(x , 0, y) * Time.deltaTime * speed;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, moveRemete[0].x, moveRemete[1].x), 0,
                                         Mathf.Clamp(transform.position.z, moveRemete[0].z, moveRemete[1].z));

        rotationTimer = Mathf.Lerp(rotationTimer, x * rotationSpeed, Time.deltaTime);
        rotationTimer = Mathf.Clamp(rotationTimer, -40f, 40f);
        transform.rotation = Quaternion.Euler(0, 0, rotationTimer);
        for (int i = 0; i < droneIdx; i++)
        {
            droneGroup[i].transform.position = Vector3.Lerp(droneGroup[i].transform.position, 
                                                            transform.position + dronePos[i], 
                                                            Time.deltaTime * 3);
            if (targetObject != null)
                droneGroup[i].transform.LookAt(targetObject.transform);
            else
                droneGroup[i].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    protected override void Attack()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            attackTimer += Time.deltaTime;
        }

        if (attackTimer > attackSpeed)
        {
            attackTimer = 0;
            StartCoroutine(AttackPattern());
        }
    }
    private void Skill()
    {
        if (targetObject != null)
        {
            targetParticle.SetActive(true);
            targetParticle.transform.position = targetObject.transform.position; 
        }
        else
            targetParticle.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Z) && skillCoolTimer[0] < 0) Targeting();

    }
    private void Targeting()
    {
        RaycastHit hit;
        if(Physics.BoxCast(transform.position,Vector3.one,Vector3.forward,out hit,transform.rotation,10000,LayerMask.GetMask("Enemy")))
            targetObject = hit.transform.gameObject;
    }
    private void Heal()
    {
        _hp += 50;
        HealEffect.Play();
    }

    private void Boom()
    {
        List<Entity> enemys = EntityManager.Instance.enemys;
        enemys.RemoveAll(e => e == null);
        BoomEffect.Play();
        foreach (Entity ent in enemys)
        {
            ent._hp -= dmg;
        }
    }
    protected override IEnumerator AttackPattern()
    {
        for(int i = 0; i < droneIdx; i++)
        {
            
            Instantiate(bullet[3], droneGroup[i].transform.position, droneGroup[i].transform.rotation);
        }
        switch (attackLv)
        { 
            case 0:
                {
                    Instantiate(bullet[0], shotPos[0].transform.position, transform.rotation);
                    break;
                }
            case 1:
                {
                    Instantiate(bullet[0], shotPos[1].transform.position, transform.rotation);
                    Instantiate(bullet[0], shotPos[2].transform.position, transform.rotation);
                    break;
                }
            case 2:
                {
                    Instantiate(bullet[0], shotPos[1].transform.position, transform.rotation);
                    Instantiate(bullet[1], shotPos[0].transform.position, transform.rotation);
                    Instantiate(bullet[0], shotPos[2].transform.position, transform.rotation);
                    break;
                }
            case 3:
                {
                    Instantiate(bullet[1], shotPos[1].transform.position, transform.rotation);
                    Instantiate(bullet[2], shotPos[0].transform.position, transform.rotation);
                    Instantiate(bullet[1], shotPos[2].transform.position, transform.rotation);
                    break;
                }
            
        }
        yield return null;
    }
    protected override void Dead()
    {
    }
   
    protected override IEnumerator HitAnim()
    {
        hitObject.SetActive(true);

        StartCoroutine(Invi(0.5f));
        yield return new WaitForSeconds(0.5f);
        
        hitObject.SetActive(false);
        yield return null;
    }
    protected IEnumerator Invi(float timer)
    {
        if(inviTimer > timer)
            yield break;
        inviTimer = timer;
        tag = inviTag;
        inviObject.SetActive(true);
        
        while(inviTimer > 0)
        {
            inviTimer -= Time.deltaTime;
            yield return null;
        }

        tag = playerTag;
        inviObject.SetActive(false);
        
        yield return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "I_Upgrade":
                if (attackLv == 3)
                {
                    InGameManager.Instance.AddScore(10);
                    break;
                }
                InGameManager.Instance.ItemCount++;
                attackLv++;
                break;
            case "I_AddDrone":
                if (droneIdx == 4)
                {
                    InGameManager.Instance.AddScore(10);
                    break;
                }
                InGameManager.Instance.ItemCount++;
                droneIdx++;
                break;
            case "I_Invi":
                InGameManager.Instance.ItemCount++;
                Invi(2f);
                break;
            case "I_Heal":
                InGameManager.Instance.ItemCount++;
                _hp += 10;
                break;
            case "I_Oil":
                InGameManager.Instance.ItemCount++;
                _oil += 40;
                break;
        }
    }
}
