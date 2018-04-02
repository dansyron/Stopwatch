using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum PlayerState { Grounded, Dead, NotGrounded,  };

public class PlayerController : MonoBehaviour {

    //fields
    public enum ActionState { IsAttacking, IsHurt, IsDodging, None };
    public float curHorSpeed = 0;
    Rigidbody rbody;
    Animator anim;
    bool breakVariableJump = false;
    float variableJumpTimer = 0;
    public float directionMod = 1f;
    TimerUI timerUI;
    float dodgeTimer = 0;
    float hurtTimer = 0;
    //float attackTimer = 0;
    float boostTimer = 0;
    bool hurtRunOnce = true;
    //bool attackRunOnce = true;
    //public GameObject Knife;
    public bool isBoosting = false;
    //bool inDodgeWithEnemy = false;
    GameObject[] enemies;
    //store enemy layers
    List<Collision> collisions = new List<Collision>();

    //dodge particle effect
    //public GameObject dodgePart;

    //fields for model color changes
    [SerializeField]
    Renderer bodyRend;
    [SerializeField]
    Renderer headRend;
    [SerializeField]
    Renderer clothingRend;
    [SerializeField]
    Renderer hairRend;
    [SerializeField]
    Renderer feetRend;
    [SerializeField]
    Material defaultMaterial;
    [SerializeField]
    Material defaultHeadMaterial;
    [SerializeField]
    Material defaultHairMaterial;
    [SerializeField]
    Material defaultBodyMaterial;
    [SerializeField]
    Material defaultClothingMaterial;
    [SerializeField]
    Material flashMaterial;
    [SerializeField]
    Material dodgeMaterial;

    /// <summary>
    /// Player Action state
    /// </summary>
    public ActionState Action
    {
        get; set;
    }

    public ActionState PreviousAction
    {
        get; set;
    }

    /// <summary>
    /// Player state
    /// </summary>
    public PlayerState State
    {
        get; private set;
    }

    /// <summary>
    /// Called from CheckForGround script. Returns if the GroundCheck is
    /// colliding with Constants.GROUND_LAYERS
    /// </summary>
    public void IsGrounded(bool i)
    {
        //fix the flappy bird effect
        //Debug.Log(rbody.velocity.y);
        //if ((rbody.velocity.y > 2 || rbody.velocity.y < -2) && State == PlayerState.Grounded)
        //{
        //    State = PlayerState.NotGrounded;
        //}
        //else
        //{
            if (i)
            {
                if (State == PlayerState.NotGrounded)
                {
                    AudioManager.instance.PlaySingle(SoundEffect.PlayerLand);
                }

                State = PlayerState.Grounded;
                directionMod = 1f;
            }
            else
            {
                State = PlayerState.NotGrounded;
            }
        //}
    }


    /// <summary>
    /// temporary
    /// </summary>
    void Awake()
    {
        GameManager.Instance.Player = this;
        timerUI = GameManager.Instance.Timer;
    }

	// Use this for initialization
	void Start ()
    {
        Action = ActionState.None;
        State = PlayerState.NotGrounded;
        rbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        timerUI = GameManager.Instance.Timer;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        timerUI.StartTimer();
        GameManager.Instance.playerInitialPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(State);
        //Debug.Log(Action);
        //Debug.Log(rbody.velocity.x);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (isBoosting == false)
        {
            //movement and acceleration
            curHorSpeed = Mathf.Clamp(rbody.velocity.x + Constants.HOR_ACCEL_SPEED * Time.deltaTime * directionMod, -Constants.HOR_MAX_SPEED, Constants.HOR_MAX_SPEED);
        }
        else
        {
            //boost movement and acceleration
            curHorSpeed = Mathf.Clamp(rbody.velocity.x + Constants.HOR_BOOST_ACCEL_SPEED * Time.deltaTime * directionMod, -Constants.HOR_BOOST_SPEED, Constants.HOR_BOOST_SPEED);

            if(!AudioManager.instance.efxSource.isPlaying)
            {
                AudioManager.instance.PlaySingle(SoundEffect.PlayerBoost);
            }

            boostTimer--;

            if(boostTimer == 0)
            {
                isBoosting = false;
            }
        }

        rbody.velocity = new Vector3(curHorSpeed, rbody.velocity.y, 0);

        //if the player lands and is moving negative, set x to 0
        if (State == PlayerState.Grounded && rbody.velocity.x < - .5f)
        {
            rbody.velocity = new Vector3(0, rbody.velocity.y, 0);
        }
        
        //handle wall jump
        Ray top = new Ray(new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), Vector3.right);
        Ray middle = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.right);
        Ray back = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.left);
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), new Vector3(transform.position.x + GetComponent<CapsuleCollider>().radius * transform.localScale.x + .1f, transform.position.y + 3f, transform.position.z), Color.green);
        RaycastHit info, info2, info3;
        Physics.Raycast(top, out info, GetComponent<CapsuleCollider>().radius * transform.localScale.x + .1f);
        Physics.Raycast(middle, out info2, GetComponent<CapsuleCollider>().radius * transform.localScale.x + .1f);
        Physics.Raycast(back, out info3, GetComponent<CapsuleCollider>().radius * transform.localScale.x + .1f);

        //if the center forward ray is hiting a wall and the player is grounded, dont move forward
        if (info2.collider && info2.collider.gameObject.layer == 12 && State == PlayerState.Grounded)
        {
            rbody.velocity = new Vector3(0, rbody.velocity.y, 0);
        }

        //If both forward rays are hitting a wall, we want to wall jump
        if (info.collider && info.collider.gameObject.layer == 12 && info2.collider && info2.collider.gameObject.layer == 12)
        {
            //test code. at the base of a wall jump, the player jumps normally until the ground check is out of ground collision
            //State = PlayerState.NotGrounded;

            //wall jump off right wall
            if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                directionMod = -1f;
                rbody.velocity = new Vector3(-Constants.HOR_MAX_SPEED, Constants.INIT_JUMP_VELOCITY, 0);
                variableJumpTimer = 0f;
                breakVariableJump = false;
            }
            else
            {
                rbody.velocity = new Vector3(rbody.velocity.x, -Constants.WALL_SLIDE_VELOCITY, 0);
            }
        }
        else if (info2.collider && info2.collider.gameObject.layer == 12)
        {
            // wall climb jump
			if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                rbody.velocity = new Vector3(rbody.velocity.x, Constants.WALL_CLIMB_VELOCITY, 0);
            }
            //wall slide
            else
            {
                rbody.velocity = new Vector3(rbody.velocity.x, -Constants.WALL_SLIDE_VELOCITY, 0);
            }
        }
        else if (info3.collider && info3.collider.gameObject.layer == 12)
        {
            //jump off left wall
			if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                directionMod = 1f;
                rbody.velocity = new Vector3(Constants.HOR_MAX_SPEED, Constants.INIT_JUMP_VELOCITY, 0);
                variableJumpTimer = 0f;
                breakVariableJump = false;
            }
            //wall slide
            else
            {
                //disable this if the player is moving forward to prevent landing slides
                if (rbody.velocity.x < 1)
                {
                    rbody.velocity = new Vector3(0, -Constants.WALL_SLIDE_VELOCITY, 0);
                }
            }
        }

        //jump control
        else if (State == PlayerState.Grounded)
        {
            if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                rbody.velocity = new Vector3(rbody.velocity.x, Constants.INIT_JUMP_VELOCITY, 0);
                variableJumpTimer = 0f;
                breakVariableJump = false;

                AudioManager.instance.PlaySingle(SoundEffect.PlayerJump);
            }
        }
        else
        {
            if (!(Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) || rbody.velocity.y <= 1f)
            {
                breakVariableJump = true;
            }
            else if (!breakVariableJump && variableJumpTimer < Constants.VARIABLE_JUMP_TIMER)
            {
                variableJumpTimer += Time.deltaTime;
                rbody.velocity = new Vector3(rbody.velocity.x, Constants.INIT_JUMP_VELOCITY, 0);
            }
        }

        //handle dodging
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (Action != ActionState.IsDodging)
            {
                AudioManager.instance.PlaySingle(SoundEffect.PlayerDodge);

                //trigger animation
                //anim.SetTrigger("Dodge");

                //Physics.IgnoreLayerCollision(10, 11, true);
                Action = ActionState.IsDodging;

                //dodge materials
                //bodyRend.material = dodgeMaterial;
                //headRend.material = dodgeMaterial;
                //clothingRend.material = dodgeMaterial;
                //hairRend.material = dodgeMaterial;
                //feetRend.material = dodgeMaterial;
            }
        }

        if (Action == ActionState.IsDodging)
        {
            dodgeTimer += Time.deltaTime;

            //leave IsDodging state check
            if (dodgeTimer >= Constants.DODGE_TIME)
            {
                //Physics.IgnoreLayerCollision(10, 11, false);
                Action = ActionState.None;
                dodgeTimer = 0;

                //normal materials back
                //bodyRend.material = defaultBodyMaterial;
                //headRend.material = defaultHeadMaterial;
                //clothingRend.material = defaultClothingMaterial;
                //hairRend.material = defaultHairMaterial;
                //feetRend.material = defaultBodyMaterial;
            }

            //enemies = GameObject.FindGameObjectsWithTag("Enemy");

            //for (int i = 0; i < enemies.Count(); i++)
            //{
            //    if ((((transform.position.x + 2) >= enemies[i].transform.position.x) && ((transform.position.x - 2) <= enemies[i].transform.position.x)) && (((transform.position.y + 2) >= enemies[i].transform.position.y) && ((transform.position.y - 2) <= enemies[i].transform.position.y)))
            //    {
            //        //Debug.Log("true");
            //        inDodgeWithEnemy = true;
            //    }
            //    //else
            //    //{
            //    //    //Debug.Log("false");
            //    //    inDodgeWithEnemy = false;
            //    //}
            //}
        }

        //handle player hurt
        if (Action == ActionState.IsHurt)
        {
            hurtTimer += Time.deltaTime;

            //leave IsHurt state check
            if (hurtTimer >= Constants.HURT_TIME)
            {
                Action = ActionState.None;
                hurtTimer = 0;
                hurtRunOnce = true;

                //normal materials back
                bodyRend.material = defaultBodyMaterial;
                headRend.material = defaultHeadMaterial;
                clothingRend.material = defaultClothingMaterial;
                hairRend.material = defaultHairMaterial;
                feetRend.material = defaultBodyMaterial;
            }
            else
            {
                //set velocity and materials once
                if (hurtRunOnce)
                {
                    hurtRunOnce = false;

                    timerUI.FlashTimer();

                    //damage materials
                    bodyRend.material = flashMaterial;
                    headRend.material = flashMaterial;
                    clothingRend.material = flashMaterial;
                    hairRend.material = flashMaterial;
                    feetRend.material = flashMaterial;

                    //set velocity
                    curHorSpeed = Mathf.Clamp(rbody.velocity.x + Constants.HOR_ACCEL_SPEED * Time.deltaTime * directionMod, -Constants.HURT_HOR_MAX_SPEED, Constants.HURT_HOR_MAX_SPEED);
                    rbody.velocity = new Vector3(curHorSpeed, rbody.velocity.y, 0);
                }
            }
        }
        //force normal materials back (bug workaround for now)
        else
        {
            bodyRend.material = defaultBodyMaterial;
            headRend.material = defaultHeadMaterial;
            clothingRend.material = defaultClothingMaterial;
            hairRend.material = defaultHairMaterial;
            feetRend.material = defaultBodyMaterial;
        }

        //handle attacking
		//if (Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
  //      {
  //          if (!AudioManager.instance.efxSource.isPlaying)
  //          {
  //              AudioManager.instance.PlaySingle(SoundEffect.PlayerAttack);
  //          }

  //          if (Action != ActionState.IsAttacking && attackRunOnce)
  //          {
  //              Action = ActionState.IsAttacking;
  //              GameManager.Instance.Attack.ToggleCollider();
  //              attackRunOnce = false;
  //          }
  //      }

  //      if (!attackRunOnce)
  //      {
  //          attackTimer += Time.deltaTime;

  //          //temporary timer threshold
  //          if (attackTimer >= Constants.ATTACK_TIME)
  //          {
  //              Action = ActionState.None;
  //              GameManager.Instance.Attack.ToggleCollider();
  //              attackTimer = 0;
  //              attackRunOnce = true;
  //          }
  //      }

        HandleAnimations();

        //if((PreviousAction == ActionState.IsDodging)&&(Action != ActionState.IsDodging))
        //{
        //    if(inDodgeWithEnemy && (Action != ActionState.IsHurt))
        //    {
        //        //Instantiate(dodgePart, transform.position, new Quaternion(0,0,0,0));
        //        isBoosting = true;
        //        boostTimer = 120;
        //        inDodgeWithEnemy = false;
        //    }
        //}

        PreviousAction = Action;
	}

    void HandleAnimations()
    {
        //handling speed
        anim.SetFloat("Speed", rbody.velocity.x);
        anim.SetFloat("YSpeed", rbody.velocity.y);

        //Handle IsJumping
        if (State != PlayerState.NotGrounded && Mathf.Abs(rbody.velocity.y) < .25f)
        {
            anim.SetBool("IsJumping", false);
        }
        else if (State == PlayerState.NotGrounded)
        {
            anim.SetBool("IsJumping", true);
        }

        //handle IsHurt
        if (Action == ActionState.IsHurt)
        {
            
        }

        //handle IsAttacking
        if (Action == ActionState.IsAttacking)
        {
            anim.SetBool("Attack", true);
        }
        //ends attak animation if the player is not attacking
        else
        {
            anim.SetBool("Attack", false);
        }

        //handle isDodging
        /*if(Action == ActionState.IsDodging)
        {
            //trigger animation
            anim.SetTrigger("Dodge");
            anim.SetTrigger("Dodge");
            //anim.SetBool("Dodge", true);
            //anim.SetBool("Dodge", false);
        }
        else
        {
            //end animation
            anim.SetBool("Dodge", false);
        }*/
    }

    /// <summary>
    /// Checks to see if the Player collider is colliding with anything that
    /// is an enemy, or Constants.ENEMY_LAYERS.
    /// </summary>
    /// <returns>result</returns>
    bool IsCollidingWithEnemy()
    {
        return collisions.Any(t => GameManager.Instance.EnemyLayers.Any(y => y == t.gameObject.layer));
    }

    #region Unity Methods

    /// <summary>
    /// Used to check if in contact with an enemy
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "PlayerKnife")
        {
            Physics.IgnoreLayerCollision(this.gameObject.layer, col.gameObject.layer, true);
        }
        //if (!collisions.Contains(col))
        //{
        //    collisions.Add(col);
        //}

        //if (IsCollidingWithEnemy())
        //{
        //    Action = ActionState.IsHurt;
        //}

        if (GameManager.Instance.EnemyLayers.Contains<int>(col.gameObject.layer))
        {
            if(Action != ActionState.IsHurt)
            {
                AudioManager.instance.PlaySingle(SoundEffect.PlayerHurt);
            }

            Action = ActionState.IsHurt;
        }
    }

    /// <summary>
    /// Removes a collider from the list if it leaves our trigger
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionExit(Collision col)
    {
        //if (collisions.Contains(col))
        //{
        //    collisions.Remove(col);
        //}
    }

    #endregion
}
