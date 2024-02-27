using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialSaveData
{
    public const int tutorialCount = 4;
    public bool[] isFinishedTutorial = new bool[tutorialCount];
}
