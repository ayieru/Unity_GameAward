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

    private string AnimationName = "Idle";

    private AnimationLayer LayerNumber;

    void Start()
    {
        PlayerAnim = GetComponent<Animator>();

        PlayerObj = GetComponent<Player>();

        LayerNumber = AnimationLayer.Player_Blue;

        PlayerAnim.Play(AnimationName, (int)LayerNumber);

        return;

        if (PlayerObj.GetPole() == Magnet.Magnet_Pole.N)
        {
            LayerNumber = AnimationLayer.Player_Red;
        }
        else if (PlayerObj.GetPole() == Magnet.Magnet_Pole.S)
        {
            LayerNumber = AnimationLayer.Player_Blue;
        }

        

        
    }


    void Update()
    {
        return;
        PlayerAnim.speed = 1;//再開

        if (PlayerObj.GetHitJagde())//登るときの状態
        {
            PlayerAnim.Play("Climbing");

             if (PlayerObj.GetVerticalKey() == 0)
            {
                PlayerAnim.speed = 0;//停止
            }
            else if (PlayerObj.GetHorizontalKey() > 0 || PlayerObj.GetHorizontalKey() < 0)
            {
                PlayerAnim.Play("PlayerWalk_Red");
            }
        }
        else//普通に移動している時の状態
        {
            if (PlayerObj.GetHorizontalKey() > 0 || PlayerObj.GetHorizontalKey() < 0)
            {
                PlayerAnim.Play("PlayerWalk_Red");
            }
            else
            {
                PlayerAnim.Play("PlayerIdle_Red");
            }
        }
    }

    public void JumpAction()
    {
        PlayerAnim.Play("Jump");
        PlayerAnim.Update(0);
        StateInfo = PlayerAnim.GetCurrentAnimatorStateInfo(0);

        Debug.Log(StateInfo.length);
    }

    /// <summary>
    /// プレイヤーの極切り替えの際のアニメーション遷移
    /// </summary>
    /// <param name="animationName">再生するアニメーション名</param>
    /// <param name="nextLayer">切り替え後のレイヤー</param>
    public void MagnetChange(AnimationLayer nextLayer)
    {
        StateInfo = PlayerAnim.GetCurrentAnimatorStateInfo(0);//切り替え前のアニメーション情報の取得

        //現在の再生時間を取得
        float CurrentAnimFrame = StateInfo.normalizedTime;
        //Debug.Log(CurrentAnimFrame);

        PlayerAnim.SetLayerWeight((int)LayerNumber, 0);//元のレイヤー

        PlayerAnim.SetLayerWeight((int)nextLayer, 1);//次のレイヤー

        LayerNumber = nextLayer;

        PlayerAnim.Play("Idle", 0, CurrentAnimFrame);
    }

    public void SetPlayerAnimation(string animationName, int animationLayer)
    {

    }

    public void SetPlayerAnimation(string animationName)
    {

    }



    /// <summary>
    /// unity側でジャンプ再生が終わったら呼び出す関数として使用する
    /// </summary>
    public void JumpFinish()
    {
        PlayerAnim.Play("PlayerIdle_Red");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //簡易的に鉄を実装
        if (collision.gameObject.CompareTag("Iron"))
        {
            PlayerAnim.SetTrigger("Climbing");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Iron"))
        {

        }
    }
}
