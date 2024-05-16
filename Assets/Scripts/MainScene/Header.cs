using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Header : MonoBehaviour
{
    [SerializeField] private RectTransform header;

    [SerializeField] private RectTransform shoppingView;
    [SerializeField] private RectTransform rpgView;
    [SerializeField] private RectTransform missionView;
    [SerializeField] private RectTransform settingView;

    // private float adTopPadding = header.parent.GetComponent<RectTransform>().sizeDelta.y;

    void Start()
    {
        float adTopPadding = header.parent.GetComponent<RectTransform>().sizeDelta.y / 16 + 50;

        header.localPosition = new Vector3(header.transform.localPosition.x, header.parent.position.y - adTopPadding);

        float viewTopPadding = adTopPadding + header.sizeDelta.y / 2;

        shoppingView.offsetMax = new Vector2(shoppingView.offsetMax.x, -viewTopPadding);
        rpgView.offsetMax = new Vector2(rpgView.offsetMax.x, -viewTopPadding);
        missionView.offsetMax = new Vector2(missionView.offsetMax.x, -viewTopPadding);
        settingView.offsetMax = new Vector2(settingView.offsetMax.x, -viewTopPadding);
    }
}
