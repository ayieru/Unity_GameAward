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

        if(PlayerObj.GetPole() == Magnet.Magnet_Pole.N)
        {
            PlayerAnim.SetBool("MagnetSwitch", true);
        }
        else if(PlayerObj.GetPole() == Magnet.Magnet_Pole.S)
        {
            PlayerAnim.SetBool("MagnetSwitch", false);
        }

        PlayerAnim.SetTrigger("Idle");
    }

    
    void Update()
    {
        if(PlayerObj.GetHitJagde())
        {
            if (PlayerObj.VerticalKey != 0)
            {
                PlayerAnim.speed = 1;//再開
            }
            else if(PlayerObj.VerticalKey == 0)
            {
                PlayerAnim.speed = 0;//停止
            }
            else if (PlayerObj.HorizontalKey > 0 || PlayerObj.HorizontalKey < 0)
            {
                PlayerAnim.speed = 1;//再開
                PlayerAnim.SetTrigger("Walk");
            }
            else if(PlayerObj.HorizontalKey == 0)
            {
                PlayerAnim.speed = 1;//再開
                PlayerAnim.SetTrigger("Idle");
            }
        }
        else
        {
            PlayerAnim.speed = 1;//再開

            if (PlayerObj.HorizontalKey > 0 || PlayerObj.HorizontalKey < 0)
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
}
