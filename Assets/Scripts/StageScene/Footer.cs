using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footer : MonoBehaviour
{
    private GameObject footer;
    private RectTransform[] buttonsRect = new RectTransform[5];

    [SerializeField] private GameObject[] menuImages = new GameObject[5];

    void Start()
    {
        footer = GameObject.Find("Footer");
        for (int i = 0; i < 5; i++) buttonsRect[i] = footer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();

        PushFooterButton(2);
    }

    public void PushFooterButton(int index)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == index)
            {
                buttonsRect[i].gameObject.transform.localPosition = new Vector3(200 * index - 400, -820);
                buttonsRect[i].sizeDelta = new Vector2(280, 280);
                menuImages[i].SetActive(true);
            }
            else if (i != index)
            {
                buttonsRect[i].gameObject.transform.localPosition = new Vector3(200 * i - 400 + 40 * Mathf.Sign(i - index), -860);
                buttonsRect[i].sizeDelta = new Vector2(200, 200);
                menuImages[i].SetActive(false);
            }
            // Debug.Log(i + ":" + buttonsRect[i].transform.localPosition.x);
        }
    }
}
