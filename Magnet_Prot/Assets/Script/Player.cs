using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("�ړ����x")]
    [SerializeField]
    private float speed;

    [Header("�W�����v��")]
    [SerializeField]
    private float jumpPower;

    private Rigidbody2D rb;

    void Start()
    {

    }

    void Update()
    {
        rb = GetComponent<Rigidbody2D>();

        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;

        if (horizontalKey > 0)
        {
            xSpeed = speed;
        }
        else if (horizontalKey < 0)
        {
            xSpeed = -speed;
        }
        else
        {
            xSpeed = 0.0f;
        }

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space) && !(rb.velocity.y < -0.5f))
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }


        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }
}
