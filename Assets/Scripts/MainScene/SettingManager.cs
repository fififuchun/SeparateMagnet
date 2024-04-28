using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;
    [SerializeField] private MissionDataManager missionDataManager;
    [SerializeField] private TutorialDataManager tutorialDataManager;


    // public void PushAllInitializeButton()
    // {
    //     for (int i = 0; i < dataManager.data.level.Count(); i++) dataManager.data.level[i] = 0;
    //     dataManager.data.fontNumbers = new int[] { 0, 0, 0, -1, -1, -1 };
    //     for (int i = 0; i < dataManager.data.haveFonts.Count(); i++) dataManager.data.haveFonts[i] = false;
    // }

    public void PushAllResetButton()
    {
        PlayerPrefs.DeleteAll();
        dataManager.ResetDataManager();
        // missionDataManager.
        
        SceneManager.LoadScene("StageScene");
    }
}
