using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using System.Threading;

public class Test : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;

    int waitCount = 5;

    CancellationTokenSource cts;

    async void Start()
    {
        ChangeText1().Forget();
    }

    async UniTask ChangeText1()
    {
        for (int i = 0; i < waitCount; i++)
        {
            await UniTask.Delay(1000);
            text1.text = (5 - i).ToString();
        }
    }

    // async UniTask Loop()
    // {
    //     while (true)
    //     {

    //     }
    // }
}
