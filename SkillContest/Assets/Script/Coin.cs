using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject modle;
    [SerializeField] private Rigidbody rb;
    public float score;

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(speed);
    }
}
