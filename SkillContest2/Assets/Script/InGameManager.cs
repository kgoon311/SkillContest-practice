using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms;

public class InGameManager : SingletonD<InGameManager>
{
    private Vector3 CamPos;
    private Quaternion CamRotate;
    private float f_Score;
    private float f_Time;
    private float stageTimer;

    private int qusetCount;
    [HideInInspector] public int killCount;
    [HideInInspector] public int TargetingCount;
    [HideInInspector] public int ItemCount;
    [SerializeField] private int bossIdx;
    [Header("Boss")]
    [SerializeField] private GameObject[] bossObject;
    [SerializeField] private GameObject lastBossModel;
    [SerializeField] private GameObject lastBossFleetingObject;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private Vector3 bossCameraPos;
    [SerializeField] private Quaternion bossCameraRotate;
    [Header("UI")]
    [SerializeField] private Image[] hpbar;
    [SerializeField] private Slider map;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI[] QusetText;
    [SerializeField] private Image[] skillImage;

    [Space(1f)]
    [SerializeField] private GameObject[] uiPanel;
    [SerializeField] private Vector3[] lastUiPanelPos;

    [Space(1f)]
    [SerializeField] private GameObject lastPlayerGroup;
    [SerializeField] private Image[] lastPlayerUI;
    [SerializeField] private Vector3 lastPlayerGroupPos;

    [Space(1f)]
    [SerializeField] private GameObject BossHpbarObject;
    [SerializeField] private Image BossHpbar;
    [SerializeField] private Vector3[] BossHpbarPos;
    [SerializeField] private float lastBossHpbarSize;

    [Space(1f)]
    [SerializeField] private GameObject WaringPanel;
    [SerializeField] private GameObject[] waringText;
    [SerializeField] private float waringReturnPos;
    [SerializeField] private float waringReturnSize;
    [Header("FadeInOut")]
    [SerializeField] private Image fadePanel;
    private void Start()
    {
        qusetCount = Random.Range(0, 3);
        CamPos = Camera.main.transform.position;
        CamRotate = Camera.main.transform.rotation;
        SpawnBoss();
    }


    private void Update()
    {
        if (EntityManager.Instance.isStop)
            UiUpdatet();
    }
    private void UiUpdatet()
    {
        stageTimer += Time.deltaTime;
        f_Time += Time.deltaTime;

        hpbar[0].fillAmount = Player.Instance._hp / Player.Instance.maxHp;
        hpbar[1].fillAmount = Player.Instance._oil / Player.Instance.maxOil;

        score.text = $"{(int)f_Score}";
        timer.text = $"{(int)f_Time}";

        map.value = stageTimer / 60;
        switch(qusetCount)
        {
            case 0:
                QusetText[0].text = $"Kill The Monster";
                QusetText[1].text = $"{killCount} / 100";
                break;
            case 1:
                QusetText[0].text = $"Use The Items";
                QusetText[1].text = $"{ItemCount} / 10";
                break;
            case 2:
                QusetText[0].text = $"Targeting Monster";
                QusetText[1].text = $"{TargetingCount} / 100";
                break;
        }
    }
    public void SpawnBoss()
    {
        StartCoroutine(BossSpawn());
    }

    private IEnumerator BossSpawn()
    {
        float timer = 0;
        StartCoroutine(WaringText());
        GameManger.Instance.CameraShake(3, 1f);
        EntityManager.Instance.isSpawnStop = true;
        EntityManager.Instance.isStop = true;
        bossObject[bossIdx].SetActive(true);

        yield return new WaitForSeconds(3.5f);
        while (timer < 1)
        {
            timer += Time.deltaTime * 3;
            Camera.main.transform.position = Vector3.Lerp(CamPos, bossCameraPos, timer);
            Camera.main.transform.rotation = Quaternion.Lerp(CamRotate, bossCameraRotate, timer);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        if (bossIdx == 2)
        {
            GameManger.Instance.CameraShake(1f, 2f);
            StartCoroutine(FadeIn(1, Color.white));
            yield return new WaitForSeconds(1f);
            lastBossFleetingObject.SetActive(false);
            lastBossModel.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FadeOut(1, Color.white));
        }
        yield return new WaitForSeconds(5f);

        timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(bossCameraPos, CamPos, timer);
            Camera.main.transform.rotation = Quaternion.Lerp(bossCameraRotate, CamRotate, timer);
            yield return null;
        }
        timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            BossHpbarObject.transform.localPosition = Vector3.Lerp(BossHpbarPos[0], BossHpbarPos[1], timer);
            if (bossIdx == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    uiPanel[i].transform.localPosition = Vector3.Lerp(uiPanel[i].transform.localPosition, lastUiPanelPos[i], timer);
                }
                lastPlayerGroup.transform.localPosition = Vector3.Lerp(lastPlayerGroup.transform.localPosition, lastPlayerGroupPos, timer);
                Vector3 size = BossHpbar.transform.localScale;
                BossHpbar.transform.localScale = Vector3.Lerp(size, new Vector3(size.x, lastBossHpbarSize), timer);
            }
            yield return null;
        }
        EntityManager.Instance.isStop = false;
    }
    public IEnumerator FadeIn(float time, Color color)
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / time;
            fadePanel.color = new Color(color.r, color.g, color.b, timer);
            yield return null;
        }
        fadePanel.color = new Color(color.r, color.g, color.b, 1);
    }
    public IEnumerator FadeOut(float time, Color color)
    {
        float timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime / time;
            fadePanel.color = new Color(color.r, color.g, color.b, timer);
            yield return null;
        }
        fadePanel.color = new Color(color.r, color.g, color.b, 0);
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
            timer += Time.deltaTime / 2;
            bossObject[bossIdx].transform.position = Vector3.Lerp(endPos, startPos, timer);
        }
        bossObject[bossIdx].SetActive(false);
        bossIdx++;
        yield return null;
    }
    public void NextStage()
    {
        stageTimer = 0;

    }
    private IEnumerator WaringText()
    {
        WaringPanel.SetActive(true);

        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / 3f;
            for (int i = 0; i < 2; i++)
            {
                waringText[i].transform.localPosition += Vector3.left * 30;
                if (waringText[i].transform.localPosition.x < waringReturnPos)
                    waringText[i].transform.localPosition += Vector3.right * waringReturnSize * 2;
            }
            yield return null;
        }

        WaringPanel.SetActive(false);
        yield return null;
    }
    public void AddScore(float score)
    {
        f_Score += score;
    }
}
