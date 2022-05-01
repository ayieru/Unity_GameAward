using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChain : MonoBehaviour
{
    //�i��ł������
    private int Direction = 1;

    //Z���̊p�x
    private float Angle = 0.0f;

    //�����n�߂鎞�̎���
    private float StartTime;

    //��ԊԊu
    [SerializeField]
    private float Duration = 5.0f;

    //Z���ŐU��q������p�x
    [SerializeField]
    private float MaxAngle = 90.0f;

    void Start()
    {
        StartTime = Time.time;
    }
    
    void FixedUpdate()
    {

        //�o�ߎ��Ԃɍ��킹���������v�Z
        float time = (Time.time - StartTime) / Duration;

        //�X���[�Y�Ɋp�x���v�Z
        Angle = Mathf.SmoothStep(Angle, Direction * MaxAngle, time);

        //�p�x��ύX
        transform.localEulerAngles = new Vector3(0f, 0f, Angle);

        //�p�x���w�肵���p�x��1�x�̍��ɂȂ����甽�]
        if (Mathf.Abs(Mathf.DeltaAngle(Angle, Direction * MaxAngle)) < 1.0f)
        {
            Direction *= -1;
            StartTime = Time.time;
        }
    }
    //�i��ł��������Ԃ�(���ۂɂ�int�l)
    public int GetDirection()
    {
        return Direction;
    }
}
