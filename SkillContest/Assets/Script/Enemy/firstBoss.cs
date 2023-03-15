using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstBoss : Boss
{
    [SerializeField] private GameObject[] turrets = new GameObject[2];

    /// <summary>
    /// 0 : nomal
    /// 1 : homing
    /// 2 : ufo
    /// 3 : 360
    /// 4 : random 360
    /// </summary>
    [SerializeField] private GameObject[] bullet;
    
    protected override IEnumerator AttackPattern()
    {
        return base.AttackPattern();
    }
    private IEnumerator Homing()
    {
        for(int i = 0; i < 2; i++)
        {
            
        }
        yield return null;
    }
}
