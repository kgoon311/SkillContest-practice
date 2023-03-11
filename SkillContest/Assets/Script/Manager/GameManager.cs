using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Player player;
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
    [SerializeField] private Image[] skill = new Image[3];
    [SerializeField] private GameObject boomObject;
    [SerializeField] private ParticleSystem boomEffect;
    [SerializeField] private float[] coolTime = new float[3];
    public List<Enemy> enemys = new List<Enemy>();

    private float[] skillTimer = new float[3];
    private bool[] skillCoolTime = new bool[3];
    private void Awake()
    {
        Instance = this; 
    }
    void Start()
    {
        player = Player.instance;
    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();
        Skill();
    }
    void UIUpdate()
    {
        //ÄðÅ¸ÀÓ
        for (int i = 0; i < 3; i++)
        {

            if (skillTimer[i] > coolTime[i])
            {
                if (skillCoolTime[i] == true)
                    skillCoolTime[i] = false;
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
                     $" : {((f_timer % 60) < 10 ? "0":"")}{(int)(f_timer % 60)}";
        score.text = $"Score : {f_score}";
    }
   
    void Skill()
    {
        if (Input.GetKey(KeyCode.Z) && skillCoolTime[0] == false)
            Targeting();
        if (Input.GetKey(KeyCode.X) && skillCoolTime[1] == false)
            Heal();
        if (Input.GetKey(KeyCode.C) && skillCoolTime[2] == false)
            StartCoroutine(Boom());
    }
    private void Targeting()
    {
        skillTimer[0] = 0;
        skillCoolTime[0] = true;

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
        skillCoolTime[1] = true;
    }
    private IEnumerator Boom()
    {
        skillTimer[2] = 0;
        skillCoolTime[2] = true;

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
