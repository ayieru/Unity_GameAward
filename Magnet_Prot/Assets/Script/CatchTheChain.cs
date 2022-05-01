using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTheChain : MonoBehaviour
{
    //キャラクターの到達点
    [SerializeField]
    private Transform ArrivalPoint = null;

    [SerializeField]
    private Player PlayerObj = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerObj == null)
        {
            Debug.Log("PlayerObjはnullです");
            return;
        }
        else
        {
            if (collision.tag == "Player"
            && PlayerObj.GetPlayerState() != Player.State.CatchChain)
            {
                //キャラクターの親を鎖にする
                collision.transform.SetParent(transform);

                PlayerObj.gameObject.GetComponent<Rigidbody2D>().Sleep();

                //キャラクターにCatchTheChainスクリプトを渡し、状態を変更する
                PlayerObj.SetPlayerState(Player.State.CatchChain, this);

            }
        }
    }

    public Vector3 GetArrivalPoint()
    {
        return ArrivalPoint.position;
    }

    //ロープに記憶しておくキャラクターの状態をセット
    //public void SetState(State sta)
    //{
    //    state = sta;
    //}
}