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

        if(Player.Instance.targetObject != null)
            targetPos = Player.Instance.targetObject.transform;

        random = new Vector3(Random.Range(minDis, maxDis), Random.Range(minDis, maxDis));
        random = new Vector3(random.x < 0 ? Mathf.Clamp(random.x , minDis, -3) : Mathf.Clamp(random.x, 3, maxDis)
                           , random.z < 0 ? Mathf.Clamp(random.z, minDis, -3) : Mathf.Clamp(random.z, 3, maxDis));
    }
    protected override void Move()
    {
        if (Player.Instance.targetObject != null)
        {
            Debug.Log("?");
            Vector3 center = (beforePos + targetPos.position) / 3 + random;
            Vector3 center2 = (beforePos + targetPos.position) / 3 * 2 + -random;
            Bezier(beforePos, center, center2, targetPos.position);
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
        Debug.Log("abc :" +abc);
        Debug.Log("abc2 :" +abc2);

        transform.LookAt(abc2);
    }
    protected void Bezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        Vector3 ab = Vector3.Lerp(a, b, bezierTimer);
        Vector3 bc = Vector3.Lerp(b, c, bezierTimer);
        Vector3 cd = Vector3.Lerp(c, d, bezierTimer);

        Vector3 abc = Vector3.Lerp(ab, bc, bezierTimer);
        Vector3 bcd = Vector3.Lerp(bc, cd, bezierTimer);

        Vector3 abcd = Vector3.Lerp(abc, bcd, bezierTimer);

        Vector3 ab2 = Vector3.Lerp(a, b, bezierTimer + Time.deltaTime);
        Vector3 bc2 = Vector3.Lerp(b, c, bezierTimer + Time.deltaTime);
        Vector3 cd2 = Vector3.Lerp(c, d, bezierTimer + Time.deltaTime);

        Vector3 abc2 = Vector3.Lerp(ab2, bc2, bezierTimer + Time.deltaTime);
        Vector3 bcd2 = Vector3.Lerp(bc2, cd2, bezierTimer + Time.deltaTime);

        Vector3 abcd2 = Vector3.Lerp(abc2, bcd2, bezierTimer + Time.deltaTime);
        transform.position = abcd;
        transform.LookAt(abcd2);
    }
}
