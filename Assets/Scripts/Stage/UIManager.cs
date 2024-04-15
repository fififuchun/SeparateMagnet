using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    
    public void PushGoHomeButton()
    {
        PlayerPrefs.SetInt("TmpCoin", gameManager.SumCoin() * MainManager.stageNum);
        SceneManager.LoadScene("StageScene");
    }
}
