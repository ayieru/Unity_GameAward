using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
    [System.Serializable]
    public class AudioClips
    {
        public float Volume = 0.0f;
        public AudioClip[] audioClips;
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
    [SerializeField] List<AudioClips> ListAudioClips = new List<AudioClips>();

    [Header("極")]
    [SerializeField] Magnet_Pole Pole = Magnet_Pole.N;

    [Header("移動速度")]
    [SerializeField] float Speed = 5.0f;

    [Header("通常ジャンプのパワー")]
    [SerializeField] float NormalJumpPower = 10.0f;

    [Header("壁ジャンプのパワー")]
    [SerializeField] float WallJumpPower = 10.0f;

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
        //ポーズ画面でキャラが動かないようにするために必要な処理です
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }

        if (PlayerAnim.GetResultGameClear() || PlayerAnim.GetGameOver())
        {
            return;
        }

        Vector3 localScale = transform.localScale;

        if (PlayerState == State.Normal)
        {
            HorizontalKey = Input.GetAxisRaw("Horizontal");
            VerticalKey = Input.GetAxisRaw("Vertical");  // 縦入力反応変数

            if (HitJagde || MagnetHitCount >= 1)
            {
                // 縦入力反応処理
                if (VerticalKey > 0)
                {
                    Rb.velocity = new Vector2(0.0f, Speed);
                }
                else if (VerticalKey < 0)
                {
                    Rb.velocity = new Vector2(0.0f, -Speed);
                }
                else
                {
                    if (!WallJump)
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
                    Rb.velocity = new Vector2(Speed, Rb.velocity.y);

                    DirectionX = (int)PlayerDirection.Right;

                    localScale.x = 1.0f;

                    transform.localScale = localScale;
                }
                else if (HorizontalKey < 0)
                {
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
            if (Input.GetButtonDown("Jump"))// && !(Rb.velocity.y < -0.5f))
            {
                if (HitJagde && MagnetHitCount <= 0)//壁のぼり時
                {
                    Rb.velocity = new Vector2(0.0f, 0.0f);//Wキーが押されていてもかかる力を０にしてジャンプのパワーを固定にする

                    Rb.AddForce((transform.up * 1.5f + (transform.right * (DirectionX * -1.0f))) * WallJumpPower, ForceMode2D.Impulse);

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
                        AudioClip[] clips = ListAudioClips[1].audioClips;
                        float SoundVolume = ListAudioClips[1].Volume;
                        Source.PlayOneShot(clips[0], SoundVolume);

                        Rb.AddForce(transform.up * NormalJumpPower, ForceMode2D.Impulse);

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

                    PlayerAnim.MagnetChange(PlayerAnimation.AnimationLayer.Player_Red);
                }
                else
                {
                    Pole = Magnet_Pole.S;

                    PlayerAnim.MagnetChange(PlayerAnimation.AnimationLayer.Player_Blue);
                }

                AudioClip[] clips = ListAudioClips[0].audioClips;
                float SoundVolume = ListAudioClips[0].Volume;
                Source.PlayOneShot(clips[0], SoundVolume);
            }
        }
        else if (PlayerState == State.CatchChain)
        {
            if (Input.GetButtonDown("Jump"))
            {
                SetPlayerState(State.ReleaseChain);
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
    public bool IsJump() { return IsGround == true || IsFootField == true; }

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

            //アニメーションの切り替え
            HitJagde = true;

            NormalJump = false;

            WallJump = false;
        }
        else if (PlayerState == State.ReleaseChain)
        {
            Rb.simulated = true;

            HitJagde = false;

            NormalJump = true;

            transform.SetParent(null);//親子付け解除

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

            return;
        }

        if (collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            // 磁石によって引き寄せられてるか
            if (collision.gameObject.GetComponent<Magnet>().Pole != Pole)
            {
                MagnetHitCount++;

                HitJagde = true;
            }

            PlayerDirectionCorrection(transform.position.x, collision.gameObject.transform.position.x);

            PlayerAnim.SetPlayerAnimationSpeed(1.0f);

            return;
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

            return;
        }

        if (collision.gameObject.CompareTag("Chain"))
        {
            NormalJump = false;

            WallJump = false;

            return;
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            SetPlayerState(State.Normal);
            IsGround = true;

            return;
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            SetPlayerState(State.Normal);
            IsFootField = true;

            return;
        }
    }

    // 離れたら処理が動く
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            MagnetHitCount = System.Math.Max(MagnetHitCount - 1, 0);

            HitJagde = false;

            Rb.gravityScale = 1.0f;

            return;
        }

        //簡易的に鉄を実装
        if (collision.gameObject.CompareTag("Iron"))
        {
            if (!TwoFlug)
            {
                HitJagde = false;
                Rb.gravityScale = 1.0f;
            }
            else
            {
                TwoFlug = false;
            }

            return;
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            IsGround = false;

            return;
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            IsFootField = false;

            return;
        }
    }

    public void PlayerDirectionCorrection(float posA, float posB)
    {
        if ((posA - posB) >= 0.0f)
        {
            Vector3 localScale = transform.localScale;

            localScale.x = -1.0f;

            transform.localScale = localScale;
        }
        else
        {
            Vector3 localScale = transform.localScale;

            localScale.x = 1.0f;

            transform.localScale = localScale;
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
