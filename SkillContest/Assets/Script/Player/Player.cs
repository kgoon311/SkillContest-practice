using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    public static Player instance;

    [SerializeField] private float oil;
    [SerializeField] private float maxOil;
    public float _oil { get { return oil; } set { oil = value; } }
    public float _maxOil { get { return maxOil; } set { maxOil = value; } }

    [SerializeField] private float inviTime;
    [Header("MoveRimite")]
    [SerializeField] private Vector2[] moveRimite = new Vector2[2]; 
    [SerializeField] private float rotateSpeed;

    [Header("Attack")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private int attackLv;
    [SerializeField] private GameObject[] bullet = new GameObject[3];
    private float attackTimer;

    [Header("Drone")]
    public GameObject targetObject;
    [SerializeField] private GameObject[] droneGroup = new GameObject[4];
    [SerializeField] private GameObject droneBullet;
    [SerializeField] private Vector3[] dronePos = new Vector3[4]; 
    [SerializeField] private int droneCount;

    protected override void Awake()
    {
        instance = this; 
        base.Awake();
    }
    protected override void Start()
    {
        for (int i = 0; i < 3; i++)

        base.Start();
    }
    protected override void Update()
    {
        base.Update();

        if (hp <= 0 || oil <= 0)
            GameOver();
    }

    protected override void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontal * Time.deltaTime * speed, 0 , vertical * Time.deltaTime * speed);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, moveRimite[0].x, moveRimite[1].x),0, 
                                         Mathf.Clamp(transform.position.z, moveRimite[0].y, moveRimite[1].y));

        for(int i = 0; i<droneCount;i++)
        {
            droneGroup[i].transform.position = Vector3.Lerp(droneGroup[i].transform.position, 
                                                            transform.position + dronePos[i], Time.deltaTime * 3);
        }

        transform.rotation = Quaternion.Lerp(Quaternion.Euler(0,0,0),
                                            Quaternion.Euler(0,0,-25 * horizontal), Mathf.Abs(horizontal));
    }

    protected override void Attack()
    {
        if (Input.GetKey(KeyCode.Space))
            attackTimer += Time.deltaTime;
       

        if (attackTimer >= attackSpeed)
        {
            ShotBullet();
            attackTimer = 0;
        }
    }
    private void ShotBullet()
    {
        switch (attackLv)
        {
            case 0:
                {
                    Instantiate(bullet[0],transform.position,transform.rotation);
                    break;
                }
            case 1:
                {
                    Instantiate(bullet[0], transform.position + new Vector3(0.5f,0,0)   , Quaternion.Euler(0,0,0));
                    Instantiate(bullet[0], transform.position + new Vector3(-0.5f, 0, 0), Quaternion.Euler(0, 0, 0));
                    break;
                }
            case 2:
                {
                    Instantiate(bullet[0], transform.position + new Vector3(-0.6f, 0, 0), Quaternion.Euler(0,0,0));
                    Instantiate(bullet[1], transform.position + new Vector3(0, 0, 0.2f) , Quaternion.Euler(0,0,0));
                    Instantiate(bullet[0], transform.position + new Vector3(0.6f, 0, 0) , Quaternion.Euler(0, 0, 0));
                    break;
                }
            case 3:
                {
                    Instantiate(bullet[1], transform.position + new Vector3(-0.7f, 0, 0)  , Quaternion.Euler(0,0,0));
                    Instantiate(bullet[2], transform.position + new Vector3(0, 0, 0.3f) , Quaternion.Euler(0,0,0));
                    Instantiate(bullet[1], transform.position + new Vector3(0.7f, 0, 0)   , Quaternion.Euler(0, 0, 0));
                    break;
                }
        }
        for(int i = 0;i<droneCount;i++)
        {
            DroneBullet bullet = Instantiate(droneBullet, droneGroup[i].transform.position, transform.rotation).GetComponent<DroneBullet>();
            bullet.targetObject = targetObject;
        }
    }
  
    public override void Hit(float hitDmg)
    {
        base.Hit(hitDmg);
        if(isInvi == false)
            GameManager.Instance.CameraShake(0.5f, 1);
        Invi(inviTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            float dmg = other.GetComponent<Entity>().dmg;
            Hit(dmg);

            Destroy(other.gameObject);
        }
        if(other.CompareTag("Item"))
        {

        }
    }
    private void GameOver()
    {

    }
}
