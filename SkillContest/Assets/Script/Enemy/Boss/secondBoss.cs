using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline;
using UnityEngine;

public class secondBoss : Boss
{
    private LineRenderer layerLine;

    [SerializeField] private GameObject[] shotPos = new GameObject[2];
    [SerializeField] private float waringLineDrawSpeed;
    /// <summary>
    /// bullets
    /// 0 : waringLine
    /// 1 : layerBoom
    /// </summary>
    /// 
    protected override void Start()
    {
        layerLine = GetComponent<LineRenderer>();
        base.Start();
    }
    protected IEnumerator ThreePronged()
    {
        List<GameObject> list = new List<GameObject>();
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
                LineRenderer line = new GameObject("layer").GetComponent<LineRenderer>();
                line.SetPosition(1, list[j + (i * 3)].transform.position);
            }
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Destroy(list[j + (i * 3)]);
            }
        }

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
