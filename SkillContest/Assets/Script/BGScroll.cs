using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    [SerializeField] private GameObject[] star = new GameObject[3];
    [SerializeField] private float speed;
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            star[i].transform.position += Vector3.forward * -Time.deltaTime * speed;
            if (star[i].transform.position.z <= -70)
            {
                star[i].transform.position = new Vector3(0, 0, 140);
            }
        }
    }
}
