using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
    [Header("��")]
    [SerializeField] Magnet_Pole Pole = Magnet_Pole.N;

    [Header("�ړ����x")]
    [SerializeField]�@private float Speed = 5.0f;

    [Header("�W�����v��")]
    [SerializeField] private float JumpPower = 10.0f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;

        if (horizontalKey > 0)
        {
            xSpeed = Speed;
        }
        else if (horizontalKey < 0)
        {
            xSpeed = -Speed;
        }
        else
        {
            xSpeed = 0.0f;
        }

        //�W�����v
        if (Input.GetButtonDown("Jump") && !(rb.velocity.y < -0.5f))
        {
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }

        //�A�N�V����
        if(Input.GetButtonDown("Action"))
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


        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }

    public Magnet_Pole GetPole() { return Pole; }
}
