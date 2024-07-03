using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KentoManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //親スクリプトの取得
    private GameManager gameManager;

    //保存しておく初期position
    private Vector2 prevPos;

    // 移動したいオブジェクトのRectTransform
    private RectTransform rectTransform;

    //スコア
    public int score;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグ前の位置を記憶しておく、RectTransformの場合はpositionではなくanchoredPositionを使う
        prevPos = rectTransform.anchoredPosition;
    }

    // ドラッグ中の処理
    public void OnDrag(PointerEventData eventData)
    {
        if (/*gameManager.timeManager.isEndDrag || */gameManager.phase != GameManager.Phase.AppearPhase) return;
        transform.position = new Vector3(eventData.position.x, prevPos.y + gameManager.canvas.transform.position.y);
    }

    // ドラッグ終了時の処理
    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameManager.timeManager.isEndDrag) return;
        gameManager.timeManager.isEndDrag = true;
    }

    //着地時の処理
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameObject.GetComponent<KentoManager>().enabled) return;
        gameObject.GetComponent<KentoManager>().enabled = false;

        gameManager.timeManager.isEndDrag = false;
        gameManager.CanInstantiate();
        Debug.Log("To StartPhase");
    }
}
