using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    // private TimeManager timeManager;
    public GameManager gameManager;

    void Start()
    {
        // timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
        gameManager = GetComponent<GameManager>();
    }


    public void PushGoHomeButton()
    {
        PlayerPrefs.SetInt("TmpCoin", gameManager.SumCoin());
        // Debug.Log(gameManager.SumCoin());
        SceneManager.LoadScene("StageScene");
    }
}
