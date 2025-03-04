using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO; // �t�@�C������p

public class ScreenshotCapture : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public GameObject targetObject;
    public string screenshotFileName = "screenshot.png"; // �t�@�C����
    public Button captureButton;
    public Text remainingCountText;
    public Image shutterOverlay;
    public float shutterDuration = 0.5f;
    public int maxScreenshotCount = 3;
    private int screenshotCount = 0;

    // �ǉ�: �N���A��ʗp��UI�e�L�X�g�Ƒҋ@����
    public Text clearScreenText;
    public float clearScreenDuration = 5f;

    // �v���C���[�̑���X�N���v�g
    public MonoBehaviour playerControllerScript;

    // �V�[���J�ڑO�̑ҋ@���ԁi�b�j
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

    // UI�{�^���������ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    void OnButtonClick()
    {
        if (screenshotCount < maxScreenshotCount)
        {
            StartCoroutine(HandleShutterEffect()); // �V���b�^�[�G�t�F�N�g���J�n
            bool isObjectInScreenshot = CaptureScreenshotAndCheckObject();
            if (isObjectInScreenshot)
            {
                Debug.Log("�I�u�W�F�N�g���X�N���[���V���b�g�Ɋ܂܂�Ă��܂�");
                StartCoroutine(DisplayClearScreenAndTransition());
            }
            else
            {
                Debug.Log("�I�u�W�F�N�g���X�N���[���V���b�g�Ɋ܂܂�Ă��܂���");
            }

            screenshotCount++;  // �X�N���[���V���b�g�񐔂��J�E���g
            UpdateRemainingCountText();
        }
        else
        {
            Debug.Log("�X�N���[���V���b�g�̉񐔐����ɒB���܂���");
        }
    }

    // �V���b�^�[�G�t�F�N�g����������R���[�`��
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
            yield return new WaitForSeconds(0.1f); // �����҂��Ă���L���v�`��

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

    // �c��X�N���[���V���b�g�񐔂�UI�ɕ\������
    void UpdateRemainingCountText()
    {
        int remainingCount = maxScreenshotCount - screenshotCount;
        if (remainingCountText != null)
        {
            remainingCountText.text = "�c��B�e�\��: " + remainingCount.ToString();
        }
    }

    // �X�N���[���V���b�g���L���v�`�����A�I�u�W�F�N�g���܂܂�Ă��邩�𔻒肷��
    public bool CaptureScreenshotAndCheckObject()
    {
        Camera camera = virtualCamera.GetComponent<Camera>(); // Cinemachine�̃J�����擾
        if (camera == null)
        {
            Debug.LogError("Camera�R���|�[�l���g�� 'FirstPersonCamera' �Q�[���I�u�W�F�N�g�ɃA�^�b�`����Ă��܂���I");
            return false; // �J�������Ȃ���Ώ������I��
        }

        // ���o�C�������ɃX�N���[���V���b�g�̉𑜓x��Ⴍ����
        int screenshotWidth = Screen.width / 2;  // �𑜓x�𔼕���
        int screenshotHeight = Screen.height / 2; // �𑜓x�𔼕���
        RenderTexture renderTexture = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        camera.targetTexture = renderTexture;

        // �����_�����O�i�J�����̃r���[��RenderTexture�ɕ`��j
        camera.Render();

        // �X�N���[���V���b�g�̃s�N�Z���f�[�^���擾
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        screenshot.Apply();

        // �X�N���[���V���b�g�̕ۑ�����v���b�g�t�H�[���ɍ��킹�ĕύX
        string screenshotPath = Path.Combine(Application.persistentDataPath, screenshotFileName);

        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(screenshotPath, bytes);

        // �N���[���A�b�v
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // �I�u�W�F�N�g���X�N���[���V���b�g�Ɋ܂܂�Ă��邩�m�F
        return IsObjectInScreenshot(screenshot);
    }

    // �X�N���[���V���b�g���ɃI�u�W�F�N�g���܂܂�Ă��邩���m�F
    private bool IsObjectInScreenshot(Texture2D screenshot)
    {
        Camera camera = virtualCamera.GetComponent<Camera>();
        Vector3 screenPos = camera.WorldToScreenPoint(targetObject.transform.position);

        if (screenPos.z > 0 && screenPos.x >= 0 && screenPos.x < Screen.width && screenPos.y >= 0 && screenPos.y < Screen.height)
        {
            Color pixelColor = screenshot.GetPixel((int)screenPos.x, (int)screenPos.y);
            if (pixelColor.a > 0)
            {
                Debug.Log("�I�u�W�F�N�g���X�N���[���V���b�g�Ɋ܂܂�Ă��܂�");
                return true;
            }
        }
        Debug.Log("�I�u�W�F�N�g���X�N���[���V���b�g�Ɋ܂܂�Ă��܂���");
        return false;
    }

    // �N���A��ʂ�\�����A5�b��ɃV�[���J��
    private IEnumerator DisplayClearScreenAndTransition()
    {
        if (clearScreenText != null)
        {
            clearScreenText.gameObject.SetActive(true);
            clearScreenText.text = "�X�e�[�W�N���A�I";

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
