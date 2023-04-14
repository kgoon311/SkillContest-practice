using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : SingletonD<EntityManager>
{
    public bool isStop;
    public bool isSpawnStop;
    public List<Entity> enemys = new List<Entity>();
    [Header("Particle")]
    public GameObject hitParticle;
    public GameObject drawParticle;
    public GameObject deadParticle;
    public GameObject bossDeadParticle;
    public GameObject scoreObject;
    private void Update()
    {
    }
    public void DeadParticle(Vector3 pos)
    {
        Instantiate(deadParticle, transform.position, transform.rotation);
        for (int i = 0; i < 10; i++)
            Instantiate(scoreObject, pos, Quaternion.Euler(0,Random.Range(0f,361f),0));
    }
}
