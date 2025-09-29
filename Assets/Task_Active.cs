using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Task_Active : MonoBehaviour
{
    public Color StartColor;

    public Color Reminder_color;
    public Color Active_color;
    public Color Late_color;

    [Header("---")]
    public TMPro.TMP_Text tName;
    public TMPro.TMP_Text tTime;
    public RawImage Background;
    Task Task_;

    public void Do_Assigned(Task t)
    {
        tName.gameObject.SetActive(true);
        tTime.gameObject.SetActive(true);
        Background.gameObject.SetActive(true);

        tName.text = t.Name;
    }
    public void Do_WithinReminder(Task t)
    {
        Background.color = Reminder_color;
    }
    public void Do_WithinActive(Task k)
    {
        Background.color = Active_color;
    }
    public void Do_WithinAcceptable(Task t)
    {
        Background.color = Late_color;
    }
    public void Do_WithinLate(Task t)
    {
        Background.color = StartColor;

        tName.gameObject.SetActive(false);
        tTime.gameObject.SetActive(false);
        Background.gameObject.SetActive(false);
    }

    void Start()
    {
        Task_ = transform.parent.GetComponent<Task>();

        Background.color = StartColor;

        Task_.eAssigned.AddListener(Do_Assigned);
        Task_.eWithinReminder.AddListener(Do_WithinReminder);
        Task_.eActive.AddListener(Do_WithinActive);
        Task_.eStillAcceptable.AddListener(Do_WithinAcceptable);
        Task_.eLate.AddListener(Do_WithinLate);
    }

    void Update()
    {
        TimeSpan difference = Task_.EndTime - DateTime.Now;

        string d = "";
        if (difference.Days > 0)
        {
            d = difference.Days.ToString() + "d ";
        }
        string h = "";
        if (difference.Hours > 0)
        {
            h = difference.Hours.ToString() + "h ";
        }
        string m = "";
        if (difference.Minutes > 0)
        {
            m = difference.Minutes.ToString() + "m ";
        }
        string s = difference.Seconds.ToString() + "s left ";

        string res = d + h + m + s;
        tTime.text = res;
    }
}
