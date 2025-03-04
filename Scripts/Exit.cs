using UnityEngine;
using UnityEngine.SceneManagement;  // シーン管理を使うために必要

public class Exit : MonoBehaviour
{
    // このメソッドをボタンのOnClickイベントに登録
    public void ChangeSceneToStageSelect()
    {
        SceneManager.LoadScene("StageSelect");  // "StageSelect"はシーンの名前
    }
}
