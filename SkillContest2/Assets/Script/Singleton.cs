using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    StreamWriter sw = new StreamWriter("Assets/Resorces/Text");
    StreamReader sr = new StreamReader("Assets/Resorces/Text");
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return instance;
        }
    }

    void Start()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);

        string[] list = new string[5];
        for (int i = 0; i < 5; i++)
        {
            list[i] = sr.ReadLine();
            if (list[i] == null) list[i] = "null , 1000";
        }
        for(int i = 0;i<5;i++)
        {
            if (int.Parse(list[i].Split(",")[1]) < 1000)
            {
                for(int j = 4; j > i; j++)
                {
                    list[j] = list[j - 1];
                }
                list[i] = $"name , 1000";
                break;
            }
        }
        for(int i = 0; i < 5;i++)
        {
            sw.WriteLine(list[i]);
        }
    }

}
public class SingletonD<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return instance;
        }
    }

    void Start()
    {
        if (instance != null)
            Destroy(gameObject);

       
    }

}
