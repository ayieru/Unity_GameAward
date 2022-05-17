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
        if(PlayerObj.HorizontalKey > 0)
        {
            PlayerAnim.SetTrigger("Walk");
        }
        else if(PlayerObj.HorizontalKey < 0)
        {
            PlayerAnim.SetTrigger("Walk");
        }
        else
        {
            PlayerAnim.SetTrigger("Idle");
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
