using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    #region Singleton
    public static MouseManager Instance;
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

    public Vector3 CursorPosition = Vector3.zero;
    public Dictionary<Collider2D, float> CurPointedAtObjs = new Dictionary<Collider2D, float>();

    public float ClickRadius;

    Camera Cam;

    public bool CanClick()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;

        CursorPosition = Camera.main.ScreenToWorldPoint(mousePos);
        CursorPosition.z = 0;

        List<Collider2D> colliders = new List<Collider2D>();
        colliders.AddRange(Physics2D.OverlapCircleAll(CursorPosition, ClickRadius).ToList<Collider2D>());

        #region Checking how long we are pointing at objects
        foreach (var c in colliders) 
        {
            if (CurPointedAtObjs.ContainsKey(c))
            {
                CurPointedAtObjs[c] += Time.deltaTime;
            }
            else
            {
                CurPointedAtObjs.Add(c, 0);
            }
        }

        #endregion

        if (colliders == null || colliders.Count <= 0) 
        {
            CurPointedAtObjs.Clear();
            return false; //No colliders to click on
        } 

        return true;
    }

    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var v in CurPointedAtObjs)
        //{
        //    Debug.Log(v);
        //}
    }
}
