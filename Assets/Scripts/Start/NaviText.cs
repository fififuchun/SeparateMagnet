using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using DG.Tweening;
// using Cysharp.Threading.Tasks;

public class NaviText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI naviText;

    void Start()
    {

    }

    void Update()
    {
        naviText.color = new Color32(255, 255, 255, (byte)(128 * Mathf.Sin(4 * Time.time) + 128));
    }
}
