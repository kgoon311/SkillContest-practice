using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warm : Enemy
{
    [SerializeField] private GameObject[] bodys = new GameObject[4];

    protected override IEnumerator AttackPattern()
    {
        for(int i = 0; i< 4; i++)
        {
            Instantiate(bullet, bodys[i].transform.position,
                                Quaternion.Euler(0, bodys[i].transform.rotation.eulerAngles.y + 90, 0));
            Instantiate(bullet, bodys[i].transform.position,
                                Quaternion.Euler(0, bodys[i].transform.rotation.eulerAngles.y - 90, 0));
        }
        yield return null; 
    }
}
