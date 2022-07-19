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

    private Transform tr;
    //private Rigidbody2D rb;
    private int SaveNum = 0;                // 一番近い要素数を保存する
    private bool NowReturn = false;         // 戻るか戻らないか
    private float SaveDistance = 10000.0f;  // 比較するための長さ保存

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        
        // MovePointが存在する場合、かつMovePointの要素数がある場合、かつrbがあるなら
        if (MovePoint != null && MovePoint.Length > 0 /*&& rb != null*/)
        {
            // オブジェクトの位置と要素の位置を比較して近いところに向かわせる
            for (int i = 0; i < MovePoint.Length; i++)
            {
                float Distance;// 今回の長さ
                // 自身とi番目の座標の長さを計算
                Distance = Vector2.Distance(this.transform.position, MovePoint[i].transform.position);
                // 保存した長さと今回の長さを比較する
                if (Distance < SaveDistance)
                {
                    // 長さ・要素数保存
                    SaveDistance = Distance;
                    SaveNum = i;

                    // 初期座標設定
                    tr.position = MovePoint[SaveNum].transform.position;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MovePoint.Length <= 1 /*|| rb == null*/) return;

        // 最初に戻るか
        if(FirstBack)
        {
            // 一番近い要素数を目標ポイントとして使用する
            int nextPoint = SaveNum;

            // 目標ポイントとの誤差がわずかになるまで移動
            if (Vector2.Distance(this.transform.position, MovePoint[nextPoint].transform.position) > 0.1f)
            {
                // 現在地から次のポイントへのベクトルを作成
                Vector2 toVector = Vector2.MoveTowards(this.transform.position, MovePoint[nextPoint].transform.position, Speed * Time.deltaTime);

                // 次のポイントへ移動
                tr.position = toVector;
            }
            else// 次のポイントを１つ進める
            {
                tr.position = MovePoint[nextPoint].transform.position;
                ++SaveNum;// 要素数のカウントを増やす

                // 現在地が配列の最後だった場合、要素数を最初に戻す
                if (SaveNum >= MovePoint.Length) SaveNum = 0;
            }
        }
        else// 順番通り戻るか
        {
            // 通常
            if (!NowReturn)
            {
                int nextPoint = SaveNum;

                // 目標ポイントとの誤差がわずかになるまで移動
                if (Vector2.Distance(this.transform.position, MovePoint[nextPoint].transform.position) > 0.1f)
                {
                    // 現在地から次のポイントへのベクトルを作成
                    Vector2 toVector = Vector2.MoveTowards(this.transform.position, MovePoint[nextPoint].transform.position, Speed * Time.deltaTime);

                    // 次のポイントへ移動
                    tr.position = toVector;
                }
                else// 次のポイントを１つ進める
                {
                    tr.position = MovePoint[nextPoint].transform.position;
                    ++SaveNum;// 要素数のカウントを増やす

                    // 現在地が配列の最後だった場合、戻るようにする
                    if (SaveNum >= MovePoint.Length) NowReturn = true;
                }
            }
            else// 折返し
            {
                // 現在の要素数より1個前の要素数を出す
                int nextPoint = SaveNum - 1;

                // 目標ポイントとの誤差がわずかになるまで移動
                if (Vector2.Distance(this.transform.position, MovePoint[nextPoint].transform.position) > 0.1f)
                {
                    // 現在地から次のポイントへのベクトルを作成
                    Vector2 toVector = Vector2.MoveTowards(this.transform.position, MovePoint[nextPoint].transform.position, Speed * Time.deltaTime);

                    // 次のポイントへ移動
                    tr.position = toVector;
                }
                else// 次のポイントを１つ戻す
                {
                    tr.position = MovePoint[nextPoint].transform.position;
                    --SaveNum;// 要素数のカウントを減らす

                    // 現在地が配列の最初だった場合、通常にする
                    if (SaveNum <= 0) NowReturn = false;
                }
            }
        }
    }
}
