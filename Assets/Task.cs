using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public enum ReminderType { None, OneM, FiveM, FifteenM, Hour };
public enum State { Unassigned, Early, WithinReminder, Active, StillAcceptable, Late }

public class Task : MonoBehaviour
{
    [TextArea] public string Name;
    public DateTime EndTime;
    [TextArea] public string Description;
    public State CurState;

    bool bWithinReminder_fired = false; 
    public UnityEvent<Task> eWithinReminder = new UnityEvent<Task>();

    bool bActive_fired = false; 
    public UnityEvent<Task> eActive = new UnityEvent<Task>();

    bool bStillAcceptabler_fired = false; 
    public UnityEvent<Task> eStillAcceptable = new UnityEvent<Task>();

    bool bLate_fired = false; 
    public UnityEvent<Task> eLate = new UnityEvent<Task>();

    [Header("---")]
    public bool IsOpened;

    public class Reminder
    {
        public int Days;
    }
    public ReminderType ReminderWindow;

    [Header("- End time -")]
    public int eHours; public int eMinutes; public int eSeconds;

    public bool IsAssigned = false;
    Coroutine c;

    void Start()
    {
        Assign("Test", new DateTime(2025, 9, 17, 19, 40, 0), "test test", ReminderType.OneM);
    }

    public void bCreation_start()
    {
        if (!IsOpened)
        {
            TaskMenu.Instance.Menu_enable(this);
            //IsOpened = true;
        }
        
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
                if (!bWithinReminder_fired) { eWithinReminder.Invoke(this); bWithinReminder_fired = true; }
                CurState = State.WithinReminder;
            }
            else if (diff.TotalSeconds < 0 && diff.TotalSeconds >= -60) //Active - within a minute of the set time
            {
                if (!bActive_fired) { eActive.Invoke(this); bActive_fired = true; }
                CurState = State.Active;
            }
            else if (diff.TotalSeconds > 60 && diff.TotalSeconds <= -3 * 60) //In case user missed the reminder for a few minutes
            {
                if (!bStillAcceptabler_fired) { eStillAcceptable.Invoke(this); bStillAcceptabler_fired = true; }
                CurState = State.StillAcceptable;
            }
            else //UNACCEPTABLE *LOUD INCORRECT BUZZER* THE USER IS TOO LATE
            {
                if (!bLate_fired) { eLate.Invoke(this); bLate_fired = true; }
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
    }
}
