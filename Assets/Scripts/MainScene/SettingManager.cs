using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;

    void Start()
    {

    }

    public void PushAllInitializeButton()
    {
        for (int i = 0; i < dataManager.data.level.Count(); i++) dataManager.data.level[i] = 0;
        dataManager.data.fontNumbers = new int[] { 0, 0, 0, -1, -1, -1 };
        for (int i = 0; i < dataManager.data.haveFonts.Count(); i++) dataManager.data.haveFonts[i] = false;
    }
}
