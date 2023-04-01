using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManger : Singleton<GameManger>
{
    public float score;

    [Header("camera")]
    private Coroutine cameraShakeCorotine;
    private bool cameraShaking;
    private float wayShakeingPower;
    private Vector3 firstPos;
    private void Awake()
    {

    }
    private void OnEnable()
    {
        firstPos = Camera.main.transform.position;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CameraShake(float time , float power)
    {
        if(cameraShaking == false)
            cameraShakeCorotine = StartCoroutine(C_CameraShake(time, power));
        else if (wayShakeingPower < power)
        {
            StopCoroutine(cameraShakeCorotine);
            Camera.main.transform.position = firstPos;
            cameraShakeCorotine = StartCoroutine(C_CameraShake(time, power));
        }
    }
    private IEnumerator C_CameraShake(float time, float power)
    {
        cameraShaking = true;
        firstPos = Camera.main.transform.position;

        float timer = 0;
        while(timer < time)
        {
            timer += Time.deltaTime;
            Camera.main.transform.position = new Vector3(firstPos.x + Random.Range(0f,power), firstPos.y, firstPos.z + Random.Range(0f, power));
            yield return null;
        }

        cameraShaking = false;
        Camera.main.transform.position = firstPos;
        yield return null;
    }
}
