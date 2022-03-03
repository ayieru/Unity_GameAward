using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    Rigidbody rb;
    float playerHeight = 2;

    //移動変数
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    Vector3 accelation;
    [SerializeField] float walkSpeed;
    float dashRate;
    [SerializeField] float dashSpeedRate;
    [SerializeField] float jumpForce;

    [SerializeField] LayerMask groundMask;
    bool isGround;
    float groundDistance = 0.4f;

    float moveMultiplier = 10f;
    float airMultiplier = 0.4f;
    float groundDrag = 6f;
    float airDrag = 2f;

    public Vector3 localGravity;

    // slope
    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void MovementStart()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void MovementUpdate()
    {
        SetMove();
        ControlDrag();
        Jump();

        //slope
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MovementFixedUpdate()
    {
        Move();
    }

    private void SetMove()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            dashRate = dashSpeedRate;
        }
        else 
        {
            dashRate = 1;
        }

        isGround = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);

        //移動方向取得
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;
        forward.y = 0f;
        right.y = 0f;
        
        forward = Input.GetAxisRaw("Vertical") * forward;
        right = Input.GetAxisRaw("Horizontal") * right;

        moveDirection = forward + right;
        //transform.position += velocity.normalized;
    }

    void Move()
    {
        //スピード
        float currentSpeed = walkSpeed * dashRate;

        if(isGround && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * currentSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else if(isGround && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * currentSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else if(!isGround)
        {
            rb.AddForce(moveDirection.normalized * currentSpeed * airMultiplier, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
    }


    void ControlDrag()
    {
        if(isGround)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void SetLocalGravity()
    {
        rb.AddForce(localGravity, ForceMode.Acceleration);
    }

}
