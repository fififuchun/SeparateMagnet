using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;

    [SerializeField] private CustomButton allResetButton;
    // [SerializeField] private CustomButton informationButton;
    // [SerializeField] private CustomButton mailButton;
    // [SerializeField] private CustomButton postButton;
    // [SerializeField] private CustomButton settingButton;
    [SerializeField] private CustomButton returnButton;

    void Start()
    {
        allResetButton.onClickCallback += PushAllResetButton;

        returnButton.onClickCallback += PushReturnButton;
    }

    public void PushAllResetButton()
    {
        dataManager.ResetDataManager();
        SceneManager.LoadScene("StageScene");
    }

    public void PushReturnButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
