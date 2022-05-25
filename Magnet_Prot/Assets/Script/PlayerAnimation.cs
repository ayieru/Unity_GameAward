using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum AnimationLayer
    {
        Player_Red = 0,//万が一の為に一応値を設定
        Player_Blue = 1,
    }


    private Animator PlayerAnim;

    private AnimatorStateInfo StateInfo;

    private Player PlayerObj;

    private AnimationLayer CurrentLayer = AnimationLayer.Player_Red;

    private bool Action = false;

    private bool Climbing = false;

    void Start()
    {
        PlayerAnim = GetComponent<Animator>();

        PlayerObj = GetComponent<Player>();

        CurrentLayer = AnimationLayer.Player_Red;

        Action = false;

        Climbing = false;

        if (PlayerObj.GetPole() == Magnet.Magnet_Pole.N)
        {
            MagnetChange(AnimationLayer.Player_Red);
        }
        else if (PlayerObj.GetPole() == Magnet.Magnet_Pole.S)
        {
            MagnetChange(AnimationLayer.Player_Blue);
        }

        PlayerAnim.Play("Idle", (int)CurrentLayer);
    }


    void Update()
    {
        PlayerAnim.speed = 1.0f;

        if (PlayerObj.GetHitJagde())//登るときの状態
        {
            PlayerAnim.Play("Climbing", (int)CurrentLayer);

            if (PlayerObj.GetVerticalKey() == 0)
            {
                PlayerAnim.speed = 0;//停止
            }
            else
            {
                PlayerAnim.speed = 1;//再開
            }
        }
        else
        {
            if (PlayerObj.GetHorizontalKey() > 0 || PlayerObj.GetHorizontalKey() < 0)
            {
                if (Action)
                {
                    PlayerAnim.Play("WalkJump", (int)CurrentLayer);
                    return;
                }

                PlayerAnim.Play("Walk", (int)CurrentLayer);
            }
            else
            {
                if (Action)
                {
                    if (PlayerObj.GetMagnetHitCount() <= 0)
                    {
                        PlayerAnim.Play("IdleJump", (int)CurrentLayer);

                        return;
                    }
                }

                if(PlayerObj.GetMagnetHitCount() <= 0)
                {
                    PlayerAnim.Play("Idle", (int)CurrentLayer);
                }
            }
        }
    }

    public void SetAction(bool enable) { Action = enable; }

    public bool GetAction() { return Action; }

    /// <summary>
    /// プレイヤーの極切り替えの際のアニメーションレイヤー変更
    /// </summary>
    /// <param name="nextLayer">切り替え後のレイヤー</param>
    public void MagnetChange(AnimationLayer nextLayer)
    {
        if (CurrentLayer == nextLayer)
        {
            return;
        }

        PlayerAnim.SetLayerWeight((int)CurrentLayer, 0.0f);//現在のレイヤー

        PlayerAnim.SetLayerWeight((int)nextLayer, 1.0f);//次のレイヤー

        CurrentLayer = nextLayer;
    }

    /// <summary>
    /// プレイヤーの極切り替えの際のアニメーションレイヤー変更
    /// </summary>
    /// <param name="nextLayer">切り替え後のレイヤー</param>
    /// <param name="nextAnimationName">切り替え先のアニメーション名</param>
    public void MagnetChange(AnimationLayer nextLayer, string nextAnimationName)
    {
        MagnetChange(nextLayer);

        PlayerAnim.Play(nextAnimationName, (int)nextLayer);
    }

    /// <summary>
    /// unity側でジャンプ再生が終わったら呼び出す関数として使用する
    /// </summary>
    public void JumpFinish()
    {
        Action = false;
        //PlayerAnim.Play("Idle", (int)CurrentLayer);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //簡易的に鉄を実装
        if (collision.gameObject.CompareTag("Iron") ||
            collision.gameObject.CompareTag("NPole") ||
            collision.gameObject.CompareTag("SPole"))
        {
            //触れたオブジェクトと自身の位置を計算して向きを補正させる
            if ((gameObject.transform.position.x - collision.gameObject.transform.position.x) >= 0.0f)
            {
                Vector3 localScale = PlayerObj.transform.localScale;

                localScale.x = -1.0f;

                transform.localScale = localScale;

                Debug.Log("左向きに補正！！");
            }
            else
            {
                Vector3 localScale = PlayerObj.transform.localScale;

                localScale.x = 1.0f;

                transform.localScale = localScale;

                Debug.Log("右向きに補正！！");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //簡易的に鉄を実装
        if (collision.gameObject.CompareTag("Iron") ||
            collision.gameObject.CompareTag("NPole") ||
            collision.gameObject.CompareTag("SPole"))
        {
            PlayerAnim.Play("Climbing", (int)CurrentLayer);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            if (PlayerObj.GetMagnetHitCount() == 0)
            {
                PlayerAnim.Play("Idle", (int)CurrentLayer);
            }
        }
    }
}
