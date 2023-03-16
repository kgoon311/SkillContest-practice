using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
            turrets[i] = Instantiate(turret, spawnPos[i], transform.rotation, transform).GetComponent<firstBossTurret>();

        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        HpUpdate();

        transform.LookAt(Player.instance.transform.position);
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

            for (int j = -1; j < 2; j++)
            {
                switch (attackCount)
                {
                    case 0:
                        {
                            StartCoroutine(Homing(i));
                            break;
                        }
                    case 1:
                        {
                            StartCoroutine(Spray(i,1f));
                            break;
                        }
                }
            }
        }


        yield return null;
    }
    private IEnumerator Homing(int turretIdx)
    {
        Transform shotPos = turrets[turretIdx].transform;
        for (int j = -1; j < 2; j++)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, shotPos.rotation.y + (45 * j), 0));
            Instantiate(bullet[1], shotPos.transform.position, rotation);
        }
        yield return null;
    }
    private IEnumerator Spray(int turretIdx , float time)
    {
        float timer = 0;

        Transform shotPos = turrets[turretIdx].transform;
        Quaternion rotation;

        while (timer < time)
        {
            timer += Time.deltaTime;

            float randomRotete = Random.Range(-30f, 30f);
            rotation = Quaternion.Euler(new Vector3(0, shotPos.rotation.y + randomRotete, 0));

            Instantiate(bullet[0], shotPos.position, rotation);
            yield return new WaitForSeconds(0.05f);
        }
    }

}
