using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMnager : MonoBehaviour
{
    public static EntityMnager instance;

    public Material hitMaterial;
    public bool isStop;
    public bool isSpawnStop;

    void Awake()
    {
        instance = this;
    }
}
