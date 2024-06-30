using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private GameObject glassObj;
    [SerializeField] private GameObject glassSlider;

    [SerializeField] private RectTransform titleRect;

    //startのcts
    CancellationTokenSource cts_start;


    void Start()
    {
        cts_start = new CancellationTokenSource();
    }

    //
    private bool isAnim = true;

    void Update()
    {
        glassObj.transform.position = new Vector3(glassSlider.transform.position.x, glassObj.transform.position.y);
        // Debug.Log(glassSlider.transform.position.x);

        int titleSize = 600 - (int)Mathf.Abs(glassSlider.transform.position.x - 385);

        titleRect.sizeDelta = new Vector2(titleSize, titleSize);

        if (titleSize > 550 && Input.GetMouseButtonUp(0) && isAnim)
        {
            GoMainAnimation(cts_start.Token).Forget();
            isAnim = false;
        }
    }

    async public UniTask GoMainAnimation(CancellationToken ct_start)
    {
        glassSlider.transform.parent.parent.GetComponent<Slider>().interactable = false;
        float needTime = 1.0f;

        titleRect.DOScale(100, needTime).ToUniTask().Forget();
        titleRect.DOLocalMove(new Vector3(-2200, 9000), needTime).ToUniTask().Forget();

        await UniTask.Delay((int)(1000 * needTime), cancellationToken: ct_start);

        SceneManager.LoadScene("StageScene");
    }

    void OnDestroy()
    {
        cts_start?.Cancel();
    }
}
