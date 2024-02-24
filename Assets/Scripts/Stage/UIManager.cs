using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }


    public void PushGoHomeButton()
    {
        PlayerPrefs.SetInt("TmpCoin", gameManager.SumCoin());
        SceneManager.LoadScene("StageScene");
    }
}
