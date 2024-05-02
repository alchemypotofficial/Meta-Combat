using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Game<T> : Singleton<T> where T : Game<T>
{
    public abstract void PreInit();
    public abstract void Init();
    public abstract void PostInit();

    private void Start()
    {
        PreInit();
        Init();
        PostInit();
    }
}
