using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdBoss : Boss
{
    [SerializeField] private int patternCount2;
    [SerializeField] private float[] moveRemite;
    [Header("Drone")]
    [SerializeField] private GameObject droneGroup;
    [SerializeField] private GameObject[] drone;
    [SerializeField] private Vector3[] droneStartPos;
    [SerializeField] private Vector3[] droneEndPos;

    [Header("LaserInspector")]
    [SerializeField] private Material laserColor;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private GameObject[] shotPos = new GameObject[3];
    [SerializeField] private float waringLineDrawSpeed;
    private int beforeAttack2 = 0;

    private float moveTimer2 = 0;
    private float moveRandomTime = 5;
    protected override void Update()
    {
        base.Update();
        transform.LookAt(Player.instance.transform.position);
        if (EntityMnager.instance.isStop == false)
        {
            moveTimer2 += Time.deltaTime;
            if (moveTimer2 > moveRandomTime)
            {
                Vector3 movePos = new Vector3(Random.Range(moveRemite[0], moveRemite[1]), transform.position.y, transform.position.z);

                StartCoroutine(Move2(movePos));
                moveTimer2 = 0;
            }
        }
    }
    protected IEnumerator Move2(Vector3 movePos)
    {
        moveRandomTime = Random.Range(4f, 8f);
        float timer = 0;

        while (timer < 1)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, movePos, timer);
            yield return null;
        }
        StartCoroutine(AttackPattern());
        yield return null;
    }
    protected override IEnumerator AttackPattern()
    {
        int attackCount = beforeAttack;
        int attackCount2 = beforeAttack2;

        while (beforeAttack == attackCount)
            attackCount = Random.Range(0, patternCount);
        while (beforeAttack2 == attackCount2)
            attackCount2 = Random.Range(0, patternCount2);

        beforeAttack = attackCount;
        beforeAttack2 = attackCount2;

        /// <summary>
        /// bullets
        /// 0 : nomal
        /// 1 : homing
        /// 2 : ufo
        /// 3 : tonado
        /// 4 : warigari
        /// 5 : talon 
        /// 6 : Áö·Ú
        /// </summary>
        switch (attackCount)
        {
            case 0:
                {
                    StartCoroutine(Homing());
                    break;
                }
            case 1:
                {
                    StartCoroutine(Spray(2f));
                    break;
                }
            default:
                {
                    StartCoroutine(NomalAttack(attackCount));
                    break;
                }
        }
        switch (attackCount2)
        {
            case 0:
                {
                    StartCoroutine(ThreePronged());
                    break;
                }
            case 1:
                {
                    StartCoroutine(DroneAttack());
                    break;
                }
            case 2:
                {
                    StartCoroutine(DroneAttack2());
                    break;
                }
            case 3:
                {
                    SpinAttack();
                    break;
                }
            
        }

        yield return null;
    }

    #region gun
    private IEnumerator Homing()
    {
        for (int i = 1; i < 3; i++)
        {
            Transform shotTransform = shotPos[i].transform;
            for (int j = -1; j < 2; j++)
            {
                Quaternion rotation = Quaternion.Euler(new Vector3(0, shotTransform.rotation.eulerAngles.y + (45 * j), 0));
                Instantiate(bullet[1], shotTransform.position, rotation);
            }
        }

        yield return null;
    }
    private IEnumerator Spray(float time)
    {
        float timer = 0;
        float randomRotete;
        Quaternion rotation;

        while (timer < time)
        {
            timer += Time.deltaTime * 10;


            for (int i = 1; i < 3; i++)
            {
                Vector3 pos = shotPos[i].transform.position;
                Vector3 dir = Player.instance.transform.position - pos;
                Quaternion lookRotate = Quaternion.LookRotation(dir);

                randomRotete = Random.Range(-50f, 50f);
                rotation = Quaternion.Euler(new Vector3(0, lookRotate.eulerAngles.y + randomRotete, 0));

                Instantiate(bullet[0], pos, rotation);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    private IEnumerator NomalAttack(int bulletType)
    {
        for (int i = 1; i < 3; i++)
        {
            Transform pos = shotPos[i].transform;
            Instantiate(bullet[bulletType], pos.position, pos.rotation);
            yield return null;
        }
    }
    #endregion
    #region Laser
    protected IEnumerator ThreePronged()
    {
        transform.LookAt(Player.instance.transform.position);

        shotPos[0].transform.localRotation = Quaternion.LookRotation(shotPos[0].transform.position, Player.instance.transform.position);
        shotPos[0].transform.localRotation = Quaternion.Euler(new Vector3(0, shotPos[0].transform.localRotation.eulerAngles.y - 60f, 0));
        for (int j = 0; j < 3; j++)
        {
            shotPos[0].transform.localRotation = Quaternion.Euler(new Vector3(0, shotPos[0].transform.localRotation.eulerAngles.y + 30f, 0));
            StartCoroutine(DrawWaringLine(shotPos[0].transform.position, shotPos[0].transform.rotation));
        }

        yield return new WaitForSeconds(1f);

        yield return null;
    }
    protected IEnumerator DroneAttack()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * 2;
            for (int i = 0; i < 6; i++)
                drone[i].transform.position = Vector3.Lerp(droneStartPos[i], droneEndPos[i], timer);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 6; i++)
            StartCoroutine(DrawWaringLine(drone[i].transform.position, drone[i].transform.localRotation));

        yield return new WaitForSeconds(3f);

        while (timer > 0)
        {
            timer -= Time.deltaTime * 2;
            for (int i = 0; i < 6; i++)
            {
                drone[i].transform.position = Vector3.Lerp(droneStartPos[i], droneEndPos[i], timer);
            }
            yield return null;
        }
        yield break;
    }
    protected IEnumerator DroneAttack2()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * 2;
            for (int i = 0; i < 6; i++)
                drone[i + 6].transform.position = Vector3.Lerp(droneStartPos[i], droneEndPos[i], timer);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 6; i++)
        {
            drone[i + 6].transform.LookAt(Player.instance.transform.position);
            StartCoroutine(DrawWaringLine(drone[i + 6].transform.position, drone[i + 6].transform.localRotation));
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2.5f);

        while (timer > 0)
        {
            timer -= Time.deltaTime * 2;
            for (int i = 0; i < 6; i++)
            {
                drone[i + 6].transform.position = Vector3.Lerp(droneStartPos[i], droneEndPos[i], timer);
            }
            yield return null;
        }

        yield break;
    }
    protected void SpinAttack()
    {
        Instantiate(bullet[7], transform.position, transform.rotation);
    }
    public IEnumerator DrawWaringLine(Vector3 pos, Quaternion rotation)
    {
        GameObject lineObeject;

        lineObeject = Instantiate(bullet[6], pos, rotation);
        lineObeject.GetComponent<Rigidbody>().velocity = lineObeject.transform.forward * waringLineDrawSpeed;

        yield return new WaitForSeconds(1f);
        StartCoroutine(DrawLaser(pos, lineObeject.transform.position));
        Destroy(lineObeject);
        yield return null;
    }
    public IEnumerator DrawLaser(Vector3 startPos, Vector3 endPos)
    {

        LineRenderer line = new GameObject("layer").AddComponent<LineRenderer>();
        line.material = laserColor;

        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);

        Quaternion rotate = Quaternion.LookRotation(startPos, endPos);

        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            line.SetWidth(timer, timer * 2f);
            if (Physics.BoxCast(startPos, Vector3.one / 2, endPos, rotate, 20, playerLayerMask))
                Player.instance.Hit(dmg);
            yield return null;
        }

        while (timer > 0)
        {
            timer -= Time.deltaTime * 3;
            line.SetWidth(timer, timer * 2f);
            yield return null;
        }
        Destroy(line.gameObject);
        yield return null;
    }
    #endregion
}
