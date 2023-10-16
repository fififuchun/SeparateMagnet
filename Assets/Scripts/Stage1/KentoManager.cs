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

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        //
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
        if(gameManager.isEndDrag) return;
        transform.position = new Vector3(eventData.position.x, prevPos.y + gameManager.canvas.transform.position.y);
    }

    // ドラッグ終了時の処理
    public void OnEndDrag(PointerEventData eventData)
    {
        if(gameManager.isEndDrag) return;
        gameManager.IsEndDrag(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameObject.GetComponent<KentoManager>().enabled) return;
        gameObject.GetComponent<KentoManager>().enabled = false;

        gameManager.CanInstantiate();
        Debug.Log("着地");
    }
}
