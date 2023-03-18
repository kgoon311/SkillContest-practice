using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
public class UFO : Enemy
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float shootCount;
    public int shotPattern;

    protected override void Move()
    {
        transform.position += Vector3.forward * -speed * Time.deltaTime;
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    protected override IEnumerator AttackPattern()
    {
        switch (shotPattern)
        { 
            case 0:
                for (int i = 0; i <shootCount; i++)
                {
                    Instantiate(bullet,  transform.position,  Quaternion.Euler(0, transform.rotation.eulerAngles.y + (360 /shootCount) * i, 0));
                }
                break;
            case 1:
                for (int i = 0; i < shootCount; i++)
                {
                    Instantiate(bullet, transform.position, Quaternion.Euler(0, Random.Range(0f,361f), 0));
                }
                break;
            
        }

        
        yield return null;
    }
}
