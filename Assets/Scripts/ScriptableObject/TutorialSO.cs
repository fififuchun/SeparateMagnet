using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Create TutorialSO")]
public class TutorialSO : ScriptableObject
{
    //表示用
    public List<TutorialData> tutorialDatas= new List<TutorialData>();
}

//表示用のクラス
[System.Serializable]
public class TutorialData
{

}
