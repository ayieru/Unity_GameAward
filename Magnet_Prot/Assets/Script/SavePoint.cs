using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : SavePointManager
{
    Transform SavePointTransform;

    public static SavePoint instance;

    void Awake()
    {
        SetSaveJadge(false);
        SavePointTransform = this.transform;// transformを取得
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // セーブ実装（1回限り）
        if (GetSaveJadge()) return;

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("セーブ成功");

            // セーブポイントが設置している座標をプレイヤーが復帰する座標に代入
            SavePointPos = SavePointTransform;
            SetSaveJadge(true);
            instance = this;
        }
    }
}
