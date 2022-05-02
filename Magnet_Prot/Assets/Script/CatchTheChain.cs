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
            if (collision.tag == "Player"
            && collision.GetComponent<Player>().GetPlayerState() != Player.State.CatchChain
            && collision.GetComponent<Player>().GetPlayerState() != Player.State.ReleaseChain)
            {
            collision.GetComponent<Rigidbody2D>().simulated = false;

                Vector3 lossyScale = collision.transform.lossyScale;

                Vector3 localPlayer = collision.transform.localScale;

                //�匳�̐e���擾���ăv���C���[�������ɐe�q�t��
                collision.transform.SetParent(transform.root);

            collision.transform.localScale = new Vector3(
                 localPlayer.x / lossyScale.x * lossyScale.x,
                 localPlayer.y / lossyScale.y * lossyScale.y,
                 localPlayer.z / lossyScale.z * lossyScale.z);


            //�L�����N�^�[��CatchTheChain�X�N���v�g��n���A��Ԃ�ύX����
            collision.GetComponent<Player>().SetPlayerState(Player.State.CatchChain, this);

            ChainObj.SetMoveFlag(true);
            }
    }

    public Vector3 GetArrivalPoint()
    {
        return ArrivalPoint.localPosition;
    }
}