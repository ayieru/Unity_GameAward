using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
    [Header("極")]
    [SerializeField] Magnet_Pole Pole = Magnet_Pole.N;

    [Header("移動速度")]
    [SerializeField] private float Speed = 5.0f;

    [Header("ジャンプ力")]
    [SerializeField] private float JumpPower = 10.0f;

    private Rigidbody2D Rb;

    private bool HitJagde = false;// 何かとプレイヤーが当たった判定

    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        GameObject Iron = GameObject.FindWithTag("Iron");// Ironのタグがついているオブジェクトを取得
    }

    void Update()
    {
        float HorizontalKey = Input.GetAxis("Horizontal");
        float VerticalKey = Input.GetAxis("Vertical");  // 縦入力反応変数
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
            Rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
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

        // 磁石で引き寄せられているか、いないかの判定
        if (HitJagde == true)
        {
            Rb.velocity = new Vector2(XSpeed, YSpeed);
        }
        else
        {
            Rb.velocity = new Vector2(XSpeed, Rb.velocity.y);
        }
    }

    // あたったタイミングで処理が動く
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Iron"))
        {
            Debug.Log("くっついた！！");
            HitJagde = true;
        }
    }

    // 離れたら処理が動く
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Iron"))
        {
            Debug.Log("離れた！！");
            HitJagde = false;
        }
    }

    public Magnet_Pole GetPole() { return Pole; }
}
