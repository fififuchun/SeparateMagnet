using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public const int levelCount = 7;

    public int[] level = new int[levelCount];
    public int[] fontNumbers = new int[6];
    public bool[] isRareFonts = new bool[MainManager.STAGECOUNT];
    public bool[] haveFonts = new bool[MainManager.NUMofFONTS];

    // public void ResetData()
    // {
    //     level = new int[levelCount];
    //     fontNumbers = new int[6] { 0, 0, 0, -1, -1, -1 };
    //     for (int i = 0; i < isRareFonts.Length; i++) isRareFonts[i] = false;
    //     for (int i = 0; i < haveFonts.Length; i++) haveFonts[i] = false;
    // }
}
