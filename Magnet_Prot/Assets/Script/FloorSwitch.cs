using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwitch : MonoBehaviour
{
    [Header("対応するドア")]
    public GameObject DoorObj;

    private GameObject PlayerObj;
    private Rigidbody2D Rb;
    private Player player;
    private bool FloorSwitchOn = false;

    private void Update()
    {
        //プレイヤーか磁石ブロックが上にいたら
        if(FloorSwitchOn)
        {
            //ドアを開く
            Transform myTransform = DoorObj.transform;

            // 座標を取得
            Vector3 pos = myTransform.position;
            pos.y += 0.01f;
        
            if (pos.y > 0.0f)
            {
                pos.y = 0.0f;
            }

            myTransform.position = pos;
        }
        else
        {
            //ドアを閉じる
            Transform myTransform = DoorObj.transform;

            // 座標を取得
            Vector3 pos = myTransform.position;
            pos.y -= 0.01f;

            if (pos.y < -2.5f)
            {
                pos.y = -2.5f;
            }

            myTransform.position = pos;  //座標を設定
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("MagnetBlock"))
        {
            //フロアスイッチオン
            FloorSwitchOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("MagnetBlock"))
        {
            //フロアスイッチオフ
            FloorSwitchOn = false;
        }
    }
}
