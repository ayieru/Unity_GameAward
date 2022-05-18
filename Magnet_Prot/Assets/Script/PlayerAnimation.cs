using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator PlayerAnim;

    Player PlayerObj;

    void Start()
    {
        PlayerAnim = GetComponent<Animator>();

        PlayerObj = GetComponent<Player>();

        if (PlayerObj.GetPole() == Magnet.Magnet_Pole.N)
        {
            PlayerAnim.SetBool("MagnetSwitch", true);
        }
        else if (PlayerObj.GetPole() == Magnet.Magnet_Pole.S)
        {
            PlayerAnim.SetBool("MagnetSwitch", false);
        }

        PlayerAnim.SetTrigger("Idle");
    }


    void Update()
    {
        if (PlayerObj.GetHitJagde())
        {
            Debug.Log("壁のぼりの動き中");
            //PlayerAnim.speed = 1;//再開
            PlayerAnim.SetTrigger("Climbing");

            if (PlayerObj.GetVerticalKey() != 0)
            {

            }
            else if (PlayerObj.GetVerticalKey() == 0)
            {
                //PlayerAnim.speed = 0;//停止
            }
            else if (PlayerObj.GetHorizontalKey() > 0 || PlayerObj.GetHorizontalKey() < 0)
            {
                //PlayerAnim.SetTrigger("Walk");
            }
            else if (PlayerObj.GetHorizontalKey() == 0)
            {
                //PlayerAnim.SetTrigger("Idle");
            }
        }
        else
        {
            //PlayerAnim.speed = 1;//再開

            Debug.Log("歩行中");

            if (PlayerObj.GetHorizontalKey() > 0 || PlayerObj.GetHorizontalKey() < 0)
            {
                PlayerAnim.SetTrigger("Walk");
            }
            else
            {
                PlayerAnim.SetTrigger("Idle");
            }
        }

        if (PlayerObj.GetPole() == Magnet.Magnet_Pole.N)
        {
            PlayerAnim.SetBool("MagnetSwitch", true);
        }
        else if (PlayerObj.GetPole() == Magnet.Magnet_Pole.S)
        {
            PlayerAnim.SetBool("MagnetSwitch", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //簡易的に鉄を実装
        if (collision.gameObject.CompareTag("Iron"))
        {
            PlayerAnim.SetTrigger("Climbing");
        }
    }
}
