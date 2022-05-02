using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTheChain : MonoBehaviour
{
    //�L�����N�^�[�̓��B�_
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

                //�匳�̐e���擾���ăv���C���[�������ɐe�q�t��
                collision.transform.SetParent(transform.root);

                player.transform.localScale = new Vector3(
                 localPlayer.x / lossyScale.x * lossyScale.x,
                 localPlayer.y / lossyScale.y * lossyScale.y,
                 localPlayer.z / lossyScale.z * lossyScale.z);


            //�L�����N�^�[��CatchTheChain�X�N���v�g��n���A��Ԃ�ύX����
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