using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdBoss : Boss
{
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

    #region gun
    private IEnumerator Homing(int turretIdx)
    {
        for (int i = 0; i < 2; i ++)
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
                Vector3 pos = shotPos[i].transform.position;
                Vector3 dir = Player.instance.transform.position - pos;
                Quaternion lookRotate = Quaternion.LookRotation(dir);

                randomRotete = Random.Range(-30f, 30f);
                rotation = Quaternion.Euler(new Vector3(0, lookRotate.eulerAngles.y + randomRotete, 0));

                Instantiate(bullet[0], pos, rotation);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator TornadoAttack(int turretIdx, int bulletType, float time)
    {
        for(int i = 0; i < 2; i++)
        {
            Transform pos = shotPos[i].transform;
            Instantiate(bullet[bulletType], pos.position, pos.rotation);
            yield return null;
        }
    }
    #endregion
}
