using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class UFO : Enemy
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float shootCount;
    protected override void Move()
    {
        transform.position += Vector3.forward * -speed * Time.deltaTime;
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    protected override IEnumerator AttackPattern()
    {
        for(int i = 0; i< shootCount;i++)
        {
            Instantiate(bullet , transform.position , Quaternion.Euler(0,transform.rotation.eulerAngles.y + (360/shootCount) * i,0));
        }
        yield return null;
    }
}
