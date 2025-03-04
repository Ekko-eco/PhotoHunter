using UnityEngine;
using UnityEngine.UI;  // UI�p���O���
using Cinemachine;    // Cinemachine�p���O���

public class CameraChanger : MonoBehaviour
{
    public CinemachineVirtualCamera thirdPersonCamera;  // �T�[�h�p�[�\���J�����iCinemachine�j
    public CinemachineVirtualCamera firstPersonCamera;  // �t�@�[�X�g�p�[�\���J�����iCinemachine�j
    public Button switchCameraButton;  // UI�{�^��

    // 2�̃I�u�W�F�N�g���C���X�y�N�^����ݒ�
    public GameObject targetObject;  // 1�ڂ̃I�u�W�F�N�g
    ///public GameObject targetObject2;  // 2�ڂ̃I�u�W�F�N�g

    // �J�ڑ��x�i�J�������x�j
    public float transitionSpeed = 1f;

    void Start()
    {
        // �{�^�����N���b�N���ꂽ����SwitchCamera���\�b�h�����s
        if (switchCameraButton != null)
        {
            switchCameraButton.onClick.AddListener(SwitchCamera);
        }

        // �����ݒ�ŃT�[�h�p�[�\���J������Priority���������Ă���
        thirdPersonCamera.Priority = 10;
        firstPersonCamera.Priority = 0;
    }

    // �J������؂�ւ��郁�\�b�h
    void SwitchCamera()
    {
        // �T�[�h�p�[�\���J�������A�N�e�B�u�Ȃ�t�@�[�X�g�p�[�\���J�������A�N�e�B�u�ɁA�t�̏ꍇ�͂��̋t
        if (thirdPersonCamera.Priority > firstPersonCamera.Priority)
        {
            // �T�[�h�p�[�\���J������Priority��Ⴍ���āA�t�@�[�X�g�p�[�\���J������D��
            thirdPersonCamera.Priority = 0;
            firstPersonCamera.Priority = 10;

            // 2�̃I�u�W�F�N�g�̕\��/��\����؂�ւ�
            ToggleObjects();

            // �J�����J�ڂ𑬂����邽�߂�CinemachineBlend�ݒ��ύX
            SetCinemachineBlend(transitionSpeed);
        }
        else
        {
            // �t�@�[�X�g�p�[�\���J������Priority��Ⴍ���āA�T�[�h�p�[�\���J������D��
            firstPersonCamera.Priority = 0;
            thirdPersonCamera.Priority = 10;

            // 2�̃I�u�W�F�N�g�̕\��/��\����؂�ւ�
            ToggleObjects();

            // �J�����J�ڂ𑬂����邽�߂�CinemachineBlend�ݒ��ύX
            SetCinemachineBlend(transitionSpeed);
        }
    }

    // �J�ڑ��x��ݒ肷�郁�\�b�h
    void SetCinemachineBlend(float speed)
    {
        // Cinemachine��Blend�̐ݒ��ύX
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null)
        {
            // `CinemachineBlend`�̐ݒ��ύX���đJ�ڑ��x�𐧌�
            brain.m_DefaultBlend.m_Time = speed;
        }
    }

    // 2�̃I�u�W�F�N�g�̕\��/��\����؂�ւ��郁�\�b�h
    void ToggleObjects()
    {
        if (targetObject != null)
        {
            // ���ꂼ��̃I�u�W�F�N�g�𔽓]
            targetObject.SetActive(!targetObject.activeSelf);
            ///targetObject2.SetActive(!targetObject2.activeSelf);
        }
        else
        {
            Debug.LogError("�^�[�Q�b�g�I�u�W�F�N�g���ݒ肳��Ă��܂���I");
        }
    }
}
