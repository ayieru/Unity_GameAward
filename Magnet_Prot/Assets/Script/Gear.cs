using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MagnetManager
{

    [Header("対応している極")]
    public Magnet_Pole Pole = Magnet_Pole.N;

    [Header("歯車オブジェクト(大)")]
    public GameObject BigGearObj;

    [Header("歯車オブジェクト(小)")]
    public GameObject SmallGearObj;

    [Header("ドアオブジェクト")]
    public GameObject DoorObj;

    [Header("歯車の回転速度")]
    public float RotSpeed;

    [Header("ドアの移動速度")]
    public float DoorSpeed = 0.01f;

    [Header("発電量の最大値")]
    public int MaxPower = 20;

    [Header("発電量")]
    private int Power = 0;

    [Header("プレイヤーが範囲内にいるか")]
    private bool PlayerStay = false;

    private GameObject PlayerObj;
    private Rigidbody2D Rb;
    private Player player;

    void Awake()
    {
        PlayerObj = GameObject.Find("Player");
        Rb = PlayerObj.GetComponent<Rigidbody2D>();
        player = PlayerObj.GetComponent<Player>();

        Power = 0;
        PlayerStay = false;
    }

    private void Update()
    {
        //歯車が稼働している際の処理
        if(PlayerStay == true && Pole == player.GetPole())
        {
            //最大値まで発電する
            if (Power < MaxPower)
            {
                Power += 1;
            }

            //範囲内にいるときに回転

            //歯車（大）
            BigGearObj.gameObject.transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime * RotSpeed, Space.World);

            //歯車（小）
            SmallGearObj.gameObject.transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime * RotSpeed, Space.World);

            //ドアを開く
            Transform myTransform = DoorObj.transform;

            // 座標を取得
            Vector3 pos = myTransform.position;
            pos.y += DoorSpeed;

            if (pos.y > 0.0f)
            {
                pos.y = 0.0f;
            }

            myTransform.position = pos;

        }

        //歯車が停止した際の処理
        if (PlayerStay == false && Pole == player.GetPole())
        {
            //ドアを閉じる
            Transform myTransform = DoorObj.transform;

            // 座標を取得
            Vector3 pos = myTransform.position;
            pos.y -= DoorSpeed;

            if (pos.y < -2.5f)
            {
                pos.y = -2.5f;
            }

            myTransform.position = pos;  //座標を設定

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Pole == player.GetPole())
        {
            //プレイヤーが入った
            PlayerStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Pole == player.GetPole())
        {
            //プレイヤーが出た
            PlayerStay = false;
        }

    }
}
