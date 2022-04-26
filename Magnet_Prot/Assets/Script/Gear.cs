using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MagnetManager
{

    [Header("�Ή����Ă����")]
    public Magnet_Pole Pole = Magnet_Pole.N;

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
    private Rigidbody2D Rb;
    private Player player;

    void Awake()
    {
        PlayerObj = GameObject.Find("Player");
        Rb = PlayerObj.GetComponent<Rigidbody2D>();
        player = PlayerObj.GetComponent<Player>();

        Power = 0;
        PlayerStay = false;
    }

    private void Update()
    {
        if(PlayerStay == true && Pole == player.GetPole())
        {
            //�ő�l�܂Ŕ��d����
            if (Power < MaxPower)
            {
                Power += 1;
            }

            //�͈͓��ɂ���Ƃ��ɉ�]

            //���ԁi��j
            BigGearObj.gameObject.transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime * RotSpeed, Space.World);

            //���ԁi���j
            SmallGearObj.gameObject.transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime * RotSpeed, Space.World);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Pole == player.GetPole())
        {
            //�v���C���[��������
            PlayerStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Pole == player.GetPole())
        {
            //�v���C���[���o��
            PlayerStay = false;
        }

    }
}
