using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get { if (instance == null) { new GameObject(typeof(T).Name).AddComponent<T>(); 
              
            }
            return instance;
        }
    }
    protected void Awake()
    {
        if (instance==null)
        {
            instance = this as T;
            DontDestroyOnLoad(instance);
            OnSingletonAwake();
        }
        else if(instance!=this)
            Destroy(gameObject);
    }
    protected abstract void OnSingletonAwake();
}
