using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    public static T Instance 
    {
        get
        {
            if (!instance) instance = FindObjectOfType<T>();
            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }


}
