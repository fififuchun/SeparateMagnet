using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//「検討を重ねる」ボタンの上下に動くアニメーション
public class KentoButtonAnim : MonoBehaviour
{
    [SerializeField] private GameObject mainViewContent;
    [SerializeField] private RectTransform kentoButtonRect;

    void Update()
    {
        float kentoButtonPosYRatio = Mathf.Abs(1 - 2 * (Mathf.Abs(-mainViewContent.transform.localPosition.x / 830) % 1));
        gameObject.transform.localPosition = new Vector3(0, 500 * kentoButtonPosYRatio - 900);

        // float kentoButtonPosYRatio_reverse = Mathf.Abs(kentoButtonPosYRatio - 1);
        // kentoButtonRect.localScale = new Vector3(kentoButtonPosYRatio_reverse, kentoButtonPosYRatio_reverse);
    }
}
