using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float gravity = -9.81f;
    
    // object that checks for ground contact
    [SerializeField] private Transform platformCheck;

    // radius of sphere to check
    [SerializeField] private float platformDistance = 0.4f;

    // the platform layer
    [SerializeField] private LayerMask platformMask;
    [SerializeField] private float speed = 12f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] private GameManager gameManager;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isThirdPerson = false;

    // get the items that the player is colliding with 
    // (should only be one thing)
    public Collider[] PlayersPlatform()
    {
        return Physics.OverlapSphere(platformCheck.position, 
            platformDistance, platformMask);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(platformCheck.position,
            platformDistance, platformMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        // constantly apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Update()
    {


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // move player in direction with speed and account for framerates
        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetKeyDown("space") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        if (IsGameOver())
        {
            gameManager.EndGame();
        }

        if (Input.GetKeyDown("p"))
        {
            Debug.Log("shifted perspective");
        }
    }

    private bool IsGameOver()
    {
        return transform.position.y < -15;
    }
}
