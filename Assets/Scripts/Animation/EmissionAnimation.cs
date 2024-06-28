using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

public class EmissionAnimation : MonoBehaviour
{
    /// <summary>
    /// イメージを放出するアニメーション
    /// </summary>
    /// <param name="emission">放出数</param>
    /// <param name="randomSize">ランダムな出現場所の半径</param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    async public static void Receive(GameObject emitImage, int emission, Vector2 randomSize, Vector2 startPos, Vector2 endPos)
    {
        GameObject[] emittedImages = new GameObject[emission];
        for (int i = 0; i < emission; i++)
        {
            Vector2 randomVector = new Vector2(Random.Range(-randomSize.x, randomSize.x), Random.Range(-randomSize.y, randomSize.y));
            emittedImages[i] = Instantiate(emitImage, startPos + randomVector, Quaternion.identity, GameObject.Find("Canvas").transform);
            emittedImages[i].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);

            await UniTask.Delay(100);

            if (i == emission - 1) await emittedImages[i].transform.DOMove(endPos, 1.0f).ToUniTask();
            else emittedImages[i].transform.DOMove(endPos, 1.0f).ToUniTask().Forget();
        }

        for (int i = 0; i < emittedImages.Length; i++) Destroy(emittedImages[i]);
    }
}
