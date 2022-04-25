using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
    [Header("��")]
    [SerializeField] Magnet_Pole Pole = Magnet_Pole.N;

    [Header("�ړ����x")]
    [SerializeField] float Speed = 5.0f;

    [Header("�W�����v��")]
    [SerializeField] float JumpPower = 10.0f;

    [Header("�W�����v�񐔂̏��")]
    [SerializeField] int MaxJumpCount = 1;

    private int JumpCount = 0;

    private Rigidbody2D Rb;

    private bool HitJagde = false;// �����ƃv���C���[��������������
    private bool TwoFlug = false;

    Transform PlayerTransform;
    
    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        PlayerTransform = this.transform;// transform���擾
    }

    void Update()
    {
        float HorizontalKey = Input.GetAxisRaw("Horizontal");
        float VerticalKey = Input.GetAxisRaw("Vertical");  // �c���͔����ϐ�
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
            if (JumpCount < MaxJumpCount)
            {
                Rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);

                JumpCount++;
            }
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
            Rb.velocity = new Vector2(XSpeed, Rb.velocity.y);�@// �W�����v
        }
    }

    // ���������^�C�~���O�ŏ���������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�ȈՓI�ɓS������
        if (collision.gameObject.CompareTag("Iron"))
        {
            Debug.Log("�S�ɂ��������I�I");

            //����Ture
            if (HitJagde)
            {
                TwoFlug = true;
            }

            HitJagde = true;

            Rb.gravityScale = 0.0f;
        }

        if (collision.gameObject.CompareTag("NPole")|| collision.gameObject.CompareTag("SPole"))
        {
            // ���΂ɂ���Ĉ����񂹂��Ă邩
            if (collision.gameObject.GetComponent<Magnet>().Pole != Pole)
            {
                Debug.Log("���΂ɂ��������I�I");

                HitJagde = true;
            }
        }

        if(collision.gameObject.CompareTag("Thorn"))
        {
            Vector2 worldPos = PlayerTransform.position;

            // �ʂ����Z�[�u�|�C���g�̍��W�ɕ���������
            worldPos.x = SavePoint.instance.GetSavePointPosX();
            worldPos.y = SavePoint.instance.GetSavePointPosY();

            PlayerTransform.position = worldPos;// ���W�ݒ�
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            JumpCount = 0;
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            JumpCount = 0;
        }
    }

    // ���ꂽ�珈��������
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            Debug.Log("���ꂽ�I�I");
            HitJagde = false;

            Rb.gravityScale = 1.0f;
        }

        //�ȈՓI�ɓS������
        if (collision.gameObject.CompareTag("Iron"))
        {
            Debug.Log("���ꂽ�I�I");

            if (!TwoFlug)
            {
                HitJagde = false;
                Rb.gravityScale = 1.0f;
            }
            else
            {
                TwoFlug = false;
            }
        }
    }

    public Magnet_Pole GetPole() { return Pole; }
}
