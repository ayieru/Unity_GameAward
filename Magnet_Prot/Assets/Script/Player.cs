using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MagnetManager
{
    [Header("極")]
    [SerializeField] Magnet_Pole Pole = Magnet_Pole.N;

    [Header("移動速度")]
    [SerializeField]　private float Speed = 5.0f;

    [Header("ジャンプ力")]
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

        //ジャンプ
        if (Input.GetButtonDown("Jump") && !(rb.velocity.y < -0.5f))
        {
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }

        //アクション
        if(Input.GetButtonDown("Action"))
        {
            Debug.Log("アクション");
        }

        //極切り替え
        if (Input.GetButtonDown("MagnetChange"))
        {
            if (Pole == Magnet_Pole.S)
            {
                Pole = Magnet_Pole.N;
                Debug.Log("極切り替え：S → N");
            }
            else
            {
                Pole = Magnet_Pole.S;
                Debug.Log("極切り替え：N → S");
            }
        }


        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }

    public Magnet_Pole GetPole() { return Pole; }
}
