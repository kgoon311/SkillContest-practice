using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class EntityMnager : MonoBehaviour
{
    public static EntityMnager instance;

    public Material hitMaterial;
    public bool isStop;
    public bool isSpawnStop;

   
    [SerializeField] private List<GameObject> Enemys = new List<GameObject>();
    [SerializeField] private List<Transform> spawnPoint = new List<Transform>();
    [SerializeField] private float[] spawnTime = new float[2];
    [SerializeField] private int[] spawnIdx;
    private int spawnCount = 0;
    private float timer;
    private int beforeCount;
    void Awake()
    {
        instance = this;
    }
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
        if (isSpawnStop == true)
            yield break;

        spawnCount = beforeCount;
        while(spawnCount == beforeCount)
            spawnCount = Random.Range(0, spawnIdx[GameManager.Instance.bossIdx]);

        beforeCount = spawnCount;
        /// <summary>
        /// Enemys 
        /// ------ 
        /// 0 : eye
        /// 1 : ufo
        /// 2 : warm
        /// 3 : boomber
        /// 4 : warigari
        /// </summary>
        switch (spawnCount)
        {
            case 0:
                {
                    for (int i = 0; i < 7; i += 2)
                    {
                        Instantiate(Enemys[0], spawnPoint[i].position, spawnPoint[i].rotation);
                    }
                    break;
                }
            case 1:
                {
                    for (int i = 1; i < 6; i += 2)
                    {
                        Instantiate(Enemys[1], spawnPoint[i].position, spawnPoint[i].rotation);
                    }
                    break;
                }
            case 2:
                {
                    for (int i = 0; i < 7; i += 2)
                    {
                        Instantiate(Enemys[1], spawnPoint[i].position, spawnPoint[i].rotation);
                    }
                    break;
                }
            case 3:
                {
                    Instantiate(Enemys[1], spawnPoint[0].position, spawnPoint[0].rotation);
                    Instantiate(Enemys[1], spawnPoint[6].position, spawnPoint[6].rotation);
                    Instantiate(Enemys[0], spawnPoint[2].position, spawnPoint[2].rotation);
                    Instantiate(Enemys[0], spawnPoint[4].position, spawnPoint[4].rotation);
                    break;
                }
            case 4:
                {
                    Instantiate(Enemys[2], spawnPoint[2].position, spawnPoint[2].rotation);
                    Instantiate(Enemys[2], spawnPoint[4].position, spawnPoint[4].rotation);
                    break;
                }
            case 5:
                {
                    Instantiate(Enemys[1], spawnPoint[0].position, spawnPoint[0].rotation);
                    Instantiate(Enemys[1], spawnPoint[6].position, spawnPoint[6].rotation);
                    Instantiate(Enemys[0], spawnPoint[2].position, spawnPoint[2].rotation);
                    Instantiate(Enemys[0], spawnPoint[4].position, spawnPoint[4].rotation);
                    Instantiate(Enemys[0], spawnPoint[9].position, spawnPoint[9].rotation);
                    Instantiate(Enemys[0], spawnPoint[11].position, spawnPoint[11].rotation);
                    break;
                }
            case 6:
                {
                    Instantiate(Enemys[1], spawnPoint[11].position, spawnPoint[11].rotation);
                    Instantiate(Enemys[1], spawnPoint[12].position, spawnPoint[12].rotation);
                    break;
                }
            case 7:
                {
                    Instantiate(Enemys[1], spawnPoint[0].position, spawnPoint[0].rotation);
                    Instantiate(Enemys[0], spawnPoint[2].position, spawnPoint[2].rotation);
                    Instantiate(Enemys[1], spawnPoint[3].position, spawnPoint[3].rotation);
                    Instantiate(Enemys[0], spawnPoint[4].position, spawnPoint[4].rotation);
                    Instantiate(Enemys[1], spawnPoint[6].position, spawnPoint[6].rotation);
                    break;
                }
            //2스테이지 
            case 8:
                {
                    Instantiate(Enemys[2], spawnPoint[11].position, spawnPoint[11].rotation);
                    yield return new WaitForSeconds(1f);
                    Instantiate(Enemys[2], spawnPoint[12].position, spawnPoint[12].rotation);
                    yield return new WaitForSeconds(1f);
                    Instantiate(Enemys[2], spawnPoint[10].position, spawnPoint[10].rotation);
                    Instantiate(Enemys[2], spawnPoint[8].position, spawnPoint[8].rotation);
                    break;
                }
            case 9:
                {
                    Instantiate(Enemys[0], spawnPoint[11].position, spawnPoint[11].rotation);
                    Instantiate(Enemys[0], spawnPoint[12].position, spawnPoint[12].rotation);
                    yield return new WaitForSeconds(1f);
                    Instantiate(Enemys[2], spawnPoint[10].position, spawnPoint[10].rotation);
                    Instantiate(Enemys[2], spawnPoint[8].position, spawnPoint[8].rotation);
                    Instantiate(Enemys[0], spawnPoint[1].position, spawnPoint[1].rotation);
                    Instantiate(Enemys[0], spawnPoint[5].position, spawnPoint[5].rotation);
                    break;
                }
            //3스테이지 
        }

        yield return null;
    }
  
}
