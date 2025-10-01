using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEditor;

public class Task_Active : MonoBehaviour
{
    public Color StartColor;

    public Color Reminder_color;
    public Color Active_color;
    public Color Acceptable_color;
    public Color Late_color;

    [Header("---")]
    public GameObject oElements;
    public TMPro.TMP_Text tName;
    public TMPro.TMP_Text tTime;
    public RawImage Background;
    Task Task_;
    Animator Anim;

    const string tReminder = "tReminder";
    const string isActive = "isActive";
    const string isAcceptable = "isAcceptable";

    public void Do_Assigned(Task t)
    {
        oElements.SetActive(true);


        tName.text = t.Name;

        Anim.SetBool(isActive, false);
        Anim.SetBool(isAcceptable, false);
    }
    public void Do_WithinReminder(Task t)
    {
        Background.color = Reminder_color;
        Anim.SetTrigger(tReminder);
    }
    public void Do_WithinActive(Task k)
    {
        Background.color = Active_color;
        Anim.SetBool(isActive, true);
    }
    public void Do_WithinAcceptable(Task t)
    {
        Background.color = Acceptable_color;
        Anim.SetBool(isActive, false);
        Anim.SetBool(isAcceptable, true);
    }
    public void Do_WithinLate(Task t)
    {
        Background.color = Late_color;
        Anim.SetBool(isActive, false);
        Anim.SetBool(isAcceptable, false);
    }
    public void Clear()
    {
        Background.color = StartColor;

        Anim.SetBool(isActive, false);
        Anim.SetBool(isAcceptable, false);
    }
    public void Callback()
    {
        if (Task_ == null) { return; }

        void general()
        {
            Clear();

            oElements.SetActive(false);
            Task_.Clear();
        }
        switch (Task_.CurState)
        {
            case State.Active:
                general();
                break;
            case State.StillAcceptable:
                general();
                break;
            default:
                //Don't react
                break;
        }
        
    }

    void Start()
    {
        Anim = transform.parent.GetComponent<Animator>();
        Task_ = transform.parent.GetComponent<Task>();

        Background.color = StartColor;

        Task_.eAssigned.AddListener(Do_Assigned);
        Task_.eWithinReminder.AddListener(Do_WithinReminder);
        Task_.eActive.AddListener(Do_WithinActive);
        Task_.eStillAcceptable.AddListener(Do_WithinAcceptable);
        Task_.eLate.AddListener(Do_WithinLate);
    }
    void TextUpdate()
    {
        TimeSpan difference = Task_.EndTime - DateTime.Now;

        if (difference < TimeSpan.Zero)
        {
            tTime.text = "";
            return;
        }

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
    void Update()
    {
        TextUpdate();
    }
}
