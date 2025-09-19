using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskMenu : MonoBehaviour
{
    public Vector3 Offset;

    public GameObject oElements;

    public TMPro.TMP_InputField oFieldTime_h;
    public TMPro.TMP_InputField oFieldTime_m;
    public TMPro.TMP_InputField oFieldTime_s;
    
    public TMPro.TMP_InputField oFieldDate_d;
    public TMPro.TMP_InputField oFieldDate_m;
    public TMPro.TMP_InputField oFieldDate_y;

    public TMPro.TMP_Dropdown oDropDown;
    /*
    On creation:
    1) Get the task reference v
    2) Change the menu's position near that task v
    3) Assign possible reminder options to the dropdown v
    4) Enable the TaskMenu v
    */

    /*
    On submit button click:
    1) Transfer the time text into time of DateTime
    2) Transfer the date text into date of DateTime
    3) Transfer the reminder time into double
    4) Assign all the data to the tas kand start it
    5) Close the menu
    */

    #region Singleton
    public static TaskMenu Instance;
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

    public void Menu_enable(Task task)
    {
        //Debug.Log(task.transform.position - transform.position);
        transform.position = task.transform.position + Offset;
        oElements.SetActive(true);
    }

    public void bMenu_submit()
    {
        int year; int month; int day;
        int hour; int minute; int second;

        if (!int.TryParse(oFieldDate_y.text, out year)) { Debug.LogError("Improper year"); }
        if (!int.TryParse(oFieldDate_m.text, out month)) { Debug.LogError("Improper month"); }
        if (!int.TryParse(oFieldDate_d.text, out day)) { Debug.LogError("Improper day"); }

        if (!int.TryParse(oFieldTime_h.text, out hour)) { Debug.LogError("Improper hour"); }
        if (!int.TryParse(oFieldTime_m.text, out minute)) { Debug.LogError("Improper minute"); }
        if (!int.TryParse(oFieldTime_s.text, out second)) { Debug.LogError("Improper second"); }

        DateTime result;
        try
        {
            result = new DateTime(year, month, day, hour, minute, second);
            Debug.Log(result);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.LogError  ("Incorrect date/time format");
        }
        
    }

    void Start()
    {
        Array enumValues = Enum.GetValues(typeof(ReminderType));
        foreach (ReminderType reminder in enumValues)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = reminder.ToString();
            oDropDown.options.Add(option);
        }

        
    }

}
