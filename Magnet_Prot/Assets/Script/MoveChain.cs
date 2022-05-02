using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChain : MonoBehaviour
{
    [Header("�U��q�̉����Ԋu(�l���傫����������蓮��)")]
    [SerializeField]
    private float Duration = 5.0f;

    [Header("Z���ŐU��q������p�x(�����̌��E�l)")]
    [SerializeField]
    private float MaxAngle = 90.0f;

    [Header("���[�v�����̈ʒu�ɖ߂��X�s�[�h")]
    [SerializeField]
    private float UndoSpeed = 2.0f;

    [Header("���[�v�������Ă��邩")]
    [SerializeField]
    private bool MoveFlag = false;

    private float DefaultMaxAngle;

    //�i��ł������
    private int ChainDirection = 1;

    private int UndoDirection;

    //Z���̊p�x
    private float Angle = 0.0f;

    //�����n�߂鎞�̎���
    private float StartTime;

    void Start()
    {
        StartTime = Time.time;

        DefaultMaxAngle = MaxAngle;

        MoveFlag = false;

        UndoDirection = ChainDirection;
    }
    
    void FixedUpdate()
    {
        
        if (MoveFlag)//���[�v��͂�ł��鎞�̃��[�v�̓���
        {
            //�o�ߎ��Ԃɍ��킹���������v�Z
            float t = (Time.time - StartTime) / Duration;

            //�@�X���[�Y�Ɋp�x���v�Z
            Angle = Mathf.SmoothStep(Angle, ChainDirection * MaxAngle, t);

            //�p�x��ύX
            transform.localEulerAngles = new Vector3(0f, 0f, Angle);

            //�p�x���w�肵���p�x��1�x�̍��ɂȂ����甽�]
            if (Mathf.Abs(Mathf.DeltaAngle(Angle, ChainDirection * MaxAngle)) < 1f)
            {
                ChainDirection *= -1;
                StartTime = Time.time;
            }
            
        }
        else//���[�v�𗣂��Ă��鎞
        {
            //�����̊p�x�ɂȂ�܂ŐU��q���J��Ԃ�
            if (transform.localEulerAngles.z != 0)
            {

                //�@���X�Ɍ��E�p�x������������
                if (MaxAngle > 0f)
                {
                    MaxAngle -= UndoSpeed * Time.deltaTime;
                }

                //�@�o�ߎ��Ԃɍ��킹���������v�Z
                float t = (Time.time - StartTime) / Duration;

                //�@�X���[�Y�Ɋp�x���v�Z
                Angle = Mathf.SmoothStep(Angle, UndoDirection * MaxAngle, t);

                //�@�p�x��ύX
                transform.localEulerAngles = new Vector3(0f, 0f, Angle);

                //�@�p�x���w�肵���p�x��1�x�̍��ɂȂ����甽�]
                if (Mathf.Abs(Mathf.DeltaAngle(Angle, UndoDirection * MaxAngle)) < 1f)
                {
                    UndoDirection *= -1;
                    StartTime = Time.time;
                }
            }
        }
    }
    //�i��ł��������Ԃ�
    public int GetChainDirection(){ return ChainDirection; }

    public void SetChainDirection(int dir) { ChainDirection = dir; }

    public void SetMoveFlag(bool enable)
    {
        MoveFlag = enable;
        MaxAngle = DefaultMaxAngle;
        
        if (!MoveFlag)//���[�v�𗣂�����
        {
            //���[�v�𗣂������p�̕����l��direction������
            UndoDirection = ChainDirection;
            
        }
        else//���[�v��͂񂾎�
        {
            StartTime = Time.time;
        }
    }

    public bool GetMoveFlag(){ return MoveFlag; }
}
