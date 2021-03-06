using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 15f;
    public float slideSpeed = 30f;
    public float gravity = -9.81f;
    public float jumpHeight = 4f;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded; 
    bool doubleJump = true;


    // Update is called once per frame
    void Update()
    {
        //Groundcheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        //Groundmovement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (isGrounded)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                speed = 20;   
            }
            else
            {
                speed = 15;
            }
            //Sliden
            if (Input.GetKey(KeyCode.LeftControl))
            {               
                controller.height = 1.2f;
                controller.Move(move * slideSpeed * Time.deltaTime);
                if(slideSpeed > 0)
                {
                    slideSpeed = slideSpeed - 0.04f;
                }
            }
            else
            {
                slideSpeed = 30f;
                controller.height = 3.4f;
                controller.Move(move * speed * Time.deltaTime);
            }
        }
        else
        {
            //Airborne Movement
            controller.Move(move * (speed-2) * Time.deltaTime);
        }

        
        // Jump and Doublejump
        if(Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                slideSpeed = 30f;
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                doubleJump = true;
            }else if (doubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                doubleJump = false;
            }
            
        }

        
        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

