using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChain : MonoBehaviour
{
    //進んでいる方向
    private int Direction = 1;

    //Z軸の角度
    private float Angle = 0.0f;

    //動き始める時の時間
    private float StartTime;

    //補間間隔
    [SerializeField]
    private float Duration = 5.0f;

    //Z軸で振り子をする角度
    [SerializeField]
    private float MaxAngle = 90.0f;

    void Start()
    {
        StartTime = Time.time;
    }
    
    void FixedUpdate()
    {

        //経過時間に合わせた割合を計算
        float time = (Time.time - StartTime) / Duration;

        //スムーズに角度を計算
        Angle = Mathf.SmoothStep(Angle, Direction * MaxAngle, time);

        //角度を変更
        transform.localEulerAngles = new Vector3(0f, 0f, Angle);

        //角度が指定した角度と1度の差になったら反転
        if (Mathf.Abs(Mathf.DeltaAngle(Angle, Direction * MaxAngle)) < 1.0f)
        {
            Direction *= -1;
            StartTime = Time.time;
        }
    }
    //進んでいる向きを返す(実際にはint値)
    public int GetDirection()
    {
        return Direction;
    }
}
