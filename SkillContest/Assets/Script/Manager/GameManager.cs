using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Player player;
    public List<Enemy> enemys = new List<Enemy>();

    [Header("Boss")]
    [SerializeField] private GameObject[] bossObject = new GameObject[3];
    private Entity[] bossScript = new Entity[3];
    private Animator[] bossAnimator = new Animator[3];

    [SerializeField] private int bossIdx;
    private bool bossActive;
    [Header("BossUI")]
    [SerializeField] private GameObject warningGroup;
    [SerializeField] private GameObject[] warningText = new GameObject[2];

    [Space(10)]
    [SerializeField] private Slider bossHpbar;
    [SerializeField] private Slider lastPlayerHpBar;
    [SerializeField] private Slider lastPlayerOilBar;
    [SerializeField] private Vector3 lastHpPos;
    [SerializeField] private RectTransform lastHpGroup;

    [Space(10)]
    [SerializeField] private Vector3 lastSkillPos;
    [SerializeField] private Vector3[] lastUiPos = new Vector3[2];
    [SerializeField] private RectTransform[] lastUiObjects = new RectTransform[2];
    [Header("UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider oilSlider;
    [SerializeField] private Slider map;

    [SerializeField] private Text score;
    [SerializeField] private Text timer;

    private float f_score;
    private float f_timer;
    private float bossTimer;
    private float bossTime = 90;

    [Header("Skill")]
    [SerializeField] private RectTransform skillGroup;
    [SerializeField] private Image[] skill = new Image[3];

    [SerializeField] private GameObject boomObject;
    [SerializeField] private ParticleSystem boomEffect;

    [SerializeField] private float[] coolTime = new float[3];
    private float[] skillTimer = new float[3];
    private bool[] skillActive = new bool[3];


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
        player = Player.instance;
        BossSetting();
        BossSpawn();
    }
    // Update is called once per frame
    void Update()
    {
        UIUpdate();
        Skill();
    }
    void UIUpdate()
    {

        if (bossIdx == 2 && bossActive)
        {
            lastPlayerHpBar.value = player._hp / player._maxHp;
            lastPlayerOilBar.value = player._oil / player._maxOil;
            return;
        }

        if (bossActive == true)
        {
            bossHpbar.value = bossScript[bossIdx]._hp / bossScript[bossIdx]._maxHp;
        }
        else if ((bossTimer / bossTime > 1))
            BossSpawn();

        //ÄðÅ¸ÀÓ
        for (int i = 0; i < 3; i++)
        {

            if (skillTimer[i] > coolTime[i])
            {
                if (skillActive[i] == true)
                    skillActive[i] = false;
                continue;
            }

            skillTimer[i] += Time.deltaTime;
            skill[i].fillAmount = skillTimer[i] / coolTime[i];
        }
        bossTimer += Time.deltaTime;
        f_timer += Time.deltaTime;

        //HP,Oil
        hpSlider.value = player._hp / player._maxHp;
        oilSlider.value = player._oil / player._maxOil;
        map.value = bossTimer / bossTime;

        timer.text = $"{((f_timer / 60) < 10 ? "0" : "")}{(int)(f_timer / 60)}" +
                     $" : {((f_timer % 60) < 10 ? "0" : "")}{(int)(f_timer % 60)}";
        score.text = $"Score : {f_score}";

    }

    #region skill
    void Skill()
    {
        if (Input.GetKey(KeyCode.Z) && skillActive[0] == false)
            Targeting();
        if (Input.GetKey(KeyCode.X) && skillActive[1] == false)
            Heal();
        if (Input.GetKey(KeyCode.C) && skillActive[2] == false)
            StartCoroutine(Boom());
    }
    private void Targeting()
    {
        skillTimer[0] = 0;
        skillActive[0] = true;

        RaycastHit target;
        if (Physics.BoxCast(player.transform.position, Vector3.one, Vector3.forward, out target, player.transform.rotation, 100, LayerMask.GetMask("Enemy")))
        {
            player.targetObject = target.transform.gameObject;
        }
    }
    private void Heal()
    {
        skillTimer[1] = 0;
        player._hp += 5;
        skillActive[1] = true;
    }
    private IEnumerator Boom()
    {
        skillTimer[2] = 0;
        skillActive[2] = true;

        boomObject.SetActive(true);
        boomObject.transform.position = player.transform.position;

        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            boomObject.transform.position += Vector3.forward * Time.deltaTime * 10;
            yield return null;
        }

        boomObject.SetActive(false);
        boomEffect.transform.position = boomObject.transform.position;
        boomEffect.Play();
        CameraShake(0.5f, 2f);

        foreach (Enemy enemy in enemys)
        {
            enemy._hp -= 50;
        }
        yield return null;
    }
    #endregion
    void BossSetting()
    {
        for (int i = 0; i < 3; i++)
        {
            bossScript[i] = bossObject[i].GetComponent<Entity>();
            bossAnimator[i] = bossObject[i].GetComponent<Animator>();
        }
    }
    void BossSpawn()
    {
        bossActive = true;
        StartCoroutine(C_BossSpawn());
    }
    private IEnumerator C_BossSpawn()
    {
        bossObject[bossIdx].SetActive(true);

        EntityMnager.instance.isStop = true;
        EntityMnager.instance.isSpawnStop = true;
        StartCoroutine(ActiveWarningText(4f)) ;
        CameraShake(4, 1);

        yield return new WaitForSeconds(4f);

        Vector3 beforeCam = Camera.main.transform.position;
        StartCoroutine(CameraMove(bossObject[bossIdx].transform.position + Vector3.up * 20, 1));

        yield return new WaitForSeconds(1);

        // bossAnimator[bossIdx].SetBool("Appear", true);

        yield return new WaitForSeconds(2);

        StartCoroutine(CameraMove(beforeCam, 1));

        yield return new WaitForSeconds(1);

        bossHpbar.gameObject.SetActive(true);
        float timer = 0;

        while (timer < 1)
        {
            timer += Time.deltaTime;
            bossHpbar.transform.localPosition = Vector3.Lerp(bossHpbar.transform.localPosition, new Vector3(0,70,0) , timer);
            yield return null; 
        }

        if (bossIdx == 2)
        {
            Debug.Log("?");
            timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime / 2;
                for (int i = 0; i < 2; i++)
                {
                    lastUiObjects[i].anchoredPosition = Vector3.Lerp(lastUiObjects[i].anchoredPosition, lastUiPos[i], timer);
                    bossHpbar.transform.localScale = Vector3.Lerp(bossHpbar.transform.localScale, new Vector3(1.5f, 1, 1), timer);
                }
                yield return null;
            }

            timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime / 2;
                skillGroup.anchoredPosition  = Vector3.Lerp(skillGroup.anchoredPosition, lastSkillPos, timer);
                lastHpGroup.anchoredPosition = Vector3.Lerp(lastHpGroup.anchoredPosition, lastHpPos, timer);

                yield return null;
            }
        }
        EntityMnager.instance.isStop = false;
        EntityMnager.instance.isSpawnStop = false;
        yield return null;
    }
    private IEnumerator ActiveWarningText(float time)
    {
        float timer = time;
        warningGroup.SetActive(true);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            for (int i = 0; i < 2; i++)
            {
                warningText[i].transform.localPosition += Vector3.left * Time.deltaTime * 300;
                if (warningText[i].transform.localPosition.x < -300)
                    warningText[i].transform.localPosition += Vector3.right * 600;
            }
            yield return null;
        }
        warningGroup.SetActive(false);
    }
    private IEnumerator CameraMove(Vector3 pos, float time)
    {
        float whileTimer = 0;
        while (whileTimer < time)
        {
            whileTimer += Time.deltaTime;

            Transform camPos = Camera.main.transform;
            camPos.position = Vector3.Lerp(camPos.position, pos, whileTimer);

            yield return null;
        }
    }
    public void CameraShake(float shakeTime, float shakePower)
    {
        StartCoroutine(C_CameraShake(shakeTime, shakePower));
    }
    private IEnumerator C_CameraShake(float shakeTime, float shakePower)
    {
        Vector3 beforePos = Camera.main.transform.position;

        float timer = 0;
        while (timer < shakeTime)
        {
            timer += Time.deltaTime;
            Camera.main.transform.position = new Vector3(beforePos.x + Random.Range(0.5f, shakePower), beforePos.y,
                                                         beforePos.z + Random.Range(0.5f, shakePower));
            yield return null;
        }

        Camera.main.transform.position = beforePos;
        yield return null;
    }
}
