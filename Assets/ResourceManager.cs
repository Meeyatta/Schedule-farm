using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    public GameObject oShopMenu;
    public int Money_Current;

    public TMPro.TMP_Text oCurMoney;

    public void Add(int amount)
    {
        Money_Current += amount;
    }

    void UpdateVisuals()
    {
        oCurMoney.text = Money_Current.ToString();
    }

    #region Singleton
    public static ResourceManager Instance;
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

    void Update()
    {
        UpdateVisuals();
    }

    public void Shop_enable()
    {
        oShopMenu.SetActive(true);
    }
    public void Shop_disable()
    {
        oShopMenu.SetActive(false);
    }
}
