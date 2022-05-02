using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChain : MonoBehaviour
{
    [Header("振り子の往復間隔(値が大きい程ゆっくり動く)")]
    [SerializeField]
    private float Duration = 5.0f;

    [Header("Z軸で振り子をする角度(初期の限界値)")]
    [SerializeField]
    private float MaxAngle = 90.0f;

    [Header("ロープを元の位置に戻すスピード")]
    [SerializeField]
    private float UndoSpeed = 2.0f;

    [Header("ロープが動いているか")]
    [SerializeField]
    private bool MoveFlag = false;

    private float DefaultMaxAngle;

    //進んでいる方向
    private int ChainDirection = 1;

    private int UndoDirection;

    //Z軸の角度
    private float Angle = 0.0f;

    //動き始める時の時間
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
        
        if (MoveFlag)//ロープを掴んでいる時のロープの動き
        {
            //経過時間に合わせた割合を計算
            float t = (Time.time - StartTime) / Duration;

            //　スムーズに角度を計算
            Angle = Mathf.SmoothStep(Angle, ChainDirection * MaxAngle, t);

            //角度を変更
            transform.localEulerAngles = new Vector3(0f, 0f, Angle);

            //角度が指定した角度と1度の差になったら反転
            if (Mathf.Abs(Mathf.DeltaAngle(Angle, ChainDirection * MaxAngle)) < 1f)
            {
                ChainDirection *= -1;
                StartTime = Time.time;
            }
            
        }
        else//ロープを離している時
        {
            //初期の角度になるまで振り子を繰り返す
            if (transform.localEulerAngles.z != 0)
            {

                //　徐々に限界角度を小さくする
                if (MaxAngle > 0f)
                {
                    MaxAngle -= UndoSpeed * Time.deltaTime;
                }

                //　経過時間に合わせた割合を計算
                float t = (Time.time - StartTime) / Duration;

                //　スムーズに角度を計算
                Angle = Mathf.SmoothStep(Angle, UndoDirection * MaxAngle, t);

                //　角度を変更
                transform.localEulerAngles = new Vector3(0f, 0f, Angle);

                //　角度が指定した角度と1度の差になったら反転
                if (Mathf.Abs(Mathf.DeltaAngle(Angle, UndoDirection * MaxAngle)) < 1f)
                {
                    UndoDirection *= -1;
                    StartTime = Time.time;
                }
            }
        }
    }
    //進んでいる向きを返す
    public int GetChainDirection(){ return ChainDirection; }

    public void SetChainDirection(int dir) { ChainDirection = dir; }

    public void SetMoveFlag(bool enable)
    {
        MoveFlag = enable;
        MaxAngle = DefaultMaxAngle;
        
        if (!MoveFlag)//ロープを離した時
        {
            //ロープを離した時用の方向値にdirectionを入れる
            UndoDirection = ChainDirection;
            
        }
        else//ロープを掴んだ時
        {
            StartTime = Time.time;
        }
    }

    public bool GetMoveFlag(){ return MoveFlag; }
}
