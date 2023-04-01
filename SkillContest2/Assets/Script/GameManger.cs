using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManger : Singleton<GameManger>
{
    public float score;
    private bool cameraShaking;
    private void Awake()
    {
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
        if(cameraShaking == true)
            StartCoroutine(C_CameraShake(time,power));
    }
    private IEnumerator C_CameraShake(float time, float power)
    {
        cameraShaking = true;
        Vector3 firstPos = Camera.main.transform.position;

        float timer = 0;
        while(timer < time)
        {
            Camera.main.transform.position = new Vector3(firstPos.x + Random.Range(0f,power),0, firstPos.z + Random.Range(0f, power));
            yield return null;
        }

        cameraShaking = false;
        Camera.main.transform.position = firstPos;
        yield return null;
    }
}
