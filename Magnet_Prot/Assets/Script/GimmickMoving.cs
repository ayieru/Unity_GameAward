using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickMoving : MonoBehaviour
{

    [Header("移動箇所設定")]
    public Vector2[] MovePoint;

    [Header("移動速度")]
    public float Speed = 1.0f;


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
            rb.position = MovePoint[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MovePoint == null || MovePoint.Length <= 0 || rb == null) return;

        Vector2 MovePos = this.transform.position;

        // 通常
        if (!NowReturn)
        {
            int nextPoint = NowMovePoint + 1;

            // 目標ポイントとの誤差がわずかになるまで移動
            if (Vector2.Distance(MovePos, MovePoint[nextPoint]) > 0.1f)
            {
                // 現在地から次のポイントへのベクトルを作成
                Vector2 toVector = Vector2.MoveTowards(MovePos, MovePoint[nextPoint], Speed * Time.deltaTime);

                // 次のポイントへ移動
                rb.MovePosition(toVector);
            }
            // 次のポイントを１つ進める
            else
            {
                rb.MovePosition(MovePoint[nextPoint]);
                ++NowMovePoint;

                // 現在地が配列の最後だった場合
                if (NowMovePoint + 1 >= MovePoint.Length) NowReturn = true;
            }
        }
        // 折返し
        else
        {
            int nextPoint = NowMovePoint - 1;

            // 目標ポイントとの誤差がわずかになるまで移動
            if (Vector2.Distance(MovePos, MovePoint[nextPoint]) > 0.1f)
            {
                // 現在地から次のポイントへのベクトルを作成
                Vector2 toVector = Vector2.MoveTowards(MovePos, MovePoint[nextPoint], Speed * Time.deltaTime);

                // 次のポイントへ移動
                rb.MovePosition(toVector);
            }
            // 次のポイントを１つ戻す
            else
            {
                rb.MovePosition(MovePoint[nextPoint]);
                --NowMovePoint;

                // 現在地が配列の最初だった場合
                if (NowMovePoint <= 0) NowReturn = false;
            }
        }
    }
}
