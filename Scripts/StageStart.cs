using UnityEngine;
using UnityEngine.UI;
using System.Collections;  // コルーチンを使用するために必要

public class StartMessage : MonoBehaviour
{
    public Text messageText;  // メッセージ用Text
    public Image background;  // 背景用Image

    // プレイヤーの操作スクリプト
    public MonoBehaviour playerControllerScript;

    void Start()
    {
        // メッセージの設定
        if (messageText != null)
        {
            messageText.text = "命あるものを撮影しろ";
        }

        // 画面を静止
        Time.timeScale = 0f;

        // 背景の設定（白色背景）
        if (background != null)
        {
            background.color = Color.white;  // 白背景
        }

        ///// 背景をTextの後ろに移動
        ///if (messageText != null && background != null)
        ///{
        ///    background.transform.SetSiblingIndex(messageText.transform.GetSiblingIndex() - 1);
        ///}

        // コルーチンを開始して5秒後にメッセージと背景を消す
        StartCoroutine(ClearMessageAfterDelay());
    }

    // 5秒待ってからメッセージと背景を消すコルーチン
    private IEnumerator ClearMessageAfterDelay()
    {
        // 実時間で待機
        yield return new WaitForSecondsRealtime(3f);

        // メッセージを消す
        if (messageText != null)
        {
            messageText.text = "";
        }

        // 背景を消す
        if (background != null)
        {
            background.gameObject.SetActive(false);  // 背景を非表示にする
        }

        // ゲームの時間を再開
        Time.timeScale = 1f;
    }
}
