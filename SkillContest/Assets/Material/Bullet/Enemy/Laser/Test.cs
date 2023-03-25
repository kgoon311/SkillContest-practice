using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public secondBoss secondBossScript;
    void Start()
    {
        StartCoroutine(secondBossScript.DrawWaringLine(transform.position, transform.rotation));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
