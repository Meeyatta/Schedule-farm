using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    public TMPro.TMP_InputField oFieldName;
    public TMPro.TMP_InputField oFieldDescription;

    public Toggle tNone; public Toggle tMin; public Toggle t5Min; public Toggle t15Min; public Toggle tHour;

    public BoxCollider2D Coll;
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

    Task CurTask;
    public void Menu_enable(Task task)
    {
        //Debug.Log(task.transform.position - transform.position);
        CurTask = task;
        transform.position = task.transform.position + Offset;
        oElements.SetActive(true);

        #region Autofilling certain fields
        DateTime curDT = DateTime.Now;

        string curYear = curDT.Year.ToString();
        string curMonth = curDT.Month.ToString();
        string curDay = curDT.Day.ToString();

        string curHour = curDT.Hour.ToString();
        string curMinutes = "";
        string curSecond = "";

        oFieldDate_y.text = curYear;
        oFieldDate_m.text = curMonth;
        oFieldDate_d.text = curDay;
        oFieldTime_h.text = curHour;
        oFieldTime_m.text = curMinutes;
        oFieldTime_s.text = curSecond;
        #endregion
    }
    public void Menu_disable()
    {
        oElements.SetActive(false);
    }

    public void bMenu_submit()
    {
        if (CurTask == null) return;

        int year; int month; int day;
        int hour; int minute; int second;

        #region Cancel submition due to incorrect format
        if (!int.TryParse(oFieldDate_y.text, out year)) { Debug.LogError("Improper year");  }
        if (!int.TryParse(oFieldDate_m.text, out month)) { Debug.LogError("Improper month");  }
        if (!int.TryParse(oFieldDate_d.text, out day)) { Debug.LogError("Improper day"); }

        if (!int.TryParse(oFieldTime_h.text, out hour)) { Debug.LogError("Improper hour"); }
        if (!int.TryParse(oFieldTime_m.text, out minute)) { Debug.LogError("Improper minute"); }
        if (!int.TryParse(oFieldTime_s.text, out second)) { Debug.LogError("Improper second"); }
        #endregion

        DateTime result = DateTime.Now;
        try
        {
            result = new DateTime(year, month, day, hour, minute, second);
            Debug.Log(result);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.LogError("Incorrect date/time format");
        }

        #region Cancel submission due to incorrent data
        if (result < DateTime.Now) 
        {
            Debug.LogError("Datetime set is earlier than current");
            return;
        }
        #endregion

        ReminderType reminder = ReminderType.None;

        if (tNone.isOn) reminder = ReminderType.None;
        if (tMin.isOn) reminder = ReminderType.OneM;
        if (t5Min.isOn) reminder = ReminderType.FiveM;
        if (t15Min.isOn) reminder = ReminderType.FifteenM;
        if (tHour.isOn) reminder = ReminderType.Hour;


        Debug.LogFormat("Submitted: {0}, {1}, {2}, {3}", result, oFieldName.text, oFieldDescription.text, reminder);
        CurTask.Assign(result ,oFieldName.text, oFieldDescription.text, reminder);
        Menu_disable();

    }

    void Start()
    {


    }
    
    void OnEnable()
    {
        MouseManager.Instance.eClickedOutsideElements.AddListener(Menu_disable);
    }
    void OnDisable() { }

    void CheckIfShouldCloseMenu()
    {
        if (Input.GetMouseButtonDown(0) && !Coll.OverlapPoint(MouseManager.Instance.CursorPosition))
        {
            Menu_disable();
        }
    }

    void Update()
    {
        CheckIfShouldCloseMenu();
    }

}
