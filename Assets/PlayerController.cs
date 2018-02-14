using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float maxJumpVelocity = 10f;
    public float minJumpVelocity = 5f;

    public int jumpCount = 1;
    private int currentJumpCount;
    private bool jump = false;
    private bool jumpCancel = true;


    Rigidbody2D rigid;

    Vector2 movement;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentJumpCount = jumpCount;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 100), currentJumpCount.ToString());
        
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(transform.position, -Vector2.up);
        return Physics2D.Raycast(transform.position, -Vector2.up, GetComponent<Collider2D>().bounds.extents.y + 0.1f);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && !jump)
            jump = true;
        if (Input.GetButtonUp("Jump") && !IsGrounded())
            jumpCancel = true;
        movement = Input.GetAxis("Horizontal") * Vector2.right * moveSpeed;
    }

    private void FixedUpdate()
    {
        Vector2 yCorrection = rigid.velocity.y * Vector2.up;
        rigid.velocity = movement;
        rigid.velocity += yCorrection;
        if (jump)
        {
            if (currentJumpCount > 0)
            {
                rigid.velocity += maxJumpVelocity * Vector2.up;
                currentJumpCount--;
            }
        }
        else
            currentJumpCount = jumpCount;
        
        if (jumpCancel)
        {
            if (rigid.velocity.y > minJumpVelocity)
                rigid.velocity += minJumpVelocity * Vector2.up;
            jumpCancel = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            jump = false;
    }
}
