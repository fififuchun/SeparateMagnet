using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    #region "General"

    public int rank;
    public int diamond;
    public int coin;

    #endregion


    #region "RPG"

    public const int levelCount = 7;

    public int[] level = new int[levelCount];
    public int[] fontNumbers = new int[6];
    public bool[] isRareFonts = new bool[MainManager.STAGECOUNT];
    public bool[] haveFonts = new bool[MainManager.NUMofFONTS];

    #endregion


    #region "Mission"

    public const int missionGroupCount = 6;
    public int[] receivedMissionCounts = new int[missionGroupCount];
    public int[] missionValues = new int[missionGroupCount];

    #endregion


    #region "Tutorial"

    public const int tutorialCount = 5;
    public bool[] isFinishedTutorial = new bool[tutorialCount];

    #endregion
}
