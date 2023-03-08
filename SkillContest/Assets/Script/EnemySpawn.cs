using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> Enemys = new List<GameObject>();
    [SerializeField] private List<Transform> spawnPoint = new List<Transform>();
    [SerializeField] private float[] spawnTime = new float[2];
    [SerializeField] private int spawnIdx;
    private float timer;
    void Start()
    {
        StartCoroutine(SpawnTimer());
    }
    private IEnumerator SpawnTimer()
    {
        timer = Random.Range(spawnTime[0], spawnTime[1]);
       
        yield return new WaitForSeconds(timer);
        
        StartCoroutine(SpawnPattern());
        StartCoroutine(SpawnTimer());
    }
    private IEnumerator SpawnPattern()
    {
        if (EntityMnager.instance.isSpawnStop)
            yield break;

        switch (spawnIdx)
        { 
            case 0:
                {
                    for(int i = 0; i < 7; i += 2)
                    {
                        Instantiate(Enemys[0], spawnPoint[i].position, spawnPoint[i].rotation);
                    }
                    break;
                }
            case 1:
                {
                    for (int i = 0; i < 7; i += 2)
                    {
                        Instantiate(Enemys[0], spawnPoint[i].position, spawnPoint[i].rotation);
                    }
                    break;
                }
        }

        yield return null;
    }
}
