using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
    public enum State
    {
        Normal,//通常状態
        CatchChain,//鎖に捕まってる状態
        ReleaseChain,//鎖を離した状態
    }
    public enum PlayerDirection
    {
        Right = 1,//右向き
        Left = -1,//左向き
    }

    [Header("極")]
    [SerializeField] Magnet_Pole Pole = Magnet_Pole.N;

    [Header("移動速度")]
    [SerializeField] float Speed = 5.0f;

    [Header("ジャンプ力")]
    [SerializeField] float JumpPower = 10.0f;

    [Header("進んでいる向き")]
    [SerializeField] private float DirectionX;

    /*鎖関連の変数*/
    [Header("鎖の所定の位置までのスピード")]
    [SerializeField] private float SpeedToRope = 5.0f;

    [Header("鎖を離したときにその向きに加える力")]
    [SerializeField]
    private float ReleasePower = 2.0f;

    [Header("鎖を離した時の力を減衰させる時間")]
    [SerializeField] private float DampingTime = 2.0f;

    public float HorizontalKey { get; private set; }
    public float VerticalKey { get; private set; }

    private MoveChain MoveChainObj;

    private CatchTheChain ChainObj;

    private Quaternion PreRotation;//ロープを掴んでいる時のプレイヤーの角度格納用

    private bool IsGround = false;// 地面と触れているか
    private bool IsFootField = false;// 足場と触れているか

    private Rigidbody2D Rb;

    private bool HitJagde = false;// 何かとプレイヤーが当たった判定
    private bool TwoFlug = false;

    private State PlayerState = State.Normal;

    //初期座標
    private float PlayerPosX;
    private float PlayerPosY;

    void Awake()
    {
        IsGround = false;
        IsFootField = false;

        Rb = GetComponent<Rigidbody2D>();

        PlayerState = State.Normal;

        DirectionX = (int)PlayerDirection.Right;

        PlayerPosX = this.transform.position.x;
        PlayerPosY = this.transform.position.y;

    }

    void Update()
    {
        Vector3 localScale = transform.localScale;

        if (PlayerState == State.Normal)
        {
            HorizontalKey = Input.GetAxisRaw("Horizontal");
            VerticalKey = Input.GetAxisRaw("Vertical");  // 縦入力反応変数
            float XSpeed = 0.0f;
            float YSpeed = 0.0f;                            // 縦移動のスピード変数

            //横入力反応処理
            if (HorizontalKey > 0)
            {
                XSpeed = Speed;

                DirectionX = (int)PlayerDirection.Right;

                localScale.x = 1.0f;

                transform.localScale = localScale;
            }
            else if (HorizontalKey < 0)
            {
                XSpeed = -Speed;

                DirectionX = (int)PlayerDirection.Left;

                localScale.x = -1.0f;

                transform.localScale = localScale;
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
                // 地面か足場に触れていたら
                if (IsGround||IsFootField)
                {
                    Rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
                }
            }

            Rb.velocity = new Vector2(XSpeed, Rb.velocity.y);

            if (HitJagde == true)// 壁のぼりの処理
            {
                Rb.velocity = new Vector2(XSpeed, YSpeed);
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
        }
        else if (PlayerState == State.CatchChain)
        {
            if (Input.GetButtonDown("Jump"))
            {
                SetPlayerState(State.ReleaseChain);

                Rb.simulated = true;
            }
            if (transform.localPosition != ChainObj.GetArrivalPoint())
            {
                //滑らかに決められた位置に移動させる
                transform.localPosition = Vector3.Lerp(transform.localPosition, ChainObj.GetArrivalPoint(), SpeedToRope * Time.deltaTime);
            }
        }
        else if (PlayerState == State.ReleaseChain)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, PreRotation, SpeedToRope * Time.deltaTime);

            //ロープの動いている速度を取得
            Vector3 velocityXZ = (MoveChainObj.transform.right * DirectionX * ReleasePower);

            //Y軸方向は重力に任せる為0にする
            velocityXZ.y = 0.0f;

            //ロープを離した時のロープが動いている速度と重力を足して全体の速度を計算
            Rb.velocity = velocityXZ + new Vector3(0.0f, Rb.velocity.y, 0.0f);

            //移動値を減少させる
            DirectionX = Mathf.Lerp(DirectionX, 0.0f, DampingTime * Time.deltaTime);

            //重力を働かせる
            Rb.velocity = new Vector3(
                 Rb.velocity.x,
                 Rb.velocity.y + Physics.gravity.y * Time.deltaTime,
                0.0f
                );
        }
    }

    public Magnet_Pole GetPole() { return Pole; }

    public State GetPlayerState() { return PlayerState; }

    public float GetDirectionX() { return DirectionX; }

    public void SetPlayerState(State state, CatchTheChain catchTheChain = null)
    {
        PlayerState = state;

        if (PlayerState == State.CatchChain)
        {
            //現在の角度を保持しておく
            PreRotation = transform.rotation;

            Rb.velocity = Vector3.zero;

            //移動値等の初期化
            float rot = transform.localEulerAngles.y;

            //角度を設定し直す
            transform.localRotation = Quaternion.Euler(0.0f, rot, 0.0f);

            SetCatchTheChain(catchTheChain);
        }
        else if (PlayerState == State.ReleaseChain)
        {
            transform.SetParent(null);

            Rb.SetRotation(0.0f);

            transform.Rotate(0.0f, 0.0f, 0.0f);

            //　ロープを離した時の向きを保持
            if (MoveChainObj.GetChainDirection() == 1)
            {
                DirectionX = (float)PlayerDirection.Right;
            }
            else
            {
                DirectionX = (float)PlayerDirection.Left;
            }

            MoveChainObj.SetMoveFlag(false);

        }
        else if (state == State.Normal)
        {
            ChainObj = null;
            transform.rotation = PreRotation;
        }
    }
    public void SetCatchTheChain(CatchTheChain chainBase)
    {
        //CatchTheChainとChainスクリプトの取得
        this.ChainObj = chainBase;
        MoveChainObj = chainBase.GetComponent<MoveChain>();
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

        if (collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            // 磁石によって引き寄せられてるか
            if (collision.gameObject.GetComponent<Magnet>().Pole != Pole)
            {
                Debug.Log("磁石にくっついた！！");

                HitJagde = true;
            }
        }

        if (collision.gameObject.CompareTag("Thorn"))
        {
            Vector2 worldPos = transform.position;

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

            transform.position = worldPos;// 座標設定
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            IsGround = true;

            Debug.Log(IsGround);

            SetPlayerState(State.Normal);
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            IsFootField = true;

            Debug.Log(IsFootField);

            SetPlayerState(State.Normal);
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

        if (collision.gameObject.CompareTag("Floor"))
        {
            IsGround = false;

            SetPlayerState(State.Normal);
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            IsFootField = false;

            SetPlayerState(State.Normal);
        }
    }
}
