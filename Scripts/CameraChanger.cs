using UnityEngine;
using UnityEngine.UI;  // UI用名前空間
using Cinemachine;    // Cinemachine用名前空間

public class CameraChanger : MonoBehaviour
{
    public CinemachineVirtualCamera thirdPersonCamera;  // サードパーソンカメラ（Cinemachine）
    public CinemachineVirtualCamera firstPersonCamera;  // ファーストパーソンカメラ（Cinemachine）
    public Button switchCameraButton;  // UIボタン

    // 2つのオブジェクトをインスペクタから設定
    public GameObject targetObject;  // 1つ目のオブジェクト
    ///public GameObject targetObject2;  // 2つ目のオブジェクト

    // 遷移速度（カメラ速度）
    public float transitionSpeed = 1f;

    void Start()
    {
        // ボタンがクリックされた時にSwitchCameraメソッドを実行
        if (switchCameraButton != null)
        {
            switchCameraButton.onClick.AddListener(SwitchCamera);
        }

        // 初期設定でサードパーソンカメラのPriorityを高くしておく
        thirdPersonCamera.Priority = 10;
        firstPersonCamera.Priority = 0;
    }

    // カメラを切り替えるメソッド
    void SwitchCamera()
    {
        // サードパーソンカメラがアクティブならファーストパーソンカメラをアクティブに、逆の場合はその逆
        if (thirdPersonCamera.Priority > firstPersonCamera.Priority)
        {
            // サードパーソンカメラのPriorityを低くして、ファーストパーソンカメラを優先
            thirdPersonCamera.Priority = 0;
            firstPersonCamera.Priority = 10;

            // 2つのオブジェクトの表示/非表示を切り替え
            ToggleObjects();

            // カメラ遷移を速くするためにCinemachineBlend設定を変更
            SetCinemachineBlend(transitionSpeed);
        }
        else
        {
            // ファーストパーソンカメラのPriorityを低くして、サードパーソンカメラを優先
            firstPersonCamera.Priority = 0;
            thirdPersonCamera.Priority = 10;

            // 2つのオブジェクトの表示/非表示を切り替え
            ToggleObjects();

            // カメラ遷移を速くするためにCinemachineBlend設定を変更
            SetCinemachineBlend(transitionSpeed);
        }
    }

    // 遷移速度を設定するメソッド
    void SetCinemachineBlend(float speed)
    {
        // CinemachineのBlendの設定を変更
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null)
        {
            // `CinemachineBlend`の設定を変更して遷移速度を制御
            brain.m_DefaultBlend.m_Time = speed;
        }
    }

    // 2つのオブジェクトの表示/非表示を切り替えるメソッド
    void ToggleObjects()
    {
        if (targetObject != null)
        {
            // それぞれのオブジェクトを反転
            targetObject.SetActive(!targetObject.activeSelf);
            ///targetObject2.SetActive(!targetObject2.activeSelf);
        }
        else
        {
            Debug.LogError("ターゲットオブジェクトが設定されていません！");
        }
    }
}
