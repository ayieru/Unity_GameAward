using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MagnetManager
{
    [Header("���ԃI�u�W�F�N�g(��)")]
    public GameObject BigGearObj;

    [Header("���ԃI�u�W�F�N�g(��)")]
    public GameObject SmallGearObj;

    [Header("��]���x")]
    public float RotSpeed;

    [Header("���d�ʂ̍ő�l")]
    public int MaxPower = 20;

    [Header("���d��")]
    private int Power = 0;

    [Header("�v���C���[���͈͓��ɂ��邩")]
    private bool PlayerStay = false;

    private GameObject PlayerObj;
    private Player player;

    void Awake()
    {
        PlayerObj = GameObject.Find("Player");
        player = PlayerObj.GetComponent<Player>();

        Power = 0;
        PlayerStay = false;
    }

    private void Update()
    {
        if (PlayerStay)
        {
            if (Pole == player.GetPole())
            {
                //�ő�l�܂Ŕ��d����
                if (Power < MaxPower)
                {
                    Power += 1;
                }
                else
                {
                    //���܂�����~�߂�@�m�F�p�ɃR�����g�A�E�g
                    //return;
                }

                //���ԁi��j
                BigGearObj.gameObject.transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime * RotSpeed, Space.World);

                //���ԁi���j
                SmallGearObj.gameObject.transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime * RotSpeed, Space.World);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //�v���C���[��������
            PlayerStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //�v���C���[���o��
            PlayerStay = false;
        }

    }
}
