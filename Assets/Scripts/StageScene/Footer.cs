using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footer : MonoBehaviour
{
    //FooterのEmptyObject
    private GameObject footer;

    //ButtonのRect
    private RectTransform[] buttonsRects = new RectTransform[5];

    //FooterのButtonそのもの
    [SerializeField] private GameObject[] menuObjects = new GameObject[5];

    //Buttonの中のイメージ
    [SerializeField] private RectTransform[] footerImagesRects = new RectTransform[5];

    void Start()
    {
        footer = GameObject.Find("Footer");
        for (int i = 0; i < 5; i++) buttonsRects[i] = footer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();

        PushFooterButton(2);
    }

    public void PushFooterButton(int index)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == index)
            {
                buttonsRects[i].gameObject.transform.localPosition = new Vector3(200 * index - 400, -820);
                buttonsRects[i].sizeDelta = new Vector2(280, 280);
                footerImagesRects[i].sizeDelta = new Vector2(200, 200);
                if (i == 2) footerImagesRects[i].sizeDelta = new Vector2(300, 150);
                menuObjects[i].SetActive(true);
            }
            else if (i != index)
            {
                buttonsRects[i].gameObject.transform.localPosition = new Vector3(200 * i - 400 + 40 * Mathf.Sign(i - index), -860);
                buttonsRects[i].sizeDelta = new Vector2(200, 200);
                footerImagesRects[i].sizeDelta = new Vector2(100, 100);
                if (i == 2) footerImagesRects[i].sizeDelta = new Vector2(200, 100);
                menuObjects[i].SetActive(false);
            }
        }
    }
}
