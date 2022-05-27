using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
    [System.Serializable]
    public class AudioClips
    {
        public float Volume = 0.0f;
        public AudioClip audioClips;
    }
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

    [Header("切り替えのSE")]
    public AudioClips ListAudioClips = new AudioClips();

    [Header("テンポを一定にするかどうか")]
    [SerializeField] bool RandomizePitch = true;

    [Header("テンポ数")]
    [SerializeField] float PitchRange = 0.1f;

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

    // 初期座標保存
    private float PlayerPosX;
    private float PlayerPosY;

    public float HorizontalKey { get; private set; }
    public float VerticalKey { get; private set; }

    private float SavePlayerRightPosX;  // 右側のX座標保存
    private float SavePlayerLeftPosX;   // 左側のX座標保存
    private float SavePlayerPosY;       // Y座標保存

    private MoveChain MoveChainObj;

    private CatchTheChain ChainObj;

    private PlayerAnimation PlayerAnim;

    private Quaternion PreRotation;//ロープを掴んでいる時のプレイヤーの角度格納用

    private Rigidbody2D Rb;

    private bool IsGround = false;      // 地面と触れているか
    private bool IsFootField = false;   // 足場と触れているか

    private bool HitJagde = false;// 何かとプレイヤーが当たった判定
    private bool TwoFlug = false;

    private bool NormalJump = false;//床、ブロック、鎖に触れてる時に行うジャンプ

    private bool WallJump = false;//鉄に触れてる時に行う壁ジャンプ

    private int MagnetHitCount = 0;//縦/横に2個以上並べたマグネットの判定に使う

    private State PlayerState = State.Normal;

    private float texX, texY;

    protected AudioSource Source;

    void Awake()
    {
        Source = GetComponent<AudioSource>();

        IsGround = false;

        IsFootField = false;

        NormalJump = false;

        Rb = GetComponent<Rigidbody2D>();

        PlayerAnim = GetComponent<PlayerAnimation>();

        PlayerState = State.Normal;

        DirectionX = (int)PlayerDirection.Right;

        MagnetHitCount = 0;

        texX = gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;
        texY = gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f;

        PlayerPosX = transform.position.x;
        PlayerPosY = transform.position.y;
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

            if (HitJagde)
            {
                // 縦入力反応処理
                if (VerticalKey > 0)
                {
                    YSpeed = Speed * 2.0f;

                    Rb.velocity = new Vector2(0.0f, Speed);
                }
                else if (VerticalKey < 0)
                {
                    Rb.velocity = new Vector2(0.0f, -Speed);
                }
                else
                {
                    if(!WallJump)
                    {
                        Rb.velocity = new Vector2(0.0f, 0.0f);
                    }
                }
            }
            else
            {
                //横入力反応処理
                if (HorizontalKey > 0)
                {
                    XSpeed = Speed;

                    Rb.velocity = new Vector2(Speed, Rb.velocity.y);

                    DirectionX = (int)PlayerDirection.Right;

                    localScale.x = 1.0f;

                    transform.localScale = localScale;
                }
                else if (HorizontalKey < 0)
                {
                    XSpeed = -Speed;

                    Rb.velocity = new Vector2(-Speed, Rb.velocity.y);

                    DirectionX = (int)PlayerDirection.Left;

                    localScale.x = -1.0f;

                    transform.localScale = localScale;
                }
                else
                {
                    if (!WallJump)
                    {
                      Rb.velocity = new Vector2(0.0f, Rb.velocity.y);
                    }
                }
            }

            //ジャンプ
            if (Input.GetButtonDown("Jump") && !(Rb.velocity.y < -0.5f))
            {
                if(HitJagde)//壁のぼり時
                {
                    Rb.AddForce((transform.up * 2.0f + (transform.right * (DirectionX * -1.0f))) * JumpPower * 0.5f, ForceMode2D.Impulse);

                    DirectionX = -DirectionX;

                    localScale.x = -localScale.x;

                    transform.localScale = localScale;

                    WallJump = true;
                }
                else
                {
                    // 地面か足場に触れていたら
                    if (IsGround || IsFootField)
                    {
                        Rb.AddForce(transform.up * JumpPower, ForceMode2D.Impulse);

                        NormalJump = true;
                    }
                }
            }

            //極切り替え
            if (Input.GetButtonDown("MagnetChange"))
            {
                if (Pole == Magnet_Pole.S)
                {
                    Pole = Magnet_Pole.N;
                    Debug.Log("極切り替え：S → N");

                    if (MagnetHitCount >= 1)//磁石の側面に触れている時用にも対応
                    {
                        PlayerAnim.MagnetChange(PlayerAnimation.AnimationLayer.Player_Red, "Attraction");

                        MagnetHitCount = 0;
                    }
                    else
                    {
                        PlayerAnim.MagnetChange(PlayerAnimation.AnimationLayer.Player_Red);
                    }
                }
                else
                {
                    Pole = Magnet_Pole.S;
                    Debug.Log("極切り替え：N → S");

                    if (MagnetHitCount >= 1)
                    {
                        PlayerAnim.MagnetChange(PlayerAnimation.AnimationLayer.Player_Blue, "Idle");

                        MagnetHitCount = 0;
                    }
                    else
                    {
                        PlayerAnim.MagnetChange(PlayerAnimation.AnimationLayer.Player_Blue);
                    }
                }

                AudioClip clips = ListAudioClips.audioClips;
                float SoundVolume = ListAudioClips.Volume;
                // テンポを毎回ランダムにして自然に近い感じにする
                if (RandomizePitch) Source.pitch = 1.0f + Random.Range(-PitchRange, PitchRange);
                Source.PlayOneShot(clips, SoundVolume);
            }
        }
        else if (PlayerState == State.CatchChain)
        {
            if (Input.GetButtonDown("Jump"))
            {
                SetPlayerState(State.ReleaseChain);

                Rb.simulated = true;

                PlayerAnim.MagnetChange(PlayerAnim.GetCurrentLayer(), "Jump");

                NormalJump = true;
            }
            if (transform.localPosition != ChainObj.GetArrivalPoint())
            {
                //滑らかに決められた位置に移動させる
                //transform.localPosition = Vector3.Lerp(transform.localPosition, ChainObj.GetArrivalPoint(), SpeedToRope * Time.deltaTime);
            }

            PlayerAnim.MagnetChange(PlayerAnim.GetCurrentLayer(), "Idle");

            NormalJump = false;

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

    public bool GetTwoFlug() { return TwoFlug; }

    public bool GetHitJagde() { return HitJagde; }

    public float GetDirectionX() { return DirectionX; }

    public float GetHorizontalKey() { return HorizontalKey; }

    public float GetVerticalKey() { return VerticalKey; }

    public int GetMagnetHitCount() { return MagnetHitCount; }

    public void SetMagnetHitCount(int count) { MagnetHitCount = count; }

    public void SetNormalJump(bool enable) { NormalJump = enable; }

    public bool GetNormalJump() { return NormalJump; }

    public void SetWallJump(bool enable) { WallJump = enable; }

    public bool GetWallJump() { return WallJump; }

    /// <summary>
    /// 地面or足場と触れているかの判定
    /// </summary>
    /// <returns></returns>
    public bool IsJump() { return (IsGround == true || IsFootField == true); }

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

    // 触れていたら常に実行させる
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 既に地面か足場に触れてある状態ならスルーする
        if (IsGround == true || IsFootField == true) return;

        if (collision.gameObject.CompareTag("Floor"))
        {
            IsGround = true;

            PlayerState = State.Normal;

            Debug.Log(IsGround);
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            // プレイヤーが触れていないブロックが動作しないようにする
            if (FootFieldBrock.instance == null) return;

            FootPos();

            PlayerState = State.Normal;

            // プレイヤーの高さが足場の高さ超えている　かつ　プレイヤーが足場に着地出来ているなら
            if (
                (SavePlayerPosY > FootFieldBrock.instance.GetSaveTopPosY()) &&
                ((SavePlayerLeftPosX > FootFieldBrock.instance.GetSaveLeftPosX()) || (SavePlayerRightPosX < FootFieldBrock.instance.GetSaveRightPosX()))
                )
            {
                // ジャンプ出来るようにする
                IsFootField = true;
            }
        }

        
    }

    // あたったタイミングで処理が動く
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //簡易的に鉄を実装
        if (collision.gameObject.CompareTag("Iron"))
        {
            Debug.Log("鉄にくっついた！！");

            Debug.Log(MagnetHitCount);

            //既にTure
            if (HitJagde)
            {
                TwoFlug = true;
            }

            HitJagde = true;

            Rb.gravityScale = 0.0f;

            NormalJump = false;

            WallJump = false;

            PlayerDirectionCorrection(transform.position.x, collision.gameObject.transform.position.x);
        }

        if (collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            // 磁石によって引き寄せられてるか
            if (collision.gameObject.GetComponent<Magnet>().Pole != Pole)
            {
                Debug.Log("磁石にくっついた！！");

                MagnetHitCount++;

                HitJagde = true;
            }

            PlayerDirectionCorrection(transform.position.x, collision.gameObject.transform.position.x);

            PlayerAnim.SetPlayerAnimationSpeed(1.0f);
        }

        

        if (collision.gameObject.CompareTag("Thorn"))
        {
            Vector2 worldPos = this.transform.position;

            // セーブポイント通ったか
            if (SavePoint.instance != null)
            {
                // 通ったセーブポイントの座標に復活させる
                worldPos = SavePoint.instance.GetSavePointPos().position;
            }
            // 通ってないなら初期座標に戻る
            else
            {
                worldPos.x = PlayerPosX;
                worldPos.y = PlayerPosY;
            }

            transform.position = worldPos;// 座標設定

            // とげに刺さったら、ジャンプの力を0にして浮かないようにする。
            Rb.velocity = new Vector2(0.0f, 0.0f);
        }

        if (collision.gameObject.CompareTag("Chain"))
        {
            NormalJump = false;

            WallJump = false;
        }
    }

    // 離れたら処理が動く
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            Debug.Log("離れた！！");

            MagnetHitCount = System.Math.Max(MagnetHitCount - 1, 0);

            Debug.Log(MagnetHitCount);

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
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            IsFootField = false;

            Debug.Log(IsFootField);
        }
    }

    public void PlayerDirectionCorrection(float posA, float posB)
    {
        if((posA - posB) >= 0.0f)
        {
            Vector3 localScale = transform.localScale;

            localScale.x = -1.0f;

            transform.localScale = localScale;

            Debug.Log("左向きに補正！！");
        }
        else
        {
            Vector3 localScale = transform.localScale;

            localScale.x = 1.0f;

            transform.localScale = localScale;

            Debug.Log("右向きに補正！！");
        }
    }

    // プレイヤーの足元の座標を求める
    private void FootPos()
    {
        /*-----------右側のX座標設定-----------*/
        float RightPosX;
        RightPosX = transform.position.x;

        // 自身のサイズ分座標をずらす
        RightPosX += texX;
        
        SavePlayerRightPosX = RightPosX;
        
        /*-----------左側のX座標設定-----------*/
        float LeftPosX;
        LeftPosX = transform.position.x;

        // 自身のサイズ分座標をずらす
        LeftPosX -= texX;
        
        SavePlayerLeftPosX = LeftPosX;

        /*-----------Y座標設定-----------*/
        float PosY;
        PosY = transform.position.y;

        // 自身のサイズ分座標をずらす
        PosY -= texY;

        SavePlayerPosY = PosY;
    }
}
