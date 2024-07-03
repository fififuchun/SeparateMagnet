using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;

    [SerializeField] private CustomButton allResetButton;
    [SerializeField] private CustomButton informationButton;
    [SerializeField] private CustomButton mailButton;
    [SerializeField] private CustomButton postButton;
    [SerializeField] private CustomButton settingButton;
    [SerializeField] private CustomButton returnButton;

    void Start()
    {
        allResetButton.onClickCallback += PushAllResetButton;
        informationButton.onClickCallback += PushInformationButton;
        mailButton.onClickCallback += PushMailButton;
        postButton.onClickCallback += PushPostButton;
        settingButton.onClickCallback += PushSettingButton;
        returnButton.onClickCallback += PushReturnButton;
    }


    public void PushAllResetButton()
    {
        dataManager.ResetDataManager();
        SceneManager.LoadScene("StageScene");
    }

    public void PushInformationButton()
    {
        WarnManager.instance.AppearWarning("機能制限", "機能制限中です！\n詳しくは公式X\n@FuchunGames\nをご覧ください\n\n\n\n");
        WarnManager.instance.PushFollowButton();
    }

    public void PushMailButton()
    {
        WarnManager.instance.AppearWarning("機能制限", "機能制限中です！\n詳しくは公式X\n@FuchunGames\nをご覧ください\n\n\n\n");
        WarnManager.instance.PushFollowButton();
    }

    public void PushPostButton()
    {
        WarnManager.instance.AppearWarning("機能制限", "機能制限中です！\n詳しくは公式X\n@FuchunGames\nをご覧ください\n\n\n\n");
        WarnManager.instance.PushFollowButton();
    }

    public void PushSettingButton()
    {
        WarnManager.instance.AppearWarning("機能制限", "機能制限中です！\n詳しくは公式X\n@FuchunGames\nをご覧ください\n\n\n\n");
        WarnManager.instance.PushFollowButton();
    }

    public void PushReturnButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
