using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
    [Header("��")]
    [SerializeField] Magnet_Pole Pole = Magnet_Pole.N;

    [Header("�ړ����x")]
    [SerializeField] private float Speed = 5.0f;

    [Header("�W�����v��")]
    [SerializeField] private float JumpPower = 10.0f;

    private Rigidbody2D Rb;

    private bool HitJagde = false;// �����ƃv���C���[��������������

    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float HorizontalKey = Input.GetAxis("Horizontal");
        float VerticalKey = Input.GetAxis("Vertical");  // �c���͔����ϐ�
        float XSpeed = 0.0f;
        float YSpeed = 0.0f;                            // �c�ړ��̃X�s�[�h�ϐ�

        if (HorizontalKey > 0)
        {
            XSpeed = Speed;
        }
        else if (HorizontalKey < 0)
        {
            XSpeed = -Speed;
        }
        else
        {
            XSpeed = 0.0f;
        }

        // �c���͔�������
        if (VerticalKey > 0)
        {
            YSpeed = Speed * 2.0f;
        }
        else if (VerticalKey < 0)
        {
            YSpeed = -Speed * 2.0f;
        }
        else
        {
            YSpeed = 0.0f;
        }

        //�W�����v
        if (Input.GetButtonDown("Jump") && !(Rb.velocity.y < -0.5f))
        {
            Rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }

        //�A�N�V����
        if (Input.GetButtonDown("Action"))
        {
            Debug.Log("�A�N�V����");
        }

        //�ɐ؂�ւ�
        if (Input.GetButtonDown("MagnetChange"))
        {
            if (Pole == Magnet_Pole.S)
            {
                Pole = Magnet_Pole.N;
                Debug.Log("�ɐ؂�ւ��FS �� N");
            }
            else
            {
                Pole = Magnet_Pole.S;
                Debug.Log("�ɐ؂�ւ��FN �� S");
            }
        }

        if (HitJagde == true)
        {
            Rb.velocity = new Vector2(XSpeed, YSpeed);      // �ǂ̂ڂ�
        }
        else
        {
            Rb.velocity = new Vector2(XSpeed, Rb.velocity.y);// �W�����v
        }
    }

    // ���������^�C�~���O�ŏ���������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Iron")|| collision.gameObject.CompareTag("NPole")|| collision.gameObject.CompareTag("SPole"))
        {
            // ���΂ɂ���Ĉ����񂹂��Ă邩
            if (collision.gameObject.GetComponent<Magnet>().Pole != Pole)
            {
                Debug.Log("���������I�I");

                HitJagde = true;
            }
        }
    }

    // ���ꂽ�珈��������
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Iron") || collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            Debug.Log("���ꂽ�I�I");
            HitJagde = false;
        }
    }

    public Magnet_Pole GetPole() { return Pole; }
}
