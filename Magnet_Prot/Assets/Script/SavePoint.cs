using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : SavePointManager
{
    Transform SavePointTransform;

    public static SavePoint instance;

    void Awake()
    {
        SavePointTransform = this.transform;// transformを取得
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // セーブ実装（1回限り）
        if (collision.gameObject.CompareTag("Player") && SaveJudge == false)
        {
            Debug.Log("セーブ成功");

            // セーブポイントが設置している座標をプレイヤーが復帰する座標に代入
            SavePointPos = SavePointTransform.position;
            SaveJudge = true;
            instance = this;
        }
    }
}