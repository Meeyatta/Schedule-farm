using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public Image img;
    void Start()
    {
        //img = GetComponent<Image>();
    }
    public void Click()
    {
        Debug.Log("Clicked");
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                img.color = Color.yellow;
                break;
            case 1:
                img.color = Color.red;
                break;
            case 2:
                img.color = Color.blue;
                break;
            default:
                img.color = Color.green;
                break;
        }

       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
