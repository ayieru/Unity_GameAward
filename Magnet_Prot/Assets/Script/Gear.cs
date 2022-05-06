using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MagnetManager
{
    [Header("歯車オブジェクト(大)")]
    public GameObject BigGearObj;

    [Header("歯車オブジェクト(小)")]
    public GameObject SmallGearObj;

    [Header("回転速度")]
    public float RotSpeed;

    [Header("発電量の最大値")]
    public int MaxPower = 20;

    [Header("発電量")]
    private int Power = 0;

    [Header("プレイヤーが範囲内にいるか")]
    private bool PlayerStay = false;

    private GameObject PlayerObj;
    private Player player;

    void Awake()
    {
        PlayerObj = GameObject.Find("Player");
        player = PlayerObj.GetComponent<Player>();

        Power = 0;
        PlayerStay = false;
    }

    private void Update()
    {
        if (PlayerStay)
        {
            if (Pole == player.GetPole())
            {
                //最大値まで発電する
                if (Power < MaxPower)
                {
                    Power += 1;
                }
                else
                {
                    //溜まったら止める　確認用にコメントアウト
                    //return;
                }

                //歯車（大）
                BigGearObj.gameObject.transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime * RotSpeed, Space.World);

                //歯車（小）
                SmallGearObj.gameObject.transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime * RotSpeed, Space.World);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーが入った
            PlayerStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーが出た
            PlayerStay = false;
        }

    }
}
