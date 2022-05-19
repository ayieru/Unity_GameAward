﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator PlayerAnim;

    AnimatorStateInfo StateInfo;

    Player PlayerObj;

    void Start()
    {
        PlayerAnim = GetComponent<Animator>();

        PlayerObj = GetComponent<Player>();

        if (PlayerObj.GetPole() == Magnet.Magnet_Pole.N)
        {
            PlayerAnim.SetBool("MagnetChange", true);
        }
        else if (PlayerObj.GetPole() == Magnet.Magnet_Pole.S)
        {
            PlayerAnim.SetBool("MagnetChange", false);
        }

        PlayerAnim.SetTrigger("Idle");
    }


    void Update()
    {
        PlayerAnim.speed = 1;//再開

        if (PlayerObj.GetHitJagde())//登るときの状態
        {
            PlayerAnim.SetTrigger("Climbing");

             if (PlayerObj.GetVerticalKey() == 0)
            {
                PlayerAnim.speed = 0;//停止
            }
            else if (PlayerObj.GetHorizontalKey() > 0 || PlayerObj.GetHorizontalKey() < 0)
            {
                PlayerAnim.SetTrigger("Walk");
            }
        }
        else//普通に移動している時の状態
        {
            if (PlayerObj.GetHorizontalKey() > 0 || PlayerObj.GetHorizontalKey() < 0)
            {
                PlayerAnim.SetTrigger("Walk");
            }
            else
            {
                PlayerAnim.SetTrigger("Idle");
            }
        }
        
        //if (MotionCounter >= 0.0f)
        //{
        //    //ジャンプアニメーションが再生し終えたらIdle or Walkに戻す
        //    MotionCounter += Time.deltaTime;
        //
        //    Debug.Log(MotionCounter);
        //
        //    if (MotionCounter >= StateInfo.length)
        //    {
        //        MotionCounter = -1.0f;
        //
        //        
        //    }
        //}

        //切り替え操作されました
        //現在の再生時間を保存
        //切り替え
        //保存した再生時間を代入
        //もし６秒の所で切り替えが押されていたら、切り替え後には６秒後からスタート
    }

    public void JumpAction()
    {
        PlayerAnim.SetBool("Jump", true);
        PlayerAnim.Update(0);
        StateInfo = PlayerAnim.GetCurrentAnimatorStateInfo(0);

        Debug.Log(StateInfo.length);
    }
    
    /// <summary>
    /// プレイヤーの極切り替えの際のアニメーション遷移
    /// </summary>
    /// <param name="enable">true/N極(赤)、false/S極(青)</param>
    public void MagnetChange(bool enable)
    {
        //現在の再生時間を取得
        float CurrentAnimFrame = StateInfo.length * StateInfo.normalizedTime;

        Debug.Log(CurrentAnimFrame);

        PlayerAnim.Update(CurrentAnimFrame);

        PlayerAnim.SetBool("MagnetChange", enable);
    }

 
    /// <summary>
    /// unity側でジャンプ再生が終わったら呼び出す関数として使用する
    /// </summary>
    public void JumpFinish()
    {
        PlayerAnim.SetBool("Jump", false);
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

/*
 *ジャンプに関して
 * 
 *プレイヤースクリプトの方でJumpAction関数を呼び、
 * 関数内でJumpアニメーションに切り替える方法を採用しました。
 * 
 */
