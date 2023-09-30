using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class KentoManager : MonoBehaviour
{
    public static KentoManager instance;
    void Awake() { instance = this; }

    // 1フレーム前の位置
    private Vector3 _prevPosition;
    // 現在のスピード
    public float kentoSpeed;

    private void Start()
    {
        // 初期位置を保持
        _prevPosition = transform.position;
    }

    private void Update()
    {
        // if (gameObject.transform.position.y < -1500) Destroy(gameObject);

        // deltaTimeが0の場合は何もしない
        if (Mathf.Approximately(Time.deltaTime, 0)) return;

        // 現在位置取得
        var position = transform.position;

        // 現在速度計算
        var velocity = (position - _prevPosition) / Time.deltaTime;
        kentoSpeed = velocity.magnitude;
        Debug.Log(kentoSpeed);

        // 現在速度をログ出力
        // Debug.Log($"velocity = {velocity}");

        // 前フレーム位置を更新
        _prevPosition = position;
    }
}
