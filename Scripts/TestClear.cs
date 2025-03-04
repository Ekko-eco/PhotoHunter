using UnityEngine;
using UnityEngine.UI;  // Textコンポーネントを使うために必要
using UnityEngine.SceneManagement;  // シーン遷移を行うために必要
using System.Collections;  // IEnumerator を使うために必要

public class CubeTouch : MonoBehaviour
{
    /// メッセージを表示するUI Text
    public Text messageText;

    // シーン遷移前の待機時間（秒）
    private float delayTime = 5f;

    // プレイヤーの操作スクリプト
    public MonoBehaviour playerControllerScript;

    // ゲーム開始時にメッセージを非表示にする
    private void Start()
    {
        // 最初はメッセージを空にする
        messageText.text = "";
    }

    // プレイヤーがCubeに触れた時の処理
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがCubeに触れた時
        if (other.CompareTag("Player"))
        {
            // メッセージを表示
            messageText.text = "ステージクリア";

            // コルーチンを開始して遅延処理を実行
            StartCoroutine(WaitAndLoadScene());
        }
    }

    // 5秒待ってからシーンを遷移するコルーチン
    private IEnumerator WaitAndLoadScene()
    {
        // プレイヤーの操作を無効化
        if (playerControllerScript != null)
        {
            playerControllerScript.enabled = false;
        }

        // ゲームの時間を停止する（キャラクターの動きも止まる）
        Time.timeScale = 0f;

        // 5秒間待機
        yield return new WaitForSecondsRealtime(delayTime);

        // ゲームの時間を元に戻す
        Time.timeScale = 1f;

        // ステージのクリア数を取得
        int StageUnlock = PlayerPrefs.GetInt("StageUnlock");
        int NextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (NextScene < 5)
        {
            if (StageUnlock < NextScene)
            {
                PlayerPrefs.SetInt("StageUnlock", NextScene);
            }
            SceneManager.LoadScene(0);
        }
        else
            //ステージ選択画面に戻る
            SceneManager.LoadScene(0);
    }
}
