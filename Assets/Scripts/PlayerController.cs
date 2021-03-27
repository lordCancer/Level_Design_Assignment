using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float walkSpeed;
    [SerializeField]
    public float jumpForce = 5.0f;
    public LayerMask WalkableLayer;
    public Transform legs;

    private Rigidbody2D rb { get{return GetComponent<Rigidbody2D>(); } }
    private SpriteRenderer spriteRenderer { get { return GetComponent<SpriteRenderer>(); } }
    private Animator animator { get { return GetComponent<Animator>(); } }
    private bool grounded;
    private bool jumpRequest;

    private void Start()
    {
        rb.freezeRotation = true;
    }

    private void Update()
    {
        Move();
        Debug.Log(grounded);
        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            jumpRequest = true;
        }
    }

    private void FixedUpdate()
    {
        //Checking if we are grounded
        grounded = IsGrounded();

        //Jump 
        if (jumpRequest)
        {
            Jump();
            jumpRequest = false;
            grounded = false;
        }
        else
        {
            grounded = IsGrounded();
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 Movement = new Vector3(horizontalInput, 0.0f, 0.0f);
        transform.position += Movement * Time.deltaTime * walkSpeed;
        if (horizontalInput < 0)
            FlipSprite(true);
        if (horizontalInput > 0)
            FlipSprite(false);
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalInput));
    }

    private void FlipSprite(bool flip)
    {
        spriteRenderer.flipX = flip;
    }

    private bool IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(legs.position, 0.05f, WalkableLayer);
        return grounded;
    }

    private void Jump()
    {
        Debug.Log("Jump is being called");
        grounded = false;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
