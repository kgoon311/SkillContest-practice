using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMnager : MonoBehaviour
{
    public static EntityMnager instance;

    public Material hitMaterial;
    public bool isStop;

    void Awake()
    {
        instance = this;
    }
}
