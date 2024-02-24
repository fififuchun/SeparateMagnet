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

    void Awake()
    {
        GetDiamond(PlayerPrefs.GetInt("Diamond", 0));
        GetDiamond(100);
    }

    void Update()
    {

    }

    public void GetDiamond(int i)
    {
        diamond += i;
        UpdateDiamond();
        PlayerPrefs.SetInt("Diamond", diamond);
    }

    public void UpdateDiamond()
    {
        diamondText.text = Diamond.ToString();
    }
}
