using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Go_Stage_1()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage_1");
    }
    
    public void Go_Stage_2()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage_2");
    }
}
