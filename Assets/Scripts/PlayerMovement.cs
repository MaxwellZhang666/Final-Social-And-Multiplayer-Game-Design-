using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float jumpHeight = 2f;
    public float gravity = -9.8f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Joystick joystick;

    Vector3 direction;

    private float directionY;

    bool pressedJump;
    public bool groundedPlayer;

    float jHorizontal = 0f;
    float jVertical = 0f;

    Vector3 playerVelocity;

    public PhotonView view;

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if(groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        jHorizontal = joystick.Horizontal * speed;
        jVertical = joystick.Vertical * speed;
        direction = new Vector3(jHorizontal, 0f, jVertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }

    public void JumpButtonPressed()
    {
        if (groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }


}
