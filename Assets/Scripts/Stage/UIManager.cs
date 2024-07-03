using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    //ステージ選択画面に戻るボタン
    [SerializeField] private CustomButton returnButton;

    void Start()
    {
        returnButton.onClickCallback += PushGoHomeButton;
    }

    public void PushGoHomeButton()
    {
        DOVirtual.DelayedCall(0.24f, () => SceneManager.LoadScene("StageScene"), false);
    }
}
