using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FirstBoss : Enemy
{
    private FirstBossTurret[] turretGroup = new FirstBossTurret[2];
    [SerializeField] private GameObject TurretObejct;
    [SerializeField] private Vector3[] turretSpawnPos;
    [SerializeField] private Vector3[] movePos;
    private int beforeAttack = -1;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < 2; i++)
            turretGroup[i] = Instantiate(TurretObejct, transform.position + turretSpawnPos[i], Quaternion.Euler(0, 180, 0), transform).GetComponent<FirstBossTurret>();
    }
    private void Start()
    {
       
    }
    protected override void Attack()
    {
        maxHp = turretGroup[0]._hp + turretGroup[1]._hp;
        base.Attack();
       
    }
    protected override void Update()
    {
        base.Update();
        _hp = turretGroup[0]._hp + turretGroup[1]._hp;
    }
    protected override IEnumerator AttackPattern()
    {
        int attack = Random.Range(0, 6);

        while (attack == beforeAttack)
            attack = Random.Range(0, 6);

        beforeAttack = attack;

        /// <summary>
        /// 
        /// 0 : spriy
        /// 1 : homing
        /// 2 : ufo
        /// 3 : tonado
        /// </summary>

        switch (attack)
        {
            case 0:
                {
                    StartCoroutine(Spriy(3f, 0.4f, 30f));
                    break;
                }
            case 1: 
                {
                    MultyShot(1, 3, 40);
                    break; 
                }
            case 2:
                {
                    NomalShot(2, 1, 0);
                    break;
                }
            case 3:
                {
                    NomalShot(3, 1, 0);
                    break;
                }
            case 4:
                {
                    NomalShot(3, 3, 60);
                    break;
                }
        }

        yield return null;
    }
    protected IEnumerator Spriy(float time, float shotDelay, float spriyRotate)
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / time * (shotDelay * 100);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2 && turretGroup[i].isDie == false; j++)
                {
                    Transform shotPos = turretGroup[i].shotPos[j].transform;
                    Instantiate(bullet[0], shotPos.position,
                        Quaternion.Euler(0, shotPos.rotation.eulerAngles.y
                        + Random.Range(-spriyRotate, spriyRotate), 0));
                }
            }
            yield return new WaitForSeconds(shotDelay);
        }
        yield return null;
    }
    private void NomalShot(int bulletIdx, int shotCount, float RotateDis)
    {
        for (int i = 0; i < 2; i++)
        {
            Transform shotPos = turretGroup[i].transform;
            for (int j = -shotCount / 2; j <= shotCount / 2; j++)
            {
                Debug.Log(j);
                Instantiate(bullet[bulletIdx], shotPos.position, Quaternion.Euler(0, shotPos.rotation.eulerAngles.y + RotateDis * j, 0));
            }
        }
    }
    private void MultyShot(int bulletIdx, int shotCount, float RotateDis)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2 && turretGroup[i].isDie == false; j++)
            {
                Transform shotPos = turretGroup[i].shotPos[j].transform;
                for (int k = -shotCount / 2; k < shotCount / 2; k++)
                {
                    Instantiate(bullet[bulletIdx], shotPos.position, Quaternion.Euler(0, shotPos.rotation.eulerAngles.y + RotateDis * k, 0));
                }
            }
        }
    }
    protected override void Move()
    {
        for (int i = 0; i < 2; i++)
            turretGroup[i].transform.position = Vector3.Lerp(turretGroup[i].transform.position
                                               , transform.position + movePos[i], Time.deltaTime);
    }
    protected override void Dead()
    {
        InGameManager.Instance.BossDead();
    }
    protected override void DeadCountdown()
    {
    }
}
