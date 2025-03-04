using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO; // ファイル操作用

public class ScreenshotCapture : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public GameObject targetObject;
    public string screenshotFileName = "screenshot.png"; // ファイル名
    public Button captureButton;
    public Text remainingCountText;
    public Image shutterOverlay;
    public float shutterDuration = 0.5f;
    public int maxScreenshotCount = 3;
    private int screenshotCount = 0;

    // 追加: クリア画面用のUIテキストと待機時間
    public Text clearScreenText;
    public float clearScreenDuration = 5f;

    // プレイヤーの操作スクリプト
    public MonoBehaviour playerControllerScript;

    // シーン遷移前の待機時間（秒）
    private float delayTime = 5f;

    void Start()
    {
        if (captureButton != null)
        {
            captureButton.onClick.AddListener(OnButtonClick);
        }

        UpdateRemainingCountText();

        if (shutterOverlay != null)
        {
            shutterOverlay.gameObject.SetActive(false);
        }

        if (clearScreenText != null)
        {
            clearScreenText.gameObject.SetActive(false);
        }
    }

    // UIボタンが押されたときに呼ばれるメソッド
    void OnButtonClick()
    {
        if (screenshotCount < maxScreenshotCount)
        {
            StartCoroutine(HandleShutterEffect()); // シャッターエフェクトを開始
            bool isObjectInScreenshot = CaptureScreenshotAndCheckObject();
            if (isObjectInScreenshot)
            {
                Debug.Log("オブジェクトがスクリーンショットに含まれています");
                StartCoroutine(DisplayClearScreenAndTransition());
            }
            else
            {
                Debug.Log("オブジェクトがスクリーンショットに含まれていません");
            }

            screenshotCount++;  // スクリーンショット回数をカウント
            UpdateRemainingCountText();
        }
        else
        {
            Debug.Log("スクリーンショットの回数制限に達しました");
        }
    }

    // シャッターエフェクトを処理するコルーチン
    IEnumerator HandleShutterEffect()
    {
        if (shutterOverlay != null)
        {
            shutterOverlay.gameObject.SetActive(true);
            float elapsedTime = 0f;
            while (elapsedTime < shutterDuration)
            {
                shutterOverlay.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, elapsedTime / shutterDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            shutterOverlay.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f); // 少し待ってからキャプチャ

            elapsedTime = 0f;
            while (elapsedTime < shutterDuration)
            {
                shutterOverlay.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, elapsedTime / shutterDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            shutterOverlay.gameObject.SetActive(false);
        }
    }

    // 残りスクリーンショット回数をUIに表示する
    void UpdateRemainingCountText()
    {
        int remainingCount = maxScreenshotCount - screenshotCount;
        if (remainingCountText != null)
        {
            remainingCountText.text = "残り撮影可能回数: " + remainingCount.ToString();
        }
    }

    // スクリーンショットをキャプチャし、オブジェクトが含まれているかを判定する
    public bool CaptureScreenshotAndCheckObject()
    {
        Camera camera = virtualCamera.GetComponent<Camera>(); // Cinemachineのカメラ取得
        if (camera == null)
        {
            Debug.LogError("Cameraコンポーネントが 'FirstPersonCamera' ゲームオブジェクトにアタッチされていません！");
            return false; // カメラがなければ処理を終了
        }

        // モバイル向けにスクリーンショットの解像度を低くする
        int screenshotWidth = Screen.width / 2;  // 解像度を半分に
        int screenshotHeight = Screen.height / 2; // 解像度を半分に
        RenderTexture renderTexture = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        camera.targetTexture = renderTexture;

        // レンダリング（カメラのビューをRenderTextureに描画）
        camera.Render();

        // スクリーンショットのピクセルデータを取得
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        screenshot.Apply();

        // スクリーンショットの保存先をプラットフォームに合わせて変更
        string screenshotPath = Path.Combine(Application.persistentDataPath, screenshotFileName);

        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(screenshotPath, bytes);

        // クリーンアップ
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // オブジェクトがスクリーンショットに含まれているか確認
        return IsObjectInScreenshot(screenshot);
    }

    // スクリーンショット内にオブジェクトが含まれているかを確認
    private bool IsObjectInScreenshot(Texture2D screenshot)
    {
        Camera camera = virtualCamera.GetComponent<Camera>();
        Vector3 screenPos = camera.WorldToScreenPoint(targetObject.transform.position);

        if (screenPos.z > 0 && screenPos.x >= 0 && screenPos.x < Screen.width && screenPos.y >= 0 && screenPos.y < Screen.height)
        {
            Color pixelColor = screenshot.GetPixel((int)screenPos.x, (int)screenPos.y);
            if (pixelColor.a > 0)
            {
                Debug.Log("オブジェクトがスクリーンショットに含まれています");
                return true;
            }
        }
        Debug.Log("オブジェクトがスクリーンショットに含まれていません");
        return false;
    }

    // クリア画面を表示し、5秒後にシーン遷移
    private IEnumerator DisplayClearScreenAndTransition()
    {
        if (clearScreenText != null)
        {
            clearScreenText.gameObject.SetActive(true);
            clearScreenText.text = "ステージクリア！";

            if (playerControllerScript != null)
            {
                playerControllerScript.enabled = false;
            }

            Time.timeScale = 0f;

            yield return new WaitForSecondsRealtime(delayTime);

            Time.timeScale = 1f;

            clearScreenText.gameObject.SetActive(false);

            if (playerControllerScript != null)
            {
                playerControllerScript.enabled = true;
            }

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
                SceneManager.LoadScene(0);
        }
    }
}
