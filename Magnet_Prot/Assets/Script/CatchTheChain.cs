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
        Player player = collision.GetComponent<Player>();

            if (collision.tag == "Player"
            && player.GetPlayerState() != Player.State.CatchChain
            && player.GetPlayerState() != Player.State.ReleaseChain)
            {
            collision.GetComponent<Rigidbody2D>().simulated = false;

                Vector3 lossyScale = player.transform.lossyScale;

                Vector3 localPlayer = player.transform.localScale;

                //大元の親を取得してプレイヤーをそこに親子付け
                collision.transform.SetParent(transform.root);

                player.transform.localScale = new Vector3(
                 localPlayer.x / lossyScale.x * lossyScale.x,
                 localPlayer.y / lossyScale.y * lossyScale.y,
                 localPlayer.z / lossyScale.z * lossyScale.z);


            //キャラクターにCatchTheChainスクリプトを渡し、状態を変更する
            player.SetPlayerState(Player.State.CatchChain, this);

            ChainObj.SetMoveFlag(true);

            ChainObj.SetChainDirection((int)player.GetDirectionX());
        }
    }

    public Vector3 GetArrivalPoint()
    {
        return ArrivalPoint.localPosition;
    }
}
