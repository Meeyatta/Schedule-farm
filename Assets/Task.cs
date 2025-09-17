using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ReminderType { None, OneM, FiveM, FifteenM, Hour };
public enum State { Unassigned, Early, WithinReminder, Active, StillAcceptable, Late }

public class Task : MonoBehaviour
{
    [TextArea] public string Name;
    public DateTime EndTime;
    [TextArea] public string Description;
    public State CurState;

    public class Reminder
    {
        public int Days;
    }
    public ReminderType ReminderWindow;

    [Header("- End time -")]
    public int eHours; public int eMinutes; public int eSeconds;
    [Header("- Current time -")]
    public int cHours; public int cMinutes; public int cSeconds;

    public bool IsAssigned = false;
    Coroutine c;

    void Start()
    {
        Assign("Test", new DateTime(2025, 9, 17, 19, 40, 0), "test test", ReminderType.OneM);
    }

    public void Assign(string name, DateTime time, string desc, ReminderType type) //Starting the task
    {
        Clear();

        Name = name;
        EndTime = time;
        Description = desc;
        ReminderWindow = type;

        IsAssigned = true;

        if (c != null) { StopCoroutine(c); }
        else { c = StartCoroutine(AwaitCompletion()); }          
    }

    IEnumerator AwaitCompletion() //Continuous checks for current task
    {
        yield return new WaitForSeconds(Time.deltaTime);

        while (IsAssigned)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            TimeSpan diff = EndTime - DateTime.Now;
            double window = 0;
            #region Assigning the time window for comparing
            switch (ReminderWindow)
            {
                case ReminderType.None:
                    window = 0;
                    break;
                case ReminderType.OneM:
                    window = 60;
                    break;
                case ReminderType.FiveM:
                    window = 5 * 60;
                    break;
                case ReminderType.FifteenM:
                    window = 15 * 60;
                    break;
                case ReminderType.Hour:
                    window = 60 * 60;
                    break;
                default:
                    Debug.LogError(ReminderWindow);
                    break;
            }

            #endregion

            #region Changing the task's state
            if (diff.TotalSeconds > window) //Before the reminder
            {
                CurState = State.Early;
            }
            else if (diff.TotalSeconds >=0 && diff.TotalSeconds <= window) //Within the reminder
            {
                CurState = State.WithinReminder;
            }
            else if (diff.TotalSeconds < 0 && diff.TotalSeconds >= -60) //Active - within a minute of the set time
            {
                CurState = State.Active;
            }
            else if (diff.TotalSeconds > 60 && diff.TotalSeconds <= -3 * 60) //In case user missed the reminder for a few minutes
            {
                CurState = State.StillAcceptable;
            }
            else //UNACCEPTABLE *LOUD INCORRECT BUZZER* THE USER IS TOO LATE
            {
                CurState = State.Late;
            }
            #endregion


        }
    }

    public void Trigger() //Things what happen when the time is ripe
    {
       
    }

    public void Clear() //Resetting the task
    {
        Name = null;
        Description = null;
        CurState = State.Unassigned;

        IsAssigned = false;
    }

    void Update()
    {
        eHours = EndTime.Hour; eMinutes = EndTime.Minute; eSeconds = EndTime.Second;
        cHours = DateTime.Now.Hour; cMinutes = DateTime.Now.Minute; cSeconds = DateTime.Now.Second;
    }
}
