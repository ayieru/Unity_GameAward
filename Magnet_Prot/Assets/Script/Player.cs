using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
    public enum State
    {
        Normal,//�ʏ���
        CatchChain,//���ɕ߂܂��Ă���
        ReleaseChain,//���𗣂������
    }
    public enum PlayerDirection
    {
        Right = 1,//�E����
        Left = -1,//������
    }

    [Header("��")]
    [SerializeField] Magnet_Pole Pole = Magnet_Pole.N;

    [Header("�ړ����x")]
    [SerializeField] float Speed = 5.0f;

    [Header("�W�����v��")]
    [SerializeField] float JumpPower = 10.0f;

    [Header("�i��ł������")]
    [SerializeField] private float DirectionX;

    /*���֘A�̕ϐ�*/
    [Header("���̏���̈ʒu�܂ł̃X�s�[�h")]
    [SerializeField] private float SpeedToRope = 5.0f;

    [Header("���𗣂����Ƃ��ɂ��̌����ɉ������")]
    [SerializeField]
    private float ReleasePower = 2.0f;

    [Header("���𗣂������̗͂����������鎞��")]
    [SerializeField] private float DampingTime = 2.0f;

    [Header("������(���𓮂�����)")]
    [SerializeField] private float SwingPower = 200.0f;

    private MoveChain MoveChainObj;

    private CatchTheChain ChainObj;

    private Quaternion PreRotation;//���[�v��͂�ł��鎞�̃v���C���[�̊p�x�i�[�p

    private bool IsGround = false;

    private Rigidbody2D Rb;

    private bool HitJagde = false;// �����ƃv���C���[��������������
    private bool TwoFlug = false;

    private State PlayerState = State.Normal;

    void Awake()
    {
        IsGround = false;

        Rb = GetComponent<Rigidbody2D>();

        PlayerState = State.Normal;

        DirectionX = (int)PlayerDirection.Right;
    }

    void Update()
    {
        if (PlayerState == State.Normal)
        {
            float HorizontalKey = Input.GetAxisRaw("Horizontal");
            float VerticalKey = Input.GetAxisRaw("Vertical");  // �c���͔����ϐ�
            float XSpeed = 0.0f;
            float YSpeed = 0.0f;                            // �c�ړ��̃X�s�[�h�ϐ�

            if (HorizontalKey > 0)
            {
                XSpeed = Speed;

                DirectionX = (int)PlayerDirection.Right;
            }
            else if (HorizontalKey < 0)
            {
                XSpeed = -Speed;

                DirectionX = (int)PlayerDirection.Left;
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
                if (IsGround)
                {
                    Rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
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
                Rb.velocity = new Vector2(XSpeed, Rb.velocity.y); // �W�����v
            }
        }
        else if (PlayerState == State.CatchChain)
        {
            if (Input.GetButtonDown("Jump"))
            {
                SetPlayerState(State.ReleaseChain);

                Rb.simulated = true;
            }
                if (transform.localPosition != ChainObj.GetArrivalPoint())
                {
                    //���炩�Ɍ��߂�ꂽ�ʒu�Ɉړ�������
                    transform.localPosition = Vector3.Lerp(transform.localPosition, ChainObj.GetArrivalPoint(), SpeedToRope * Time.deltaTime);
                }
        }
        else if (PlayerState == State.ReleaseChain)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, PreRotation, SpeedToRope * Time.deltaTime);

            //���[�v�̓����Ă��鑬�x���擾
            Vector3 velocityXZ = (MoveChainObj.transform.right * DirectionX * ReleasePower);

            //Y�������͏d�͂ɔC�����0�ɂ���
            velocityXZ.y = 0.0f;

            //���[�v�𗣂������̃��[�v�������Ă��鑬�x�Əd�͂𑫂��đS�̂̑��x���v�Z
            Rb.velocity = velocityXZ + new Vector3(0.0f, Rb.velocity.y, 0.0f);

            //�ړ��l������������
            DirectionX = Mathf.Lerp(DirectionX, 0.0f, DampingTime * Time.deltaTime);

            //�d�͂𓭂�����
            Rb.velocity = new Vector3(
                 Rb.velocity.x,
                 Rb.velocity.y + Physics.gravity.y * Time.deltaTime,
                0.0f
                );
        }
    }

    public Magnet_Pole GetPole() { return Pole; }

    public State GetPlayerState() { return PlayerState; }

    public float GetDirectionX() { return DirectionX; } 

    public void SetPlayerState(State state, CatchTheChain catchTheChain = null)
    {
        PlayerState = state;

        if (PlayerState == State.CatchChain)
        {
            //���݂̊p�x��ێ����Ă���
            PreRotation = transform.rotation;

            Rb.velocity = Vector3.zero;

            //�ړ��l���̏�����
            float rot = transform.localEulerAngles.y;

            //�p�x��ݒ肵����
            transform.localRotation = Quaternion.Euler(0.0f, rot, 0.0f);

            SetCatchTheChain(catchTheChain);
        }
        else if (PlayerState == State.ReleaseChain)
        {
            transform.SetParent(null);

            Rb.SetRotation(0.0f);

            transform.Rotate(0.0f, 0.0f, 0.0f);

            //�@���[�v�𗣂������̌�����ێ�
            if (MoveChainObj.GetChainDirection() == 1)
            {
                DirectionX = (float)PlayerDirection.Right;
            }
            else
            {
                DirectionX = (float)PlayerDirection.Left;
            }

            MoveChainObj.SetMoveFlag(false);

        }
        else if (state == State.Normal)
        {
            ChainObj = null;
            transform.rotation = PreRotation;
        }

    }
    public void SetCatchTheChain(CatchTheChain chainBase)
    {
        //CatchTheChain��Chain�X�N���v�g�̎擾
        this.ChainObj = chainBase;
        MoveChainObj = chainBase.GetComponent<MoveChain>();
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

        if (collision.gameObject.CompareTag("NPole") || collision.gameObject.CompareTag("SPole"))
        {
            // ���΂ɂ���Ĉ����񂹂��Ă邩
            if (collision.gameObject.GetComponent<Magnet>().Pole != Pole)
            {
                Debug.Log("���΂ɂ��������I�I");

                HitJagde = true;
            }
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            IsGround = true;

            SetPlayerState(State.Normal);
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            IsGround = true;

            SetPlayerState(State.Normal);
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

        if (collision.gameObject.CompareTag("Floor"))
        {
            IsGround = false;

            SetPlayerState(State.Normal);
        }

        if (collision.gameObject.CompareTag("Block"))
        {
            IsGround = false;

            SetPlayerState(State.Normal);
        }
    }
}
