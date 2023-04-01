using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser2 : EnemyBullet
{
    [SerializeField] private GameObject[] endPos = new GameObject[4];
    protected override IEnumerator AttackPattern()
    {
        speed = 0;
        for (int i = 0; i < 4; i++)
            SecondBoss.Instance.DrawLaser(transform.position, endPos[i].transform.position,2f);

        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / 2;
            transform.Rotate(Vector3.up);
        }
        yield return null;
    }
}
