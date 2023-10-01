using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class KentoManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //親スクリプトの取得
    private GameManager gameManager;

    // 1フレーム前の位置
    // private Vector3 _prevPosition;

    // 現在のスピード
    // [SerializeField] private float kentoSpeed;
    //kentoSpeedのプロパティ
    // public float KentoSpeed { get => kentoSpeed; }

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
        // 初期位置を保持
        // _prevPosition = transform.position;
    }

    private void Update()
    {
        //kentoの管理はこっち
        if (transform.position.y < -1000) Destroy(gameObject);

        // deltaTimeが0の場合は何もしない
        // if (Mathf.Approximately(Time.deltaTime, 0)) return;

        // 現在位置取得
        // var position = transform.position;

        // 現在速度計算
        // var velocity = (position - _prevPosition) / Time.deltaTime;
        // kentoSpeed = velocity.magnitude;
        // Debug.Log(kentoSpeed);

        // 前フレーム位置を更新
        // _prevPosition = position;
    }

    // ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグ前の位置を記憶しておく、RectTransformの場合はpositionではなくanchoredPositionを使う
        prevPos = rectTransform.anchoredPosition;
        Debug.Log("drag開始" + prevPos);
    }

    // ドラッグ中の処理
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(eventData.position.x, prevPos.y + gameManager.canvas.transform.position.y);
    }

    // ドラッグ終了時の処理
    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 20;
        gameObject.GetComponent<KentoManager>().enabled = false;
        Debug.Log("drag終了");
    }
}
