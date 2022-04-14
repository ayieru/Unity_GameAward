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
        GameObject Iron = GameObject.FindWithTag("Iron");// Iron�̃^�O�����Ă���I�u�W�F�N�g���擾
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

        // ���΂ň����񂹂��Ă��邩�A���Ȃ����̔���
        if (HitJagde == true)
        {
            Rb.velocity = new Vector2(XSpeed, YSpeed);
        }
        else
        {
            Rb.velocity = new Vector2(XSpeed, Rb.velocity.y);
        }
    }

    // ���������^�C�~���O�ŏ���������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Iron"))
        {
            Debug.Log("���������I�I");
            HitJagde = true;
        }
    }

    // ���ꂽ�珈��������
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Iron"))
        {
            Debug.Log("���ꂽ�I�I");
            HitJagde = false;
        }
    }

    public Magnet_Pole GetPole() { return Pole; }
}
