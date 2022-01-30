using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float JumpForce;
    public float speed = 2;
    private Rigidbody Rigidbody;
    private Animator Animator;
    private float Horizontal;
    private float Vertical;
    private bool Grounded;
    public float distance = 2f;
    bool facingRight;
    bool isRun;
    bool isJump;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");

        Vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(0, 0, Horizontal);
        movement = movement.normalized * speed * Time.deltaTime;

        Rigidbody.MovePosition(transform.position + movement);

        if (Mathf.Abs(Horizontal) > 0.01)
        {
            if (!isJump)
            {
                Animator.SetBool("isRun", true);
                isRun = true;
            }
        }
        else
        {
            Animator.SetBool("isRun", false);
            isRun = false;
        }



        // Salto
        if (Input.GetButtonDown("Jump") && Grounded)
        {
            Debug.Log("saltar");
            Jump();
        }
    }

    private void FixedUpdate()
    {

        Rigidbody.velocity = new Vector3(Horizontal * speed, Rigidbody.velocity.y);

        //Detectar Suelo
        Vector2 forward = transform.TransformDirection(Vector3.down) * distance;
        Debug.DrawRay(transform.position, forward, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, distance))
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }

        if (!Grounded && isJump)
        {
            Debug.Log("en el suelo");
            Animator.SetBool("isJump", false);
            isJump = false;   
        }

        //flipping the player H
        if (Horizontal < 0 && !facingRight)
        {
            flipPlayer();
        }
        else if (Horizontal > 0 && facingRight)
        {
            flipPlayer();
        }


        //flipping the player v
        if (Vertical < 0 && !facingRight)
        {
            //Debug.Log("primer");
            //flipPlayerV();
        }
        else if (Vertical > 0 && facingRight)
        {
            //Debug.Log("segundo");
            //flipPlayerV();
        }

    }

    void flipPlayer()
    {
        facingRight = !facingRight;

        Vector3 localScale = transform.localScale;
        localScale.z *= -1f;
        transform.localScale = localScale;
    }

    void flipPlayerV()
    {
        facingRight = !facingRight;

        Vector3 localScale = transform.localScale;
        localScale.x = -1f;
        transform.localScale = localScale;
    }

    private void Jump()
    {
        //transform.Rotate(Vector3.up, 1f, Space.World);
        isJump = true;
        Animator.SetBool("isJump", true);
        Rigidbody.AddForce(Vector3.up * JumpForce);
    }
}
