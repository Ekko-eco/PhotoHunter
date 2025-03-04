using UnityEngine;
using UnityEngine.UI;
using System.Collections;  // �R���[�`�����g�p���邽�߂ɕK�v

public class StartMessage : MonoBehaviour
{
    public Text messageText;  // ���b�Z�[�W�pText
    public Image background;  // �w�i�pImage

    // �v���C���[�̑���X�N���v�g
    public MonoBehaviour playerControllerScript;

    void Start()
    {
        // ���b�Z�[�W�̐ݒ�
        if (messageText != null)
        {
            messageText.text = "��������̂��B�e����";
        }

        // ��ʂ�Î~
        Time.timeScale = 0f;

        // �w�i�̐ݒ�i���F�w�i�j
        if (background != null)
        {
            background.color = Color.white;  // ���w�i
        }

        ///// �w�i��Text�̌��Ɉړ�
        ///if (messageText != null && background != null)
        ///{
        ///    background.transform.SetSiblingIndex(messageText.transform.GetSiblingIndex() - 1);
        ///}

        // �R���[�`�����J�n����5�b��Ƀ��b�Z�[�W�Ɣw�i������
        StartCoroutine(ClearMessageAfterDelay());
    }

    // 5�b�҂��Ă��烁�b�Z�[�W�Ɣw�i�������R���[�`��
    private IEnumerator ClearMessageAfterDelay()
    {
        // �����Ԃőҋ@
        yield return new WaitForSecondsRealtime(3f);

        // ���b�Z�[�W������
        if (messageText != null)
        {
            messageText.text = "";
        }

        // �w�i������
        if (background != null)
        {
            background.gameObject.SetActive(false);  // �w�i���\���ɂ���
        }

        // �Q�[���̎��Ԃ��ĊJ
        Time.timeScale = 1f;
    }
}
