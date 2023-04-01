using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGameManager : SingletonD<InGameManager>
{
    [Header("Boss")]
    [SerializeField] private GameObject[] bossObject;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private GameObject WaringPanel;
    [SerializeField] private GameObject[] waringText;
    [SerializeField] private float returnPos;
    [SerializeField] private float returnSize;
    [SerializeField] private int bossIdx;

    public void SpawnBoss()
    {
        StartCoroutine(BossSpawn());
    }

    private IEnumerator BossSpawn()
    {
        float timer = 0;
        StartCoroutine(WaringText());
        GameManger.Instance.CameraShake(1, 1f);
        EntityManager.Instance.isSpawnStop = true;
        EntityManager.Instance.isStop = true;
        yield return new WaitForSeconds(2f);
        while (timer < 1)
        {
            timer += Time.deltaTime;
            bossObject[bossIdx].transform.position = Vector3.Lerp(startPos, endPos, timer);
        }
        EntityManager.Instance.isStop = false;
    }
    public void BossDead()
    {
        StartCoroutine(C_BossDead());
    }
    private IEnumerator C_BossDead()
    {
        float timer = 0;
        EntityManager.Instance.isStop = true;
        GameManger.Instance.CameraShake(2, 1f);
        while (timer < 1)
        {
            timer += Time.deltaTime/2;
            bossObject[bossIdx].transform.position = Vector3.Lerp(endPos, startPos, timer);
        }
        bossIdx++;
        yield return null;
    }
    private IEnumerator WaringText()
    {
        WaringPanel.SetActive(true);

        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / 2;
            for (int i = 0; i < 2; i++)
            {
                waringText[i].transform.position += Vector3.left * Time.deltaTime;
                if (waringText[i].transform.localPosition.x <= returnPos)
                    waringText[i].transform.position += Vector3.right * returnSize;

                yield return null;
            }
        }

        WaringPanel.SetActive(false);
        yield return null;
    }
}
