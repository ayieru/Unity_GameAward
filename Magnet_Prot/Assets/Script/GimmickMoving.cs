using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickMoving : MonoBehaviour
{

    [Header("移動箇所設定")]
    public GameObject[] MovePoint;// 次に移動する場所

    [Header("移動速度")]
    public float Speed = 1.0f;

    [Header("最後に到達したら最初に戻すか")]
    public bool FirstBack = false;

    private Rigidbody2D rb;
    private int NowMovePoint = 0;   // 現在の要素数
    private bool NowReturn = false; // 戻るか戻らないか

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // MovePointが存在する場合、かつMovePointの要素数がある場合、かつrbがあるなら
        if (MovePoint != null && MovePoint.Length > 0 && rb != null)
        {
            // 初期座標設定
            rb.position = MovePoint[0].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MovePoint.Length <= 1 || rb == null) return;

        // 最初に戻るか
        if(FirstBack)
        {
            int nextPoint = NowMovePoint + 1;

            // 目標ポイントとの誤差がわずかになるまで移動
            if (Vector2.Distance(this.transform.position, MovePoint[nextPoint].transform.position) > 0.1f)
            {
                // 現在地から次のポイントへのベクトルを作成
                Vector2 toVector = Vector2.MoveTowards(this.transform.position, MovePoint[nextPoint].transform.position, Speed * Time.deltaTime);

                // 次のポイントへ移動
                rb.MovePosition(toVector);
            }
            else// 次のポイントを１つ進める
            {
                rb.MovePosition(MovePoint[nextPoint].transform.position);
                ++NowMovePoint;// 要素数のカウントを増やす

                // 現在地が配列の最後だった場合
                if (NowMovePoint + 1 >= MovePoint.Length) NowMovePoint = -1;
            }
        }
        else// 順番通り戻るか
        {
            // 通常
            if (!NowReturn)
            {
                int nextPoint = NowMovePoint + 1;

                // 目標ポイントとの誤差がわずかになるまで移動
                if (Vector2.Distance(this.transform.position, MovePoint[nextPoint].transform.position) > 0.1f)
                {
                    // 現在地から次のポイントへのベクトルを作成
                    Vector2 toVector = Vector2.MoveTowards(this.transform.position, MovePoint[nextPoint].transform.position, Speed * Time.deltaTime);

                    // 次のポイントへ移動
                    rb.MovePosition(toVector);
                }
                else// 次のポイントを１つ進める
                {
                    rb.MovePosition(MovePoint[nextPoint].transform.position);
                    ++NowMovePoint;// 要素数のカウントを増やす

                    // 現在地が配列の最後だった場合
                    if (NowMovePoint + 1 >= MovePoint.Length) NowReturn = true;
                }
            }
            else// 折返し
            {
                int nextPoint = NowMovePoint - 1;

                // 目標ポイントとの誤差がわずかになるまで移動
                if (Vector2.Distance(this.transform.position, MovePoint[nextPoint].transform.position) > 0.1f)
                {
                    // 現在地から次のポイントへのベクトルを作成
                    Vector2 toVector = Vector2.MoveTowards(this.transform.position, MovePoint[nextPoint].transform.position, Speed * Time.deltaTime);

                    // 次のポイントへ移動
                    rb.MovePosition(toVector);
                }
                else// 次のポイントを１つ戻す
                {
                    rb.MovePosition(MovePoint[nextPoint].transform.position);
                    --NowMovePoint;// 要素数のカウントを減らす

                    // 現在地が配列の最初だった場合
                    if (NowMovePoint <= 0) NowReturn = false;
                }
            }
        }
    }
}
