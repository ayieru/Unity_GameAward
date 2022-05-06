using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
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
    private Magnet_Pole currentPole;

    private float PlayerPosX;
    private float PlayerPosY;

    [SerializeField] LayerMask layer;

    Transform PlayerTransform;
    
    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        PlayerTransform = this.transform;// transformを取得

        currentPole = this.Pole;
        ChangeColor();

        PlayerPosX = this.transform.position.x;
        PlayerPosY = this.transform.position.y;
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

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, layer);
        if (hitInfo.collider)
        {
            Debug.Log("磁石にくっついた！！");
            HitJagde = true;
        }
        else
        {
            HitJagde = false;
        }

        if (HitJagde == true)
        {
            Rb.velocity = new Vector2(XSpeed, YSpeed * 1.4f);      // 壁のぼり
        }
        else
        {
            Rb.velocity = new Vector2(XSpeed, Rb.velocity.y);　// ジャンプ
        }

        if (currentPole != Pole) ChangeColor();
    }

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


                //HitJagde = true;
            }
        }

        if(collision.gameObject.CompareTag("Thorn"))
        {
            Vector2 worldPos = PlayerTransform.position;

            // セーブポイント通ったか
            if (SavePoint.instance != null)
            {
                // 通ったセーブポイントの座標に復活させる
                worldPos.x = SavePoint.instance.GetSavePointPosX();
                worldPos.y = SavePoint.instance.GetSavePointPosY();
            }
            // 通ってないなら初期座標に戻る
            else
            {
                worldPos.x = PlayerPosX;
                worldPos.y = PlayerPosY;
            }
           
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
    private void ChangeColor()
    {
        if (Pole == Magnet_Pole.N)
        {
            var colorCode = "#FF0000";
            if (ColorUtility.TryParseHtmlString(colorCode, out Color color))
                GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            var colorCode = "#0000FF";
            if (ColorUtility.TryParseHtmlString(colorCode, out Color color))
                GetComponent<SpriteRenderer>().color = color;
        }

        currentPole = Pole;
    }
}
