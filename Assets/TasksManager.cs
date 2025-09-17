using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TasksManager : MonoBehaviour
{
    #region Singleton
    public static TasksManager Instance;
    void Singleton()
    {
        if (Instance != null) { Destroy(Instance); }
        else { Instance = this; }
    }

    void Awake()
    {
        Singleton();
    }
    #endregion

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(DateTime.Now);
    }
}
