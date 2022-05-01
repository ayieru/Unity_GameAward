using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTheChain : MonoBehaviour
{
    //�L�����N�^�[�̓��B�_
    [SerializeField]
    private Transform ArrivalPoint = null;

    [SerializeField]
    private Player PlayerObj = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerObj == null)
        {
            Debug.Log("PlayerObj��null�ł�");
            return;
        }
        else
        {
            if (collision.tag == "Player"
            && PlayerObj.GetPlayerState() != Player.State.CatchChain)
            {
                //�L�����N�^�[�̐e�����ɂ���
                collision.transform.SetParent(transform);

                PlayerObj.gameObject.GetComponent<Rigidbody2D>().Sleep();

                //�L�����N�^�[��CatchTheChain�X�N���v�g��n���A��Ԃ�ύX����
                PlayerObj.SetPlayerState(Player.State.CatchChain, this);

            }
        }
    }

    public Vector3 GetArrivalPoint()
    {
        return ArrivalPoint.position;
    }

    //���[�v�ɋL�����Ă����L�����N�^�[�̏�Ԃ��Z�b�g
    //public void SetState(State sta)
    //{
    //    state = sta;
    //}
}