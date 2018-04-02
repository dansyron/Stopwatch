using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{

    #region Fields

    // Player Animator Field
    public Animator animator;

    // Audio Fields
    public AudioSource audioSource;
    public AudioClip playerAttack;
    public AudioClip playerHurt;
    public AudioClip timeSlowDown;
    public AudioClip timeSpeedUp;

    // Knife Game Object Fields
    public GameObject knife;
    private GameObject knifeInstance;
    private bool knifeSpawnLimiter = true;

    // Player's Rigid Body
    public Rigidbody playerRigidBody;

    // Private Fields
    private bool isMovementEnabled;
    private bool playerWillBeDying = false;
    private bool isTimeSlowedDown = false;

    // Slowdown Duration
    public float slowdownDuration = Constants.TIMER_SLOWDOWN_DURATION;


    // MOVEMENT RELATED FIELDS

    // Direction Related Fields
    enum PlayerDirection : int { MovingRight = 1, MovingLeft = -1 };
    enum PlayerLookDirection { Right, Left };
    PlayerLookDirection look;

    // Velocity Related Fields
    public float firstMaxSpeed = 6.0f;
    public float secondMaxSpeed = 9.0f;
    const float acceleration = 0.1f;
    const float deceleration = 0.5f;
    public float currentSpeed;
    Vector2 tempVelocity;

    // Jump Related Fields
    public float jumpHeight;
    int jumpCount;
    const int NUM_JUMPS = 2;

    // Grounded State Boolean
    public bool isGrounded;


    // Animation Hash IDs
    public int speedFloat;
    public int aliveFloat;
    public int attackFloat;

    #endregion

    #region Initialization

    // Use this for initialization
    void Awake()
    {
        // Sets all fields with their respective animation ID
        speedFloat = Animator.StringToHash("Speed");
        aliveFloat = Animator.StringToHash("Alive");
        attackFloat = Animator.StringToHash("Attack");
    }

    void Start () 
    {
        // initialize the ground check to false and the jump count to zero
        isGrounded = false;
        jumpCount = 0;
        currentSpeed = 0;

        // initializes player to be able to move
        isMovementEnabled = true;
	}

    #endregion

    #region Update

    // Update is called once per frame

    // Update contains general functionality
	void Update () 
    {
        // inititate player attack if player is on the ground, the player is not already attacking,
        // the player presses Right Shift, and the player is not dying.
        if (Input.GetKey(KeyCode.RightShift) && isGrounded && playerWillBeDying == false)
        {
            if (animator.GetBool(attackFloat) == false)
            {
                // If attack is initiated, play attack sound and change to attacking animation
                audioSource.PlayOneShot(playerAttack);
                animator.SetBool(attackFloat, true);
            }
        }
        
        // If death boolean is true and player collides with ground, kill player.
        if (isGrounded == true && playerWillBeDying == true)
        {
            // Makes it so player can no longer move and sends info to animator to switch
            // animation to dying animation.
            isMovementEnabled = false;
            animator.SetBool(aliveFloat, false);
        }

        // The knife is despawned and the ability to spawn another is restored upon attack animation finishing
        // this process has to go before the spawn process.
        if (animator.GetBool(attackFloat) == false && knifeSpawnLimiter == false)
        {
            // Destroys knife player has in hand
            Destroy(knifeInstance);

            // Allows the process of regaining a knife
            knifeSpawnLimiter = true;
        }

        // Spawns one knife object and sets its parent to character arm bone
        if (Input.GetKeyDown(KeyCode.RightShift) && knifeSpawnLimiter == true)
        {
            // Spawns knife and sets parent to right hand bone
            knifeInstance = (GameObject)Instantiate(knife, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z)));
            knifeInstance.transform.SetParent(gameObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand));

            // Sets limiter to false to prevent more knifes from being spawned in this attack animation.
            knifeSpawnLimiter = false;
        }

        // Initiates slowdown effect upon pressing the "K" key if slowdown is not already in effect
        if (Input.GetKeyDown(KeyCode.K) && isTimeSlowedDown == false)
        {
            // Applies slowdown effect, plays slowdown sound, and prints message
            isTimeSlowedDown = true;
            audioSource.PlayOneShot(timeSlowDown);
            print("slowdown");

            // Sets slowdown effects in timer and enemy
            // NOTE: THIS CODE WILL BE CHANGED UPON LATER IMPLEMENTATION OF DELEGATES
            FindObjectOfType<TimerUI>().InitiateSlowDown();

            // Only applies enemy slowdown if there are any enemies
            if (FindObjectsOfType<EnemyCharacter>().Length > 0)
            {
                foreach (EnemyCharacter enemyChar in FindObjectsOfType<EnemyCharacter>())
                {
                    enemyChar.InitiateSlowDown();
                }
            }

            // Initiates coroutine that will end slowdown affect after a certain duration of time
            StartCoroutine("WaitForSlowDown");
        }
    
    }


    // FixedUpdate contains specifically movement related functionality
    void FixedUpdate()
    {
        // If movement is not enabled, no movement is allowed
        if (isMovementEnabled == true)
        {
            // Jump if the player hits the spacebar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Increment jump count every time the player hits space
                jumpCount++;

                // Only allow the player to jump a certain amount of times
                if (jumpCount <= NUM_JUMPS)
                {
                    GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jumpHeight, 0);
                }
            }

            // Move the player left or right based on keyboard input
            if (Input.GetKey(KeyCode.D) && isGrounded)
            {
                AcceleratePlayer(PlayerDirection.MovingRight);
                animator.SetFloat(speedFloat, currentSpeed);
            }
            else if (Input.GetKey(KeyCode.A) && isGrounded)
            {
                AcceleratePlayer(PlayerDirection.MovingLeft);
                animator.SetFloat(speedFloat, -currentSpeed);
            }

            // If the player is on the ground and not moving, slow them down
            else if (isGrounded)
            {
                DeceleratePlayer();
                animator.SetFloat(speedFloat, currentSpeed);
            }


            // Changes player's direction based on velocity
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

    }

    #endregion

    #region Collision Related Methods

    // Changes player behavior based on what player collides with
    void OnCollisionEnter(Collision coll)
    {
        // Checks if player collided with an enemy
        if (coll.gameObject.tag == "Enemy")
        {
            // If collision is with enemy, test to see whether enemy is dying before triggering own death.
            // If enemy is dying, player does not die for touching it
            if (coll.gameObject.GetComponent<EnemyCharacter>().isDying == false)
            {
                // Start knockback routine and play "player hurt" sound once
                // Passing in the duration of the knockback, the strength, and the direction the player is facing
                audioSource.PlayOneShot(playerHurt);
                StartCoroutine(Knockback(0.02f, 120, gameObject.transform.forward.normalized));

                // Makes the timer start flashing in reaction to player being hurt
                FindObjectOfType<TimerUI>().FlashTimer();
            }
        }

        // Set isGrounded to true if the player is currently on the ground
        if (coll.gameObject.tag == "Floor")
        {
            isGrounded = true;
            print("ground");

            // Reset jump count to zero
            jumpCount = 0;

            // If timer in scene hasn't started, start the timer now that the player has touched the ground
            if (FindObjectOfType<TimerUI>().HasTimerStarted() == false)
            {
                FindObjectOfType<TimerUI>().StartTimer();
            }
        }
    }

    // Changes player behavior based on what the player is no longer colliding with
    void OnCollisionExit(Collision coll)
    {
        // Set isGrounded to false if the player is not currently on the ground
        if (coll.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }

    // Coroutine that affects the player's movement upon being knocked back
    public IEnumerator Knockback(float knockDur, float knockbackPwr, Vector3 knockbackDir)
    {
        // Set the death boolean
        playerWillBeDying = true;

        // So we are not carrying momentum when landing
        currentSpeed = 0;

        // Initializes local timer to track duration of knockback
        float timer = 0;

        // For the duration of time passed in as argument, add force
        while (knockDur > timer)
        {
            timer += Time.deltaTime;
            //add force that pushes player back
            playerRigidBody.AddForce(new Vector3(-knockbackDir.x - knockbackPwr, knockbackDir.y + knockbackPwr, 0));

        }

        // Returns 0 to exit
        yield return 0;

    }

    #endregion

    #region Private Movement Methods

    // Method by which player's running movement can be accelerated
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
            // If the player is currently moving right, deccelerate at a faster rate to change directions
            if (currentSpeed > -firstMaxSpeed)
            {
                currentSpeed -= acceleration * 5;
            }
            // Otherwise accelerate normally
            else
            {
                currentSpeed -= acceleration;
            }
        }

        // Adjusts player's velocity to newly determined one
        GetComponent<Rigidbody>().velocity = new Vector3(currentSpeed, GetComponent<Rigidbody>().velocity.y, 0);
    }

    // Method by which player's running movement can be decelerated
    void DeceleratePlayer()
    {
        // If the player is currently moving right, decrease the current speed to 0
        if (currentSpeed > 0)
        {
            currentSpeed -= deceleration;

            if (currentSpeed < 0.5)
            {
                currentSpeed = 0;
            }
        }
        // If the player is currently moving left, increase the current speed to 0
        if (currentSpeed < 0)
        {
            currentSpeed += deceleration;

            if (currentSpeed > -0.5)
            {
                currentSpeed = 0;
            }
        }

        // Apply the deceleration to the players velocity
        GetComponent<Rigidbody>().velocity = new Vector3(currentSpeed, GetComponent<Rigidbody>().velocity.y, 0);
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Coroutine that simply waits for a duration of time before ending the
    /// slowdown effect.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForSlowDown()
    {
        yield return new WaitForSeconds(slowdownDuration);

        // Ends slowdown effect, plays time speed up sound, and prints a message
        isTimeSlowedDown = false;
        audioSource.PlayOneShot(timeSpeedUp);
        print("slowdown ended");

        // Ends slowdown effects in timer and enemy
        // NOTE: THIS CODE WILL BE CHANGED UPON LATER IMPLEMENTATION OF DELEGATES
        FindObjectOfType<TimerUI>().EndSlowDown();

        // Only ends enemy slowdown if there are any enemies
        if (FindObjectsOfType<EnemyCharacter>().Length > 0)
        {
            foreach (EnemyCharacter enemyChar in FindObjectsOfType<EnemyCharacter>())
            {
                enemyChar.EndSlowDown();
            }
        }
    }

    #endregion
}
