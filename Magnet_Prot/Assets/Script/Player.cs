using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
    [Header("極")]
    [SerializeField] Magnet_Pole Pole = Magnet_Pole.N;

    [Header("移動速度")]
    [SerializeField] float Speed = 5.0f;

    [Header("ジャンプ力")]
    [SerializeField] float JumpPower = 10.0f;

    [Header("ジャンプ回数の上限")]
    [SerializeField] int MaxJumpCount = 1;

    private int JumpCount = 0;

    private Rigidbody2D Rb;

    private bool HitJagde = false;// 何かとプレイヤーが当たった判定
    private bool TwoFlug = false;

    Transform PlayerTransform;
    
    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        PlayerTransform = this.transform;// transformを取得
    }

    void Update()
    {
        float HorizontalKey = Input.GetAxisRaw("Horizontal");
        float VerticalKey = Input.GetAxisRaw("Vertical");  // 縦入力反応変数
        float XSpeed = 0.0f;
        float YSpeed = 0.0f;                            // 縦移動のスピード変数

        if (HorizontalKey > 0)
        {
            XSpeed = Speed;
        }
        else if (HorizontalKey < 0)
        {
            XSpeed = -Speed;
        }
        else
        {
            XSpeed = 0.0f;
        }

        // 縦入力反応処理
        if (VerticalKey > 0)
        {
            YSpeed = Speed * 2.0f;
        }
        else if (VerticalKey < 0)
        {
            YSpeed = -Speed * 2.0f;
        }
        else
        {
            YSpeed = 0.0f;
        }

        //ジャンプ
        if (Input.GetButtonDown("Jump") && !(Rb.velocity.y < -0.5f))
        {
            if (JumpCount < MaxJumpCount)
            {
                Rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);

                JumpCount++;
            }
        }

        //アクション
        if (Input.GetButtonDown("Action"))
        {
            Debug.Log("アクション");
        }

        //極切り替え
        if (Input.GetButtonDown("MagnetChange"))
        {
            if (Pole == Magnet_Pole.S)
            {
                Pole = Magnet_Pole.N;
                Debug.Log("極切り替え：S → N");
            }
            else
            {
                Pole = Magnet_Pole.S;
                Debug.Log("極切り替え：N → S");
            }
        }

        if (HitJagde == true)
        {
            Rb.velocity = new Vector2(XSpeed, YSpeed);      // 壁のぼり
        }
        else
        {
            Rb.velocity = new Vector2(XSpeed, Rb.velocity.y);　// ジャンプ
        }
    }

    // あたったタイミングで処理が動く
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //簡易的に鉄を実装
        if (collision.gameObject.CompareTag("Iron"))
        {
            Debug.Log("鉄にくっついた！！");

            //既にTure
            if (HitJagde)
            {
                TwoFlug = true;
            }

            HitJagde = true;

            Rb.gravityScale = 0.0f;
        }

        if (collision.gameObject.CompareTag("NPole")|| collision.gameObject.CompareTag("SPole"))
        {
            // 磁石によって引き寄せられてるか
            if (collision.gameObject.GetComponent<Magnet>().Pole != Pole)
            {
                Debug.Log("磁石にくっついた！！");

                HitJagde = true;
            }
        }

        if(collision.gameObject.CompareTag("Thorn"))
        {
            Vector2 worldPos = PlayerTransform.position;

            // 通ったセーブポイントの座標に復活させる
            worldPos.x = SavePoint.instance.GetSavePointPosX();
            worldPos.y = SavePoint.instance.GetSavePointPosY();

            PlayerTransform.position = worldPos;// 座標設定
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            JumpCount = 0;
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            JumpCount = 0;
        }
    }

    // 離れたら処理が動く
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            Debug.Log("離れた！！");
            HitJagde = false;

            Rb.gravityScale = 1.0f;
        }

        //簡易的に鉄を実装
        if (collision.gameObject.CompareTag("Iron"))
        {
            Debug.Log("離れた！！");

            if (!TwoFlug)
            {
                HitJagde = false;
                Rb.gravityScale = 1.0f;
            }
            else
            {
                TwoFlug = false;
            }
        }
    }

    public Magnet_Pole GetPole() { return Pole; }
}
