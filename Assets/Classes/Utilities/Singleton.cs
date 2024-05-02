using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            Debug.LogWarning("Warning: An instance of this singleton already exists.");
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}