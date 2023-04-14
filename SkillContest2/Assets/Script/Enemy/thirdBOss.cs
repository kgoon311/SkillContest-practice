using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdBOss : Enemy
{
    public static thirdBOss Instance;
    [SerializeField] private Vector3[] remitePos = new Vector3[2];
    [SerializeField] private GameObject[] shotPos;
    private bool move;
    [Header("Layer")]
    [SerializeField] private LineRenderer laserObject;
    [SerializeField] private GameObject waringObject;
    [Header("Drone")]
    [SerializeField] private GameObject droneGroup;
    [SerializeField] private GameObject[] drone = new GameObject[12];
    [SerializeField] private Vector3[] droneStartPos = new Vector3[6];
    [SerializeField] private Vector3[] droneEndPos = new Vector3[6];
    private int beforeAttack = -1;
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    private void OnEnable()
    {
        droneGroup.SetActive(true);
    }
    protected override void Attack()
    {
        base.Attack();
    }
    protected override IEnumerator AttackPattern()
    {
        int attack = Random.Range(0, 5);

        while (attack == beforeAttack)
            attack = Random.Range(0, 5);

        beforeAttack = attack;
        switch (attack)
        {
            case 0:
                {
                    StartCoroutine(DroneAttack());
                    break;
                }
            case 1:
                {
                    StartCoroutine(DroneAttack2());
                    break;
                }
            case 2:
                {
                    shotPos[2].transform.localRotation = Quaternion.Euler(0f, 0, 0f);
                    int count = Random.Range(1, 4);
                    for (int i = 0; i < count; i++)
                    {
                        DrawWaingLine(shotPos[2].transform.position, shotPos[2].transform.rotation, 1f);
                        yield return new WaitForSeconds(0.5f);
                    }
                    break;
                }
            case 3:
                {
                    for (int i = -1; i < 2; i++)
                    {
                        shotPos[0].transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 30 * i, 0);
                        shotPos[1].transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 30 * i, 0);
                        DrawWaingLine(shotPos[0].transform.position, shotPos[0].transform.rotation, 1f);
                        DrawWaingLine(shotPos[1].transform.position, shotPos[1].transform.rotation, 1f);
                    }
                    break;
                }
            case 4:
                {
                    if (move == true)
                        break;
                    Vector3 beforePos = transform.position;
                    move = true;
                    for(int i = 0; i< Random.Range(2,4);i++)
                    {
                        float timer = 0;
                        while (timer <= 1)
                        {
                            timer += Time.deltaTime * 1.5f;
                            transform.position = Vector3.Lerp(beforePos, remitePos[0], timer);
                            yield return null;
                        }
                        timer = 0;
                        while (timer <= 1)
                        {
                            timer += Time.deltaTime * 3;
                            transform.position = Vector3.Lerp(remitePos[0], remitePos[1], timer);
                            yield return null;
                        }
                        timer = 0;
                        while (timer <= 1)
                        {
                            timer += Time.deltaTime * 1.5f;
                            transform.position = Vector3.Lerp(remitePos[1], beforePos, timer);
                            yield return null;
                        }
                    }
                    transform.position = beforePos;
                    move = false;
                    break;
                }
        }
        attack = Random.Range(0, 6);

        while (attack == beforeAttack)
            attack = Random.Range(0, 6);

        beforeAttack = attack;

        /// <summary>
        /// 
        /// 0 : spriy
        /// 1 : homing
        /// 2 : ufo
        /// 3 : tonado
        /// 4 : warigari
        /// 5 : Áö·Ú
        /// </summary>

        switch (attack)
        {
            case 0:
                {
                    StartCoroutine(Spriy(3f, 0.6f, 60f));
                    break;
                }
            case 1:
                {
                    MultyShot(1, 5, 30);
                    break;
                }
            case 2:
                {
                    NomalShot(2, 3, 30);
                    break;
                }
            case 3:
                {
                    NomalShot(3, 3, 30);
                    break;
                }
            case 4:
                {
                    NomalShot(2, 5, 40);
                    break;
                }
            case 5:
                {
                    NomalShot(4, 1, 0);
                    break;
                }
            case 6:
                {
                    MultyShot(5, 1, 0);
                    break;
                }
        }

        yield return null;
    }
   
    protected IEnumerator Spriy(float time, float shotDelay, float spriyRotate)
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / time * (shotDelay * 100);
            for (int i = 0; i < 2; i++)
            {
                    Transform Pos = shotPos[i].transform;
                    Instantiate(bullet[0], Pos.position,
                        Quaternion.Euler(0, Pos.rotation.eulerAngles.y
                        + Random.Range(-spriyRotate, spriyRotate), 0));

                    Instantiate(bullet[0], Pos.position,
                        Quaternion.Euler(0, Pos.rotation.eulerAngles.y
                        + Random.Range(-spriyRotate, spriyRotate), 0));
            }
            yield return new WaitForSeconds(shotDelay);
        }
        yield return null;
    }
    private void NomalShot(int bulletIdx, int shotCount, float RotateDis)
    {
        for (int i = 0; i < 2; i++)
        {
            Transform Pos = shotPos[i].transform;
            for (int j = -shotCount / 2; j <= shotCount / 2; j++)
            {
                Debug.Log(j);
                Instantiate(bullet[bulletIdx], Pos.position, Quaternion.Euler(0, Pos.rotation.eulerAngles.y + RotateDis * j, 0));
            }
        }
    }
    private void MultyShot(int bulletIdx, int shotCount, float RotateDis)
    {
        for (int i = 0; i < 2; i++)
        {
                Transform pos = shotPos[i].transform;
                for (int k = -shotCount / 2; k < shotCount / 2; k++)
                {
                    Instantiate(bullet[bulletIdx], pos.position, Quaternion.Euler(0, pos.rotation.eulerAngles.y + RotateDis * k, 0));
                }
        }
    }
    protected override void Move()
    {
        transform.LookAt(Player.Instance.transform);
    }
    private IEnumerator DroneAttack()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < 6; i++)
                drone[i].transform.position = Vector3.Lerp(droneStartPos[i], droneEndPos[i], timer);

            yield return null;
        }

        for (int i = 0; i < 6; i++)
            DrawWaingLine(drone[i].transform.position, drone[i].transform.rotation, 1f);

        yield return new WaitForSeconds(2f);

        timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * 3;
            for (int i = 0; i < 6; i++)
                drone[i].transform.position = Vector3.Lerp(droneEndPos[i], droneStartPos[i], timer);

            yield return null;
        }

    }
    private IEnumerator DroneAttack2()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < 6; i++)
            {
                drone[i + 6].transform.position = Vector3.Lerp(droneStartPos[i], droneEndPos[i], timer);
                drone[i + 6].transform.LookAt(Player.Instance.transform);
            }

            yield return null;
        }

        for (int i = 0; i < 6; i++)
        {
            drone[i + 6].transform.LookAt(Player.Instance.transform);
            DrawWaingLine(drone[i + 6].transform.position, drone[i + 6].transform.rotation, 1f);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2f);

        timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * 3;
            for (int i = 0; i < 6; i++)
                drone[i + 6].transform.position = Vector3.Lerp(droneEndPos[i], droneStartPos[i], timer);

            yield return null;
        }
    }
    public void DrawWaingLine(Vector3 pos, Quaternion rotate, float waitTime)
    {
        StartCoroutine(C_DrawWaingLine(pos, rotate, waitTime));
    }
    private IEnumerator C_DrawWaingLine(Vector3 pos, Quaternion rotate, float waitTime)
    {
        GameObject lineObejct = Instantiate(waringObject, pos, rotate);
        yield return new WaitForSeconds(waitTime);
        Destroy(lineObejct);
        DrawLaser(pos, lineObejct.transform.position, 1f);
    }
    public void DrawLaser(Vector3 startPos, Vector3 endPos, float time)
    {
        StartCoroutine(C_DrawLaser(startPos, endPos, time));
    }
    private IEnumerator C_DrawLaser(Vector3 startPos, Vector3 endPos, float time)
    {
        float timer = 0;
        LineRenderer laser = Instantiate(laserObject);
        GameManger.Instance.CameraShake(0.5f, 0.200f);
        laser.SetPosition(0, startPos);
        while (timer < 1)
        {
            timer += Time.deltaTime / time;
            laser.SetWidth(timer, timer);
            laser.SetPosition(1, endPos);
            if (Physics.BoxCast(startPos, Vector3.one / 2, endPos, Quaternion.LookRotation(startPos, endPos), 100, LayerMask.GetMask("Player")))
                Player.Instance._hp -= dmg;

            yield return null;
        }
        while (timer > 0)
        {
            timer -= Time.deltaTime / time * 2;
            laser.SetWidth(timer, timer);

            yield return null;
        }
        Destroy(laser.gameObject);
    }
}
