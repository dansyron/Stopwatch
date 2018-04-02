using UnityEngine;
using System.Collections;

public class EnemyAIParentScript : MonoBehaviour
{
    //protected Constants gameConstants;
    protected RaycastHit hit;
    protected bool playerInAttackRange;

    //used for when the player is in the aggro range
    protected bool moveTowardsPlayer;
    protected bool isAttacking;
    protected bool canAttack;
    protected bool triggerAttack;
    protected bool triggerIsEmpty;
    protected bool allowMovement = false;
    protected GameObject attackRangeChild;

    //vector 3 of the direction/mag from this object towards the player
    protected Vector3 playerDirection;
    protected RaycastHit[] hitsRight;
    protected RaycastHit[] hitsLeft;

    //Manyof the general specs
    public GameObject enemyWeapon;
    public GameObject player;
    public float moveSpeed;
    private float maxMoveSpeed;
    public float aggroRange;
    public bool attackBool;
    protected TimerUI timerUI;
    RaycastHit turnAround;
    Ray ray;

    // Use this for initialization
    protected virtual void Start()
    {
        //getting components
        moveSpeed = 1f;
        maxMoveSpeed = moveSpeed;
        attackBool = false;
        allowMovement = true;
        aggroRange = 10f;
        timerUI = GameManager.Instance.Timer;//GameObject.Find("TimerUI").GetComponent<TimerUI>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //checks if the slowdown is active
        if (timerUI.getSlowDownTime)
        {
            moveSpeed = maxMoveSpeed / Constants.SLOWDOWN_RELATIVE_TIMESCALE;
        }
        //if not do the below
        else
        {
            moveSpeed = maxMoveSpeed;
        }

        playerDirection = new Vector3(gameObject.transform.position.x - player.transform.position.x, 
            gameObject.transform.position.y - player.transform.position.y, 0);
        hitsRight = Physics.RaycastAll(transform.position + (Vector3.up / 5), playerDirection, aggroRange);
        hitsLeft = Physics.RaycastAll(transform.position + (Vector3.up / 5), -playerDirection, aggroRange);

        #region Aggro Range and Moves Towards Player
        //temporay update method for manageing 
        if ((playerDirection.x > -aggroRange) && (playerDirection.x < aggroRange))
        {
        if (playerDirection.magnitude < aggroRange)
        {
            if (moveTowardsPlayer == false)
            {
                //if true moves towards player
                moveTowardsPlayer = true;
            }
        }
        else
        {
            //otherwise continues patrol route
            moveTowardsPlayer = false;
        }
        }
        else
        {
            moveTowardsPlayer = false;
        }

        //If not moving towards player and allowed to move patrols in a line
        if ((!moveTowardsPlayer) && (allowMovement))
        {
            //move the enemy foward
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }

        //transform.Translate((moveSpeed * Time.deltaTime), 0, 0);

        //checks if the trigger is empty meaning the enemy will go off the cliff
        if (triggerIsEmpty)
        {
            //Flip the enemy
                gameObject.transform.Rotate(0, 180, 0);
                //gameObject.transform.rotation = new Quaternion(0, 0, 0, 1);
        }
        if (!triggerIsEmpty)
        {
            //throws out a raycast to see if there is a floor directly infront of the enemy
            //checks when the enemy is moving towards the left
            if (transform.rotation == new Quaternion(0, 1, 0, 0))
            {
                if (Physics.Raycast(gameObject.transform.position + (Vector3.up / 5), Vector3.left, out turnAround, 1f))
                {
                    //sets local variable to the collided objects name
                    string rayHit = turnAround.collider.gameObject.tag;
                    //if it is a Floor
                    if (rayHit == "Floor")
                    {
                        //turns around
                        gameObject.transform.rotation = new Quaternion(0, 0, 0, 1);
                        //doesnt mater what it is set to as long as it is not a current tag
                        rayHit = "reset";
                    }
                }
            }
            //checks when the player is moving towards the right
            if (transform.rotation == new Quaternion(0, 0, 0, 1))
            {
                if (Physics.Raycast(gameObject.transform.position + (Vector3.up / 5), Vector3.right, out turnAround, 1f))
                {
                    //sets local variable to the collided objects name
                    string rayHit = turnAround.collider.gameObject.tag;
                    //if it is a Floor
                    if (rayHit == "Floor")
                    {
                        //turns around
                        gameObject.transform.rotation = new Quaternion(0, 1, 0, 0);
                        //doesnt mater what it is set to as long as it is not a current tag
                        rayHit = "reset";
                        turnAround = new RaycastHit();
                    }
                }
            }
        }
        //sets trigger is empty so it stays empty unless there is something in it
        //triggerIsEmpty = true;

        //checks if the moveTowardsPlayer bool is true and allowsMovement is true
        if (moveTowardsPlayer && allowMovement)
        {
            foreach (RaycastHit hit in hitsLeft)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    //If statement checking if the player is behind the enemy
                    if (playerDirection.x > 0)
                    {
                        //rotates the enemy around if the player is behind them
                        gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);

                        //this gameObject moves towards the player
                        gameObject.transform.Translate((new Vector3(playerDirection.x, 0, 0).normalized * (moveSpeed * Time.deltaTime)));

                    }
                    if (playerDirection.x < 0)
                    {
                        //rotates the enemy around if the player is behind them
                        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);

                        //this gameObject moves towards the player
                        gameObject.transform.Translate(-(new Vector3(playerDirection.x, 0, 0).normalized * (moveSpeed * Time.deltaTime)));

                    }
                }
            }
            foreach (RaycastHit hit in hitsRight)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    //this gameObject moves towards the player
                    //gameObject.transform.Translate(-(new Vector3(playerDirection.x, 0, 0).normalized / (10 - moveSpeed)));

                }
            }
        }
        #endregion

    }

    /// <summary>
    /// get allow movement
    /// </summary>
    public bool GetAllowMovement
    {
        get { return allowMovement; }
    }

    /// <summary>
    /// sets allow movement
    /// </summary>
    public bool SetAllowMovement
    {
        set { allowMovement = value; }
    }

    #region Triggers

    protected virtual void OnCollisionEnter(Collision coll)
    {

    }

    ///// <summary>
    ///// Stuff happens if a trigger exits the collider
    ///// </summary>
    ///// <param name="coll">collider from the object leaving the trigger range</param>
    protected virtual void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Floor")
        {
            //allowMovement = false;
            triggerIsEmpty = true;
        }
    }
    /// <summary>
    /// Stuff happens if a trigger enters the collider
    /// </summary>
    /// <param name="coll">collider from the object the entered the trigger area</param>
    protected virtual void OnTriggerEnter(Collider coll)
    {
    }

    ///// <summary>
    ///// Checks if a trigger is staying in the trigger area
    ///// </summary>
    ///// <param name="coll">collider of any object staying in the trigger area</param>
    protected virtual void OnTriggerStay(Collider coll)
    {
        //sets trigger is empty to false
        triggerIsEmpty = false;
    }

    //public virtual void MoveSlower(float speedFactor)
    //{

    //}

    //public virtual void ReturnToRegularSpeed()
    //{

    //}

    #endregion

}
