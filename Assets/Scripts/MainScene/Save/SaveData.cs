using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public const int levelCount = 7;
    public int[] level = new int[levelCount];
    public int[] fontNumbers = new int[6];
    public bool[] haveFonts= new bool[20];
}
