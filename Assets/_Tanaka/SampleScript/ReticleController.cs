using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleController : MonoBehaviour
{
    public float radius = 2;
    public float speed = 100;
    private Vector3 startPos;
    private float angle;

    public RectTransform rect;

    // �ǉ�����
    public float timeSpan; // �����_�����������s������Ԋu
    public int maxReticleSpeed = 800; // �ő�l (���e�B�N���̃X�s�[�h)
    public int minReticleSpeed = 300; // �ŏ��l(���e�B�N���̃X�s�[�h)
    public int maxSpanTime = 2; // �ő�l�i�X�s�[�h���ς��Ԋu�j
    public int minSpanTime = 0; // �ŏ��l�i�X�s�[�h���ς��Ԋu�j
    public bool isFunc; // ��x�����Ăт�������

    private float timeCount; // ���Ԍv���p
    private int reticleMoveSpeed; // �����_���ŏo�͂����X�s�[�h�l���i�[����ϐ�

    void Start()
    {   
        // �~�ړ��̒��S�n�_�����߂�
        startPos = rect.transform.localPosition;
    }

    private void OnEnable()
    {   
        if(startPos != null)
        {
            rect.transform.localPosition = startPos;
        }
 
    }

    private void FixedUpdate()
    {

        if (!GameManager.isShoot)
        {
            // ���x����肸���Z
            angle += Time.deltaTime * RandomGenerator();
            // ���x���烉�W�A���ɕϊ�
            var rad = angle * Mathf.Deg2Rad;
            // ���x��Int�^�ŃL���X�g
            // �p�x / 360 �̗]�肪0�̏ꍇ��1�A�]�肪�o���ꍇ-1��Ԃ�
            // ? �̍��ӂ�]���ATrue�Ȃ� : �̍��ӁAFalse�Ȃ�E��
            // �]��Ɋւ��āAcast���Ă�̂�360�߂���܂�0���Ԃ�
            // �E��]�A����]�����݂ɌJ��Ԃ�
            var f = (int)angle / 360 % 2 == 0 ? 1 : -1;
            // Mathf.Sign�Ő��������𔻒�
            // 1�A�܂�True�Ȃ̂�
            // ����?�Ɠ������Ƃ����s
            // radius = �͈͂Ȃ̂���?�A�����or���ŕԂ�
            var diffPos = Mathf.Sign(f) == 1 ? -radius : radius;
            // ���g�̍��W�ɑ΂��đ��
            // �~����̍��W�����߂�ɂ́A���a * cos���W�A��, ���a * sin���W�A��
            rect.transform.localPosition =
                startPos +
                new Vector3(
                    // �ȉ~������Ă�?
                    // 
                    Mathf.Cos(rad) * radius * f + diffPos,
                    // ���a�̕�(100�Ȃ�-100 ~ 100)
                    Mathf.Sin(rad) * radius
                    );


            // Sin�֐��́u���a�P�̖_�ɁATime�̎������邮��Ɗ����t���āA�����I������ʒu�������Ă����v
            // ���Ԃ͐i�ݑ������ŕ\���ƒ����ɂȂ�A������~�ɂ���ɂ�
            // Sin�֐���-1 ~ 1�̒l(���)
        }

    }

    // @�ǉ�
    /// <summary>
    /// �����_�������p�̊֐�
    /// </summary>
    private int RandomGenerator()
    {
        if (!isFunc)
        {
            // �����_���Ȑ��l���i�[
            timeSpan = Random.Range(minSpanTime, maxSpanTime);
            Debug.LogWarning("�������_���̊Ԋu" + timeSpan);
            isFunc = true;
        }

        timeCount += Time.deltaTime;

        // �����_���ŏo�͂����Ԋu�ɂȂ�������s
        if (timeCount >= timeSpan)
        {
            // �X�s�[�h�̃����_���ݒ� 
            // �w�肵��臒l�Ń����_���̃X�s�[�h�l���i�[����
            reticleMoveSpeed = Random.Range(minReticleSpeed, maxReticleSpeed);
            Debug.LogWarning("�������_���ȃX�s�[�h" + reticleMoveSpeed);
            isFunc = false;
            // �J�E���g�̏�����
            timeCount = 0;
        }

        // �����_���ŏo�͂����X�s�[�h�l��Ԃ�
        return reticleMoveSpeed;
    }


}
