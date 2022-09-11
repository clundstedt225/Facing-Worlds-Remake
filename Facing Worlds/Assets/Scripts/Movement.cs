using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Vars & Properties
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public Vector3 moveDirection = Vector3.zero;
    public Vector3 jumpVelocity = Vector3.zero;
    private float airFriction = 0.5f;
    CharacterController controller;
    float slopeMovementY = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitinfo;
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = Vector3.ClampMagnitude(transform.TransformDirection(moveDirection), 1);

        Ray rayDown = new Ray(transform.position, Vector3.down * 12);
        Physics.Raycast(rayDown, out hitinfo, 12);

        if (controller.isGrounded)
        {
            //If "falling" in the y direction
            if (controller.velocity.y < 0)
            {
                if (hitinfo.normal.y < 1)
                {
                    slopeMovementY = hitinfo.normal.y * speed;
                    moveDirection.y = -slopeMovementY;
                }
                else
                {
                    slopeMovementY = 0;
                }
            }

            moveDirection *= speed;

            //Jumping Logic
            if (Input.GetButtonDown("Jump"))
            {
                jumpVelocity = moveDirection * airFriction;
                jumpVelocity.y = jumpSpeed;
            }
            else
            {
                jumpVelocity = Vector3.zero;
            }
        }
        else
        {
            moveDirection *= 8;
        }

        //Apply movements and simulate gravity
        jumpVelocity.y -= gravity * Time.deltaTime;
        controller.Move((moveDirection + jumpVelocity) * Time.deltaTime);
    }
}
