using UnityEngine;
using UnityEngine.UI;  // Text�R���|�[�l���g���g�����߂ɕK�v
using UnityEngine.SceneManagement;  // �V�[���J�ڂ��s�����߂ɕK�v
using System.Collections;  // IEnumerator ���g�����߂ɕK�v

public class CubeTouch : MonoBehaviour
{
    /// ���b�Z�[�W��\������UI Text
    public Text messageText;

    // �V�[���J�ڑO�̑ҋ@���ԁi�b�j
    private float delayTime = 5f;

    // �v���C���[�̑���X�N���v�g
    public MonoBehaviour playerControllerScript;

    // �Q�[���J�n���Ƀ��b�Z�[�W���\���ɂ���
    private void Start()
    {
        // �ŏ��̓��b�Z�[�W����ɂ���
        messageText.text = "";
    }

    // �v���C���[��Cube�ɐG�ꂽ���̏���
    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[��Cube�ɐG�ꂽ��
        if (other.CompareTag("Player"))
        {
            // ���b�Z�[�W��\��
            messageText.text = "�X�e�[�W�N���A";

            // �R���[�`�����J�n���Ēx�����������s
            StartCoroutine(WaitAndLoadScene());
        }
    }

    // 5�b�҂��Ă���V�[����J�ڂ���R���[�`��
    private IEnumerator WaitAndLoadScene()
    {
        // �v���C���[�̑���𖳌���
        if (playerControllerScript != null)
        {
            playerControllerScript.enabled = false;
        }

        // �Q�[���̎��Ԃ��~����i�L�����N�^�[�̓������~�܂�j
        Time.timeScale = 0f;

        // 5�b�ԑҋ@
        yield return new WaitForSecondsRealtime(delayTime);

        // �Q�[���̎��Ԃ����ɖ߂�
        Time.timeScale = 1f;

        // �X�e�[�W�̃N���A�����擾
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
            //�X�e�[�W�I����ʂɖ߂�
            SceneManager.LoadScene(0);
    }
}
