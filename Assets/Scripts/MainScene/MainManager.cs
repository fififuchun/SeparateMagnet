using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
// using UnityEditor.Experimental.GraphView;

public class MainManager : MonoBehaviour
{
    [SerializeField] private GameObject mainViewContent;
    [SerializeField] private RectTransform[] stageImages;


    void Awake()
    {
        //
    }

    [SerializeField] private ScrollRect scrollRect;
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Debug.Log(mainViewContent.transform.position);
            scrollRect.enabled = false;
            mainViewContent.transform.DOKill();
            mainViewContent.transform.DOMove(new Vector2(Mathf.Floor((mainViewContent.transform.position.x + 415) / 830) * 830, mainViewContent.transform.position.y), 0.5f);
        }
        else if (!scrollRect.enabled) scrollRect.enabled = true;


        // for (int i = 0; i < mainViewContent.transform.childCount; i++)
        //     if (Mathf.Abs(stageButtons[i].transform.position.x - canvas.transform.position.x) < stageButtons[i].sizeDelta.x / 2)
        //     {
        //         stageButtons[i].sizeDelta = new Vector2(500, 500);
        //     }
        //     else
        //     {
        //         stageButtons[i].sizeDelta = new Vector2(400, 400);
        //     }
    }
}