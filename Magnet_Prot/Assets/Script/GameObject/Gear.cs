using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MagnetManager
{
    [Header("歯車オブジェクト(大)")]
    public GameObject BigGearObj;

    [Header("歯車オブジェクト(小)")]
    public GameObject SmallGearObj;

    [Header("歯車の回転速度")]
    public float RotSpeed;

    [Header("ドアの移動速度")]
    public float DoorSpeed = 0.01f;

    private bool stay = false;

    private GameObject PlayerObj;
    private Player player;
    private GameObject DoorObj;

    private Vector3 Doorpos;
    private Vector3 initPos;
    private float texY;

    private bool SeStop = false;// SEを止めるタイミング

    protected AudioSource Source;

    void Awake()
    {
        Source = GetComponent<AudioSource>();
        PlayerObj = GameObject.Find("Player");
        DoorObj = transform.Find("Door").gameObject;
        player = PlayerObj.GetComponent<Player>();

        Doorpos = DoorObj.transform.position;
        texY = DoorObj.GetComponent<SpriteRenderer>().bounds.size.y;
        initPos = DoorObj.transform.position;
        stay = false;
        SeStop = false;
    }

    private void Update()
    {
        //歯車が稼働している際の処理
        if(stay == true)
        {
            if (Doorpos.y <= initPos.y + texY)
            {
                SeStop = false;
                //ドアを開く
                Doorpos.y += DoorSpeed;
                DoorObj.transform.position = Doorpos;

                //歯車（大）
                BigGearObj.gameObject.transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime * RotSpeed, Space.World);

                //歯車（小）
                SmallGearObj.gameObject.transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime * RotSpeed, Space.World);

                GimmickSeSound.instance.SoundStart();
            }
            else if (Doorpos.y > initPos.y + texY) SeStop = true;
        }

        //歯車が停止した際の処理
        if (stay == false)
        {
            if (Doorpos.y >= initPos.y)
            {
                SeStop = false;
                //ドアを閉める
                Doorpos.y -= DoorSpeed;
                DoorObj.transform.position = Doorpos;

                //歯車（大）
                BigGearObj.gameObject.transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime * RotSpeed, Space.World);

                //歯車（小）
                SmallGearObj.gameObject.transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime * RotSpeed, Space.World);

                GimmickSeSound.instance.SoundStart();
            }
            else if (Doorpos.y < initPos.y - texY) SeStop = true;
        }

        if (SeStop == true) GimmickSeSound.instance.SoundStop();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("MagnetBlock"))
        {
            stay = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("MagnetBlock"))
        {
            stay = true;
        }
    }
}
