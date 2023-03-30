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

   
    [Header("Attack")]
    [SerializeField] private Vector3[] shotPos;
    [SerializeField] private int attackLv;
    [SerializeField] private GameObject[] bullet;
    [Header("Drone")]
    [SerializeField] private GameObject[] droneGroup;
    [SerializeField] private int droneIdx = 0;
    private GameObject targetObject;
    
    private string inviTag;
    private string playerTag;

    private float inviTimer;
    private GameObject hitObject;
    private GameObject inviObject;
    protected override void Awake()
    {
        base.Awake();
        inviTag = "Invi";
        playerTag = "Player";
    }
    private void Start()
    {
        GameManger.Instance.player = this;
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void myUpdate()
    {
        base.myUpdate();
    }
    protected override void Attack()
    {
        if (Input.GetKey(KeyCode.Space))
            attackTimer += Time.deltaTime;

        if (attackTimer > attackSpeed)
        {
            attackTimer = 0;
            AttackPattern();
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
        yield return null;
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

        rotationTimer = Mathf.Lerp(rotationTimer,x * rotationSpeed,Time.deltaTime);
        rotationTimer = Mathf.Clamp(rotationTimer,-40f, 40f);
        transform.rotation = Quaternion.Euler(0, 0, rotationTimer);
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
                    GameManger.Instance.score += 10;
                    break;
                }
                attackLv++;
                break;
            case "I_AddDrone":
                if (droneIdx == 4)
                {
                    GameManger.Instance.score += 10;
                    break;
                }
                droneIdx++;
                break;
            case "I_Invi":
                Invi(2f);
                break;
            case "I_Heal":
                _hp += 10;
                break;
            case "I_Oil":
                _oil += 40;
                break;
        }
    }
}
