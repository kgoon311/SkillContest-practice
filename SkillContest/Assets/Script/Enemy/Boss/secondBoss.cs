using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class secondBoss : Boss
{
    [Header("Drone")]
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
        base.Start();
    }
    protected override IEnumerator AttackPattern()
    {
        int attackCount = Random.Range(0, patternCount);
        /// <summary>
        /// bullets
        /// 0 : waringLine
        /// 1 : layerBoom
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
                    Grenade();
                    break;
                }
        }



        yield return null;
    }
    protected IEnumerator ThreePronged()
    {
        List<GameObject> list = new List<GameObject>();

        transform.LookAt(Player.instance.transform.position);

        for (int i = 0; i < 2; i++)
        {
            shotPos[i].transform.rotation = Quaternion.LookRotation(shotPos[i].transform.position, Player.instance.transform.position);
            shotPos[i].transform.Rotate(new Vector3(0, -30f, 0));
            for (int j = 0; j < 3; j++)
            {
                shotPos[i].transform.Rotate(new Vector3(0, 30 * j, 0));
                list.Add(DrawWaringLine(i));
            }
        }

        yield return new WaitForSeconds(waringLineDrawSpeed);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                DrawLaser(shotPos[i].transform.position, list[j + (i * 3)].transform.position);
                Destroy(list[j + (i * 3)]);
            }
        }
        yield return null;
    }
    protected IEnumerator DroneAttack()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * 2;
            for (int i = 0; i < 6; i++)
            {
                drone[i].transform.position = Vector3.Lerp(droneStartPos[i], droneEndPos[i], timer);
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 6; i++)
        {
            DrawLaser(drone[i].transform.position, drone[i].transform.position);
        }

        yield return new WaitForSeconds(0.5f);
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

    protected void Grenade()
    {
        for (int i = 0; i < 2; i++)
        {
            shotPos[i].transform.rotation = Quaternion.Euler(0, 180, 0);
            Instantiate(bullet[1], shotPos[i].transform.position, shotPos[0].transform.rotation);
        }
    }

    protected IEnumerator DrawLaser(Vector3 startPos, Vector3 endPos)
    {

        LineRenderer line = new GameObject("layer").AddComponent<LineRenderer>();

        line.materials[0] = laserColor;
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);

        Quaternion rotate = Quaternion.LookRotation(startPos, endPos);
        if (Physics.BoxCast(startPos, Vector3.one / 2, endPos, rotate, 20, playerLayerMask))
            Player.instance.Hit(dmg);

        yield return new WaitForSeconds(1f);
        Destroy(line.gameObject);
        yield return null;
    }
    protected GameObject DrawWaringLine(int gunIdx)
    {
        GameObject lineObeject;

        lineObeject = Instantiate(bullet[0], shotPos[gunIdx].transform.position, shotPos[gunIdx].transform.rotation);
        lineObeject.GetComponent<Rigidbody>().velocity = Vector3.forward * Time.deltaTime * waringLineDrawSpeed;

        return lineObeject;
    }
}
