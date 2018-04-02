/**
 * Author: O'Shea Brown
 * Date: 21 September 2016
 * email: oshea.brown@gmail.com
 * 
 * This class will handle all of the players movements
 */

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    enum PlayerDirection : int { MovingRight = 1, MovingLeft = -1 };
    enum PlayerLookDirection {Right, Left};
    PlayerLookDirection look;
    public Animator animatorController;
    public PlayerHashIDs tags;
    public float firstMaxSpeed = 6f;  //old 6
    public float secondMaxSpeed = 9f; //old 9
    const float acceleration = .2f;  //old .2
    const float deceleration = .5f;  //old .5
    public float currentSpeed;
    public float jumpHeight;
    int jumpCount;
    const int NUM_JUMPS = 2;
    Vector2 tempVelocity;
    public bool isGrounded;

    // Use this for initialization
    void Start()
    {
        // initialize the ground check to false and the jump count to zero
        isGrounded = false;
        jumpCount = 0;
        currentSpeed = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // jump if the player hits the spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // increment jump count every time the player hits space
            jumpCount++;

            // only allow the player to jump a certain amount of times
            if (jumpCount <= NUM_JUMPS)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jumpHeight, 0);
            }
        }

        // move the player left or right based on keyboard input
        if (Input.GetKey(KeyCode.D) && isGrounded)
        {
            AcceleratePlayer(PlayerDirection.MovingRight);
            animatorController.SetFloat(tags.speedFloat, currentSpeed);
        }
        else if (Input.GetKey(KeyCode.A) && isGrounded)
        {
            AcceleratePlayer(PlayerDirection.MovingLeft);
            animatorController.SetFloat(tags.speedFloat, -currentSpeed);
        }
        // if the player is on the ground and not moving, slow them down
        else if (isGrounded)
        {
            DeceleratePlayer();
            animatorController.SetFloat(tags.speedFloat, currentSpeed);
        }

        if (GetComponent<Rigidbody>().velocity.x > 0)
        {
            //look = PlayerLookDirection.Right;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (GetComponent<Rigidbody>().velocity.x < 0)
        {
            //look = PlayerLookDirection.Left;
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }

    }

    void AcceleratePlayer(PlayerDirection playerDirection)
    {
        if (currentSpeed < secondMaxSpeed && playerDirection == PlayerDirection.MovingRight)
        {
            // if the player is currently moving left, accelerate at a faster rate to change directions
            if (currentSpeed < firstMaxSpeed)
            {
                currentSpeed += acceleration * 5;
            }
            // otherwise accelerate normally
            else
            {
                currentSpeed += acceleration;
            }
        }
        else if (currentSpeed >= -secondMaxSpeed && playerDirection == PlayerDirection.MovingLeft)
        {
            // if the player is currently moving right, deccelerate at a faster rate to change directions
            if (currentSpeed > -firstMaxSpeed)
            {
                currentSpeed -= acceleration * 5;
            }
            // otherwise accelerate normally
            else
            {
                currentSpeed -= acceleration;
            }
        }

        GetComponent<Rigidbody>().velocity = new Vector3(currentSpeed, GetComponent<Rigidbody>().velocity.y, 0);
    }

    void DeceleratePlayer()
    {
        // if the player is currently moving right, decrease the current speed to 0
        if (currentSpeed > 0)
        {
            currentSpeed -= deceleration;

            if (currentSpeed < 0.5)
            {
                currentSpeed = 0;
            }
        }
        // if the player is currently moving left, increase the current speed to 0
        if (currentSpeed < 0)
        {
            currentSpeed += deceleration;

            if (currentSpeed > -0.5)
            {
                currentSpeed = 0;
            }
        }

        // apply the deceleration to the players velocity
        GetComponent<Rigidbody>().velocity = new Vector3(currentSpeed, GetComponent<Rigidbody>().velocity.y, 0);
    }

    void OnCollisionEnter(Collision coll)
    {
        //print("ground");
        // set isGrounded to true if the player is currently on the ground
        if (coll.gameObject.tag == "Floor")
        {
            isGrounded = true;
            print("ground");
            // reset jump count to zero
            jumpCount = 0;
        }
        
    }

    void OnCollisionExit(Collision coll)
    {
        // set isGrounded to false if the player is not currently on the ground
        if (coll.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }
}
