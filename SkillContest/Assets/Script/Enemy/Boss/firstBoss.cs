using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class firstBoss : Boss
{
    [SerializeField] private GameObject turret;
    [SerializeField] private Vector3[] spawnPos;
    private firstBossTurret[] turrets = new firstBossTurret[2];

    /// <summary>
    /// 0 : nomal
    /// 1 : homing
    /// 2 : ufo
    /// 3 : 360
    /// 4 : random 360
    /// </summary>
    [SerializeField] private GameObject[] bullet;
    protected override void Start()
    {
        for (int i = 0; i < 2; i++)
            turrets[i] = Instantiate(turret, transform.position + spawnPos[i], transform.rotation, transform).GetComponent<firstBossTurret>();

        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        HpUpdate();

    }
    private void HpUpdate()
    {
        _hp = turrets[0]._hp + turrets[1]._hp;
        _maxHp = turrets[0]._maxHp + turrets[1]._maxHp;
    }
    protected override IEnumerator AttackPattern()
    {
        int attackCount = Random.Range(0, patternCount);
        for (int i = 0; i < 2; i++)
        {
            if (turrets[i].isDie == true)
                continue;

            switch (attackCount)
            {
                case 0:
                    {
                        StartCoroutine(Homing(i));
                        break;
                    }
                case 1:
                    {
                        StartCoroutine(Spray(i, 1f));
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(TornadoAttack(i, 2, 2f));
                        break;
                    }
                case 3:
                    {
                        StartCoroutine(TornadoAttack(i, 3, 2f));
                        break;
                    }
                case 4:
                    {
                        StartCoroutine(TornadoAttack(i, 4, 2f));
                        break;
                    }
            }
        }


        yield return null;
    }
    private IEnumerator Homing(int turretIdx)
    {
        for (int i = 0; i < 2; i++)
        {
            Transform shotPos = turrets[turretIdx].shotPos[i].transform;
            for (int j = -1; j < 2; j++)
            {
                Quaternion rotation = Quaternion.Euler(new Vector3(0, shotPos.rotation.eulerAngles.y + (45 * j), 0));
                Instantiate(bullet[1], shotPos.position, rotation);
            }
        }

        yield return null;
    }
    private IEnumerator Spray(int turretIdx, float time)
    {
        float timer = 0;
        float randomRotete;
        Quaternion rotation;

        while (timer < time)
        {
            timer += Time.deltaTime * 10;


            for (int i = 0; i < 2; i++)
            {
                Vector3 shotPos = turrets[turretIdx].shotPos[i].transform.position;
                Vector3 dir = Player.instance.transform.position - shotPos;
                Quaternion lookRotate = Quaternion.LookRotation(dir);

                randomRotete = Random.Range(-30f, 30f);
                rotation = Quaternion.Euler(new Vector3(0, lookRotate.eulerAngles.y + randomRotete, 0));

                Instantiate(bullet[0], shotPos, rotation);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator TornadoAttack(int turretIdx, int bulletType, float time)
    {
        Transform shotPos = turrets[turretIdx].transform;
        Instantiate(bullet[bulletType], shotPos.position, shotPos.rotation);
        yield return null;
    }
}
