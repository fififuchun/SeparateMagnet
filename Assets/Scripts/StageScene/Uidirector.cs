using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Uidirector : MonoBehaviour
{
    float maxExp = 100;//最大の経験値量（暫定レベルアップ時リセット）
    float currentExp = 50;//今の経験値量
    public Slider slider;
    
    // Start is called before the first frame update
    void Start()
    {
        slider.value = currentExp / maxExp;
        Debug.Log(slider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
