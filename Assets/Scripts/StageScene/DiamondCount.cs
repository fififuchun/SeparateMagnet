using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiamondCount : MonoBehaviour
{
    //ダイアモンドプロパティ
    private int diamond;
    public int Diamond { get => diamond; }

    //ヘッダーのダイアモンドテキスト
    [SerializeField] private TextMeshProUGUI diamondText;

    void Start()
    {

    }

    void Update()
    {

    }

    public void GetDiamond(int i)
    {
        diamond += i;
    }

    public void UpdateDiamond()
    {
        diamondText.text = Diamond.ToString();
    }
}
