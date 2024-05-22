using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;
    // [SerializeField] private MissionDataManager missionDataManager;
    // [SerializeField] private TutorialDataManager tutorialDataManager;

    [SerializeField] private CustomButton allResetButton;

    void Start()
    {
        allResetButton.onClickCallback += PushAllResetButton;
    }

    public void PushAllResetButton()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        dataManager.ResetDataManager();

        SceneManager.LoadScene("StageScene");
    }
}
