using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warm : Enemy
{
    [SerializeField] private GameObject[] shotPos;

    protected override IEnumerator AttackPattern()
    {
        for(int i = 0;i<3;i++)
        {
            Instantiate(bullet[0], shotPos[i * 2].transform.position, shotPos[i * 2].transform.localRotation);
            Instantiate(bullet[0], shotPos[i * 2 + 1].transform.position, shotPos[i * 2 + 1].transform.localRotation);
            Instantiate(bullet[0], shotPos[6].transform.position, shotPos[6].transform.localRotation);
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }
}
