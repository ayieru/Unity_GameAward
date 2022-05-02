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

    private Vector3 DefaultScale;

    void Start()
    {
        DefaultScale = PlayerObj.transform.lossyScale;
    }

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
                Vector3 lossScale = PlayerObj.transform.lossyScale;

                Vector3 localPlayer = PlayerObj.transform.localScale;

                //大元の親を取得してプレイヤーをそこに親子付け
                collision.transform.SetParent(transform.root);

                PlayerObj.transform.localScale = new Vector3(
                    localPlayer.x / lossScale.x * DefaultScale.x,
                 localPlayer.y / lossScale.y * DefaultScale.y,
                 localPlayer.z / lossScale.z * DefaultScale.z);
                

                //キャラクターにCatchTheChainスクリプトを渡し、状態を変更する
                PlayerObj.SetPlayerState(Player.State.CatchChain, this);

                PlayerObj.GetComponent<Rigidbody2D>().simulated = false;

            }
        }
    }

    public Vector3 GetArrivalPoint()
    {
        return ArrivalPoint.position;
    }
}