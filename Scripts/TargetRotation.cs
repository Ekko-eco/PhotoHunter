using UnityEngine;
using System.Collections;  // コルーチンを使うために必要

public class RotateObject : MonoBehaviour
{
    public Transform target;  // 回転させたいオブジェクト
    private bool canRotate = false;  // 回転を開始するためのフラグ

    void Start()
    {
        // シーン開始後5秒後に回転を開始
        StartCoroutine(StartRotationAfterDelay(5f));
    }

    void Update()
    {
        // 回転を開始するフラグが立ったら回転を実行
        if (canRotate && target != null)
        {
            float rotationSpeed = 1f; // 360度 ÷ 360秒 = 1度/秒
            target.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    // 指定した時間後に回転を開始するコルーチン
    private IEnumerator StartRotationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // 指定時間だけ待機
        canRotate = true;  // 回転を開始するフラグを立てる
    }
}
