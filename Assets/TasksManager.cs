using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TasksManager : MonoBehaviour
{
    public List<Task> Tasks;
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

    [Header("- Current time -")]
    public int cHours; public int cMinutes; public int cSeconds;

    void Update()
    {
        cHours = DateTime.Now.Hour; cMinutes = DateTime.Now.Minute; cSeconds = DateTime.Now.Second;
    }
}
