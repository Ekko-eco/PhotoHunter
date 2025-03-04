using UnityEngine;
using System.Collections;  // �R���[�`�����g�����߂ɕK�v

public class RotateObject : MonoBehaviour
{
    public Transform target;  // ��]���������I�u�W�F�N�g
    private bool canRotate = false;  // ��]���J�n���邽�߂̃t���O

    void Start()
    {
        // �V�[���J�n��5�b��ɉ�]���J�n
        StartCoroutine(StartRotationAfterDelay(5f));
    }

    void Update()
    {
        // ��]���J�n����t���O�����������]�����s
        if (canRotate && target != null)
        {
            float rotationSpeed = 1f; // 360�x �� 360�b = 1�x/�b
            target.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    // �w�肵�����Ԍ�ɉ�]���J�n����R���[�`��
    private IEnumerator StartRotationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // �w�莞�Ԃ����ҋ@
        canRotate = true;  // ��]���J�n����t���O�𗧂Ă�
    }
}
