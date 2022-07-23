using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimation : MonoBehaviour
{
    public enum AnimationLayer
    {
        Player_Red = 0,//万が一の為に一応値を設定
        Player_Blue = 1,
    }

    [Header("ゲームオーバーフラグ(リザルトシーンで使用)")]
    [SerializeField]
    private bool GameOver = false;

    [Header("ゲームクリアフラグ(リザルトシーンで使用)")]
    [SerializeField]
    private bool ResultGameClear = false;

    private Animator PlayerAnim;

    private AnimatorStateInfo StateInfo;

    private Player PlayerObj;

    private AnimationLayer CurrentLayer = AnimationLayer.Player_Red;

    private bool SpecialFloor = false;

    void Start()
    {
        PlayerAnim = GetComponent<Animator>();

        PlayerObj = GetComponent<Player>();

        CurrentLayer = AnimationLayer.Player_Red;

        PlayerAnim.speed = 1.0f;

        SpecialFloor = false;

        if (PlayerObj.GetPole() == Magnet.Magnet_Pole.N)
        {
            MagnetChange(AnimationLayer.Player_Red);
        }
        else if (PlayerObj.GetPole() == Magnet.Magnet_Pole.S)
        {
            MagnetChange(AnimationLayer.Player_Blue);
        }

        PlayerAnim.Play("Idle", (int)CurrentLayer);
    }


    void Update()
    {
        //↓の二つは遷移先で再生させる
        if (ResultGameClear && SceneManager.GetActiveScene().name == "Result")
        {
            PlayerAnim.Play("Goal", (int)CurrentLayer);

            return;
        }
        else if (GameOver && SceneManager.GetActiveScene().name == "Over")
        {
            PlayerAnim.Play("GameOver", (int)CurrentLayer);

            return;
        }

        if (PlayerObj.GetHitJagde() || PlayerObj.GetMagnetHitCount() >= 1)//上下移動状態
        {
            PlayerAnim.Play("Climbing", (int)CurrentLayer);

            if (PlayerObj.GetVerticalKey() == 0)
            {
                PlayerAnim.speed = 0;//停止
            }
            else
            {
                PlayerAnim.speed = 1;//再開
            }
        }
        else//左右移動状態
        {
            if (PlayerObj.GetComponent<Rigidbody2D>().velocity.y > 0.5f)
            {
                PlayerAnim.Play("Jump", (int)CurrentLayer);

                return;
            }
            else if (PlayerObj.GetComponent<Rigidbody2D>().velocity.y < 0.5f)
            {
                //地面or足場と触れていない。かつMagnetGroundやIronGroundといった特殊な足場と触れていない時
                if (!PlayerObj.IsJump() && !SpecialFloor && PlayerObj)//空中で磁石に引き寄せられている時or落下してる時
                {
                    PlayerAnim.speed = 1.0f;

                    PlayerAnim.Play("Fall", (int)CurrentLayer);

                    return;
                }
            }

            if (PlayerObj.GetHorizontalKey() > 0 || PlayerObj.GetHorizontalKey() < 0)//歩いてる時
            {
                PlayerAnim.speed = 1.0f;

                PlayerAnim.Play("Walk", (int)CurrentLayer);
            }
            else if (PlayerObj.GetHorizontalKey() == 0)//歩いてる時
            {
                PlayerAnim.Play("IdleSide", (int)CurrentLayer);
            }
        }
    }

    public void SetResultGameClear(bool enable) { ResultGameClear = enable; }

    public bool GetResultGameClear() { return ResultGameClear; }

    public void SetGameOver(bool enable) { GameOver = enable; }

    public bool GetGameOver() { return GameOver; }

    public void SetSpecialFloor(bool enable) { SpecialFloor = enable; }

    /// <summary>
    /// アニメーション再生速度設定
    /// </summary>
    /// <param name="speed">0.0fは一時停止、1.0fは再生させるイメージ</param>
    public void SetPlayerAnimationSpeed(float speed = 0.0f) { PlayerAnim.speed = speed; }

    public float GetPlayerAnimationSpeed() { return PlayerAnim.speed; }

    public AnimationLayer GetCurrentLayer() { return CurrentLayer; }

    /// <summary>
    /// プレイヤーの極切り替えの際のアニメーションレイヤー変更
    /// </summary>
    /// <param name="nextLayer">切り替え後のレイヤー</param>
    public void MagnetChange(AnimationLayer nextLayer)
    {
        if (CurrentLayer == nextLayer)
        {
            return;
        }

        PlayerAnim.SetLayerWeight((int)CurrentLayer, 0.0f);//現在のレイヤー

        PlayerAnim.SetLayerWeight((int)nextLayer, 1.0f);//次のレイヤー

        CurrentLayer = nextLayer;
    }

    /// <summary>
    /// unity側でジャンプ再生が終わったら呼び出す関数として使用する
    /// </summary>
    public void AnimationStop()
    {
        PlayerAnim.speed = 0;//停止
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") ||
            collision.gameObject.CompareTag("Block"))
        {
            PlayerAnim.speed = 1.0f;

            PlayerAnim.Play("Idle", (int)CurrentLayer);

            PlayerObj.SetNormalJump(false);

            PlayerObj.SetWallJump(false);

            return;
        }

        if (collision.gameObject.CompareTag("MagnetGround")
            || collision.gameObject.CompareTag("IronGround")
            || collision.gameObject.CompareTag("MagnetBlock"))
        {
            PlayerObj.SetWallJump(false);

            PlayerObj.SetNormalJump(false);

            PlayerObj.SetPlayerState(Player.State.Normal);

            return;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Iron")
            ||collision.gameObject.CompareTag("NPole")
            ||collision.gameObject.CompareTag("SPole"))
        {
            PlayerAnim.Play("Climbing", (int)CurrentLayer);

            return;
        }

        if (collision.gameObject.CompareTag("MagnetGround")
            || collision.gameObject.CompareTag("IronGround")
            || collision.gameObject.CompareTag("MagnetBlock"))
        {
            SpecialFloor = true;

            return;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            if (PlayerObj.GetMagnetHitCount() == 0)
            {
                PlayerAnim.Play("Idle", (int)CurrentLayer);
            }

            return;
        }

        if (collision.gameObject.CompareTag("MagnetGround")
        || collision.gameObject.CompareTag("IronGround")
        || collision.gameObject.CompareTag("MagnetBlock"))
        {
            SpecialFloor = false;

            return;
        }
    }
}
