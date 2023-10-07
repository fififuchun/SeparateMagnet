using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class exp_ber : MonoBehaviour
{
    int maxExp = 100;//最大の経験値量（暫定レベルアップ時リセット）
    int currentExp = 50;//今の経験値量
    public Slider slider;
    
    // Start is called before the first frame update
    void Start()
    {
        slider.value = currentExp / maxExp;
    }

}
