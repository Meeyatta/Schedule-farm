using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HoverMenu : MonoBehaviour
{
    #region Singleton
    public static HoverMenu Instance;
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

    public float Time_enable;
    public float Time_disable;
    public Vector3 Offset;

    [Header("---")]
    public float CurDisableTime = 0;

    public bool IsEnabled = false;
    [Header("-")]
    public TMPro.TMP_Text oText_name;
    public TMPro.TMP_Text oText_desc;
    public TMPro.TMP_Text oText_time;
    public TMPro.TMP_Text oText_date;

    public GameObject oElements;
    Dictionary<Collider2D, Task> AllTasks = new Dictionary<Collider2D, Task>();

    void Enable(List<Collider2D> colliders)
    {
        //Debug.Log("Got enabled around " + colliders.Count);

        Task task = null;
        Collider2D collider = null;
        foreach (var coll in colliders)
        {
            Debug.Log(coll + " " + coll.tag);
            if (AllTasks.TryGetValue(coll, out task))
            {
                collider = coll;
                break;
            }

            if (coll.tag == "TaskMenu")
            {
                collider = null;
                break;
            }
        }

        if (task == null || collider == null) { return; }
        if (!task.IsAssigned) return;

        oElements.transform.position = MouseManager.Instance.CursorPosition + Offset;

        oText_name.text = task.Name;
        oText_desc.text = task.Description;
        oText_time.text = task.EndTime.Hour + ":" + task.EndTime.Minute + ":" + task.EndTime.Second;
        oText_date.text = task.EndTime.Day + "." + task.EndTime.Month + "." + task.EndTime.Year;

        CurDisableTime = 0;
        IsEnabled = true;
        oElements.SetActive(true);
    }
    void Disable()
    {
        CurDisableTime = 0;
        IsEnabled = false;
        oElements.SetActive(false);
    }

    void Update()
    {
        #region Disabled - try to enable if we are pointing at something
        if (!IsEnabled)
        {
            bool can = false;
            foreach (var v in MouseManager.Instance.CurPointedAtObjs)
            {
                if (v.Value >= Time_enable)
                {
                    can = true;
                }
            }
            if (can) { Enable(MouseManager.Instance.CurPointedAtObjs.Keys.ToList<Collider2D>()); }
        }
        #endregion 
        #region Enabled - try to disable if we spent enough time not focusing on it
        else
        {
            CurDisableTime += Time.deltaTime;
            if (CurDisableTime > Time_disable && MouseManager.Instance.CurPointedAtObjs.Count <= 0)
            {
                Disable();
            }
        }
        #endregion
    }

    void Start()
    {
        List<Task> all = FindObjectsOfType<Task>().ToList();
        foreach (var v in all)
        {
            AllTasks.Add(v.GetComponent<Collider2D>(), v);
            
        }
        Debug.Log("Found " +  AllTasks.Count);
    }
}
