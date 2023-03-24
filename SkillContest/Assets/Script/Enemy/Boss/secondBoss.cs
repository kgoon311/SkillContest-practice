using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class secondBoss : Boss
{
    [Header("Drone")]
    [SerializeField] private GameObject droneGroup;
    [SerializeField] private GameObject[] drone;
    [SerializeField] private Vector3[] droneStartPos;
    [SerializeField] private Vector3[] droneEndPos;

    [Header("LaserInspector")]
    [SerializeField] private Material laserColor;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private GameObject[] shotPos = new GameObject[2];
    [SerializeField] private float waringLineDrawSpeed;

    protected override void Start()
    {
        playerLayerMask = LayerMask.GetMask("Player");
        droneGroup.SetActive(true);
        base.Start();
    }
    protected override void Update()
    {
        for (int i = 0; i < 6; i++)
            drone[i + 6].transform.LookAt(Player.instance.transform.position);

        base.Update();
    }
    protected override IEnumerator AttackPattern()
    {
        int attackCount = beforeAttack;

        while (beforeAttack == attackCount)
            attackCount = Random.Range(0, patternCount);

        beforeAttack = attackCount;

        /// <summary>
        /// bullets
        /// 0 : waringLine
        /// 1 : grenade
        /// 2 : spinBall
        /// </summary>
        /// 
        switch (attackCount)
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
                    Grenade();
                    break;
                }
            case 4:
                {
                    SpinAttack();
                    break;
                }
        }



        yield return null;
    }

    #region attackPattern
    protected IEnumerator ThreePronged()
    {
        transform.LookAt(Player.instance.transform.position);

        for (int i = 0; i < 2; i++)
        {
            shotPos[i].transform.localRotation = Quaternion.LookRotation(shotPos[i].transform.position, Player.instance.transform.position);
            shotPos[i].transform.localRotation = Quaternion.Euler(new Vector3(0, shotPos[i].transform.localRotation.eulerAngles.y - 60f, 0));
            for (int j = 0; j < 3; j++)
            {
                shotPos[i].transform.localRotation = Quaternion.Euler(new Vector3(0, shotPos[i].transform.localRotation.eulerAngles.y + 30f, 0));
                StartCoroutine(DrawWaringLine(shotPos[i].transform.position, shotPos[i].transform.rotation));
            }
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
        Instantiate(bullet[2], transform.position, transform.rotation);
    }
    protected void Grenade()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        Grenade grenade =  Instantiate(bullet[1], transform.position, transform.rotation).GetComponent<Grenade>();
        grenade.secondBoss = this;
    }

    public IEnumerator DrawWaringLine(Vector3 pos, Quaternion rotation)
    {
        GameObject lineObeject;

        lineObeject = Instantiate(bullet[0], pos, rotation);
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

        yield return new WaitForSeconds(0.5f);

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
