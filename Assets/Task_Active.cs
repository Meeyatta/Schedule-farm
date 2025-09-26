using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Task_Active : MonoBehaviour
{
    public TMPro.TMP_Text TextField;
    Task Task_;

    void Start()
    {
        Task_ = transform.parent.GetComponent<Task>();
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
        TextField.text = res;
    }
}
