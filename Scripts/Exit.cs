using UnityEngine;
using UnityEngine.SceneManagement;  // �V�[���Ǘ����g�����߂ɕK�v

public class Exit : MonoBehaviour
{
    // ���̃��\�b�h���{�^����OnClick�C�x���g�ɓo�^
    public void ChangeSceneToStageSelect()
    {
        SceneManager.LoadScene("StageSelect");  // "StageSelect"�̓V�[���̖��O
    }
}
