using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Player player;
    public List<Enemy> enemys = new List<Enemy>();

    [Header("Boss")]
    [SerializeField] private GameObject[] bossObject = new GameObject[3];
    [SerializeField] private Animation[] bossAnim = new Animation[3];
    private Entity[]    bossScript = new Entity[3];
    private Animator[]  bossAnimator = new Animator[3];

    [SerializeField] private Vector3[] bossPos = new Vector3[3];

    [SerializeField] private int bossIdx;
    private bool bossActive;
    [Header("BossUI")]
    [SerializeField] private Slider bossHpbar;
    [SerializeField] private Slider lastBossHpbar;
    [SerializeField] private Slider lastPlayerHpBar;
    [SerializeField] private Slider lastPlayerOilBar;

    [SerializeField] private Vector2 lastSkillPos;

    [SerializeField] private Vector2[] lastUiPos = new Vector2[2]; 
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
    [SerializeField] private GameObject skillGroup;
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
            lastBossHpbar.value = bossScript[bossIdx]._hp / bossScript[bossIdx]._maxHp;

            lastPlayerHpBar.value  = player._hp / player._maxHp;
            lastPlayerOilBar.value = player._oil / player._maxOil;
            return;
        }

        if(bossActive == true)
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
        hpSlider.value  = player._hp / player._maxHp;
        oilSlider.value = player._oil / player._maxOil;
        map.value = bossTimer / bossTime;
        
        timer.text = $"{((f_timer / 60) < 10 ? "0" : "")}{(int)(f_timer / 60)}" +
                     $" : {((f_timer % 60) < 10 ? "0":"")}{(int)(f_timer % 60)}";
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
        while(timer < 1)
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
        for(int i = 0; i < 3;i++)
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
        float whileTimer = 0;
        bossObject[bossIdx].SetActive(true);

        EntityMnager.instance.isStop = true;
        EntityMnager.instance.isSpawnStop = true;

        CameraShake(1, 3);
        yield return new WaitForSeconds(1.1f);
        Vector3 beforCam = Camera.main.transform.position; 

        while (whileTimer < 1)
        {
            whileTimer += Time.deltaTime * 3;

            Transform camPos = Camera.main.transform;
            camPos.position = Vector3.Lerp(camPos.position, bossObject[bossIdx].transform.position, whileTimer);
            yield return null; 
        }

        bossAnimator[bossIdx].SetBool("Appear", true);
        yield return new WaitForSeconds(2);

        whileTimer = 0;
        while (whileTimer < 1)
        {
            whileTimer += Time.deltaTime * 3;

            Transform camPos = Camera.main.transform;
            camPos.position = Vector3.Lerp(camPos.position, beforCam, whileTimer);

            yield return null;
        }

        EntityMnager.instance.isStop = false;
        EntityMnager.instance.isSpawnStop = false;
        yield return null;
    }

    public void CameraShake(float shakeTime, float shakePower)
    {
        StartCoroutine(C_CameraShake(shakeTime, shakePower));
    }
    private IEnumerator C_CameraShake(float shakeTime , float shakePower)
    {
        Vector3 beforePos = Camera.main.transform.position;

        float timer = 0;
        while(timer < shakeTime)
        {
            timer += Time.deltaTime;
            Camera.main.transform.position = new Vector3(beforePos.x + Random.Range(0.5f, shakePower),beforePos.y, 
                                                         beforePos.z + Random.Range(0.5f, shakePower));
            yield return null;
        }

        Camera.main.transform.position = beforePos;
        yield return null;  
    }
}
