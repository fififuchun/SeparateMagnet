using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    #region "General"

    // public int rank;
    public int diamond;
    public int coin;
    public int sumcoin;

    public int lastStageNum;
    public static int tax;

    #endregion


    #region "RPG"

    public const int LEVEL_COUNT = 7;

    public int[] level = new int[LEVEL_COUNT];
    public int[] fontNumbers = new int[6];
    public bool[] isRareFonts = new bool[MainManager.STAGE_COUNT];
    public bool[] haveFonts = new bool[MainManager.FONT_COUNT];

    #endregion


    #region "Mission"

    public const int MISSIONGROUP_COUNT = 6;
    public int[] receivedMissionCounts = new int[MISSIONGROUP_COUNT];
    public int[] missionValues = new int[MISSIONGROUP_COUNT];

    #endregion


    #region "Tutorial"

    public const int TUTORIAL_COUNT = 5;
    public bool[] isFinishedTutorial = new bool[TUTORIAL_COUNT];

    #endregion
}
