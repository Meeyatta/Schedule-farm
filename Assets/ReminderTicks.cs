using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReminderTicks : MonoBehaviour
{
    public List<Toggle> Toggles = new List<Toggle>();
    public void Check(Toggle toggle)
    {
        if (!toggle.isOn) return;

        foreach (var v in Toggles) { if (v != toggle) { v.isOn = false; } }
    }
}
