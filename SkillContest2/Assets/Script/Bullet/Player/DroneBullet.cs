using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet : PlayerBullet
{
    [SerializeField] private float minDis;
    [SerializeField] private float maxDis;
    float bezierTimer = 0;
    Vector3 beforePos;
    Transform targetPos;
    Vector3 random;

    private void Start()
    {
        beforePos = transform.position;
        targetPos = Player.Instance.targetObject.transform;
        random = new Vector3(Random.Range(minDis, maxDis), Random.Range(minDis, maxDis));
    }
    protected override void Move()
    {
        if (Player.Instance.targetObject != null)
        {
            Vector3 center = (beforePos + targetPos.position) / 2 + random;
            Bezier(beforePos, center, targetPos.position);
            bezierTimer += Time.deltaTime;
            return;
        }
        base.Move();
    }
    protected void Bezier(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = Vector3.Lerp(a, b, bezierTimer);
        Vector3 bc = Vector3.Lerp(b, c, bezierTimer);

        Vector3 abc = Vector3.Lerp(ab, bc, bezierTimer);

        Vector3 ab2 = Vector3.Lerp(a, b, bezierTimer + Time.deltaTime);
        Vector3 bc2 = Vector3.Lerp(b, c, bezierTimer + Time.deltaTime);

        Vector3 abc2 = Vector3.Lerp(ab2, bc2, bezierTimer + Time.deltaTime);
        transform.position = abc;
        transform.rotation = Quaternion.LookRotation(abc , abc2);
    }
    protected void Bezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        Vector3 ab = Vector3.Lerp(a, b, bezierTimer);
        Vector3 bc = Vector3.Lerp(b, c, bezierTimer);
        Vector3 cd = Vector3.Lerp(c, b, bezierTimer);

        Vector3 abc = Vector3.Lerp(ab, bc, bezierTimer);
        Vector3 bcd = Vector3.Lerp(bc, cd, bezierTimer);

        Vector3 abcd = Vector3.Lerp(abc, bcd, bezierTimer);

        Vector3 ab2 = Vector3.Lerp(a, b, bezierTimer + Time.deltaTime);
        Vector3 bc2 = Vector3.Lerp(b, c, bezierTimer + Time.deltaTime);
        Vector3 cd2 = Vector3.Lerp(c, b, bezierTimer + Time.deltaTime);

        Vector3 abc2 = Vector3.Lerp(ab2, bc2, bezierTimer + Time.deltaTime);
        Vector3 bcd2 = Vector3.Lerp(bc2, cd2, bezierTimer + Time.deltaTime);

        Vector3 abcd2 = Vector3.Lerp(abc2, bcd2, bezierTimer + Time.deltaTime);
        transform.position = abcd;
        transform.rotation = Quaternion.LookRotation(abcd, abcd2);
    }
}
