using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    private GameObject canvas;
    [SerializeField] private GameObject mainViewContent;
    [SerializeField] private RectTransform[] stageButtons;

    // public Slider slider;
    // [SerializeField] private GameObject circleImage;
    // [SerializeField] private GameObject[] circleImages = new GameObject[3];


    void Awake()
    {
        canvas = GameObject.Find("Canvas");

        // stageButtons = new RectTransform[mainViewContent.transform.childCount];
        // for (int i = 0; i < mainViewContent.transform.childCount; i++) stageButtons[i] = mainViewContent.transform.GetChild(i).GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            // Debug.Log(mainViewContent.transform.position);
            mainViewContent.transform.position = new Vector2(Mathf.Floor((mainViewContent.transform.position.x + 415) / 830) * 830, mainViewContent.transform.position.y);
        }

        // for (int i = 0; i < mainViewContent.transform.childCount; i++)
        //     if (Mathf.Abs(stageButtons[i].transform.position.x - canvas.transform.position.x) < stageButtons[i].sizeDelta.x / 2)
        //     {
        //         stageButtons[i].sizeDelta = new Vector2(500, 500);
        //     }
        //     else
        //     {
        //         stageButtons[i].sizeDelta = new Vector2(400, 400);
        //     }

        // circleImage.transform.rotation = quaternion.Euler(0, 0, slider.value * 2 * Mathf.PI);
        // for (int i = 0; i < 3; i++)
        // {
        //     circleImages[i].transform.localRotation = quaternion.Euler(0, 0, -slider.value * 2 * Mathf.PI);
        // }
    }
}
