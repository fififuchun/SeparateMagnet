using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject tutorial;

    public void InstantiateTutorial(Sprite sprite, string message)
    {
        GameObject tutorialPrefab = Instantiate(tutorial, canvas.transform);

        if (tutorialPrefab.transform.GetChild(0).gameObject == null) return;
        GameObject secretaryObj = tutorialPrefab.transform.GetChild(0).gameObject;

        if (secretaryObj.transform.GetChild(0).GetComponent<Image>() != null)
            secretaryObj.transform.GetChild(0).GetComponent<Image>().sprite = sprite;

        if (secretaryObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>() != null)
            secretaryObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
    }

    //----------------

    [SerializeField] private List<string[]> stringGroups= new List<string[]>();
    //  string[,] stringGroups= new string[,];

    public void PushTutorial(string[] strings)
    {
        for (int i = 0; i < strings.Length; i++)
        {
            
        }
    }
}
