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

    public float ClickRadius;

    Camera Cam;

    public bool CanClick()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        worldPosition.z = 0;

        List<Collider2D> colliders = new List<Collider2D>();
        colliders.AddRange(Physics2D.OverlapCircleAll(worldPosition, ClickRadius).ToList<Collider2D>());

        if (colliders == null || colliders.Count <= 0) return false; //No colliders to click on

        return true;
    }

    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
