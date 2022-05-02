using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTheChain : MonoBehaviour
{
    //キャラクターの到達点
    [SerializeField]
    private Transform ArrivalPoint;

    private MoveChain ChainObj;

    void Start()
    {
        ChainObj = GetComponent<MoveChain>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.tag == "Player"
            && collision.GetComponent<Player>().GetPlayerState() != Player.State.CatchChain
            && collision.GetComponent<Player>().GetPlayerState() != Player.State.ReleaseChain)
            {
            collision.GetComponent<Rigidbody2D>().simulated = false;

                Vector3 lossyScale = collision.transform.lossyScale;

                Vector3 localPlayer = collision.transform.localScale;

                //大元の親を取得してプレイヤーをそこに親子付け
                collision.transform.SetParent(transform.root);

            collision.transform.localScale = new Vector3(
                 localPlayer.x / lossyScale.x * lossyScale.x,
                 localPlayer.y / lossyScale.y * lossyScale.y,
                 localPlayer.z / lossyScale.z * lossyScale.z);


            //キャラクターにCatchTheChainスクリプトを渡し、状態を変更する
            collision.GetComponent<Player>().SetPlayerState(Player.State.CatchChain, this);

            ChainObj.SetMoveFlag(true);
            }
    }

    public Vector3 GetArrivalPoint()
    {
        return ArrivalPoint.localPosition;
    }
}