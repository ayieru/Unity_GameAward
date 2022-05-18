using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootFieldBrock : FootFieldBrockManager
{
    public static FootFieldBrock instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = null;

        // 設置した中心座標保存
        SaveCenterPosX = transform.position.x;
        SaveCenterPosY = transform.position.y;

        /*-----------右側のX座標設定-----------*/
        float RightPosX = SaveCenterPosX;

        // 自身のサイズ分座標をずらす
        RightPosX += gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;

        SaveRightPosX = RightPosX;

        /*-----------左側のX座標設定-----------*/
        float LeftPosX = SaveCenterPosX;

        // 自身のサイズ分座標をずらす
        LeftPosX -= gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;

        SaveLeftPosX = LeftPosX;

        /*-----------左側のY座標設定-----------*/
        float TopPosY = SaveCenterPosY;

        // 自身のサイズ分座標をずらす
        TopPosY += gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f;

        SaveTopPosY = TopPosY;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 既に触れてある状態ならスルーする
        if (instance != null) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            instance = this;// プレイヤーが触れているブロックはinstanceをthisにしてプログラムを動かす
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 既に離れているならスルーする
        if (instance == null) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            instance = null;// プレイヤーが触れていないブロックはinstanceをnullにして消す
        }
    }
}
