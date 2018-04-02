using UnityEngine;
using System.Collections;
/*
public class TempAIParentScript : MonoBehaviour {
    //Constants gameConstants;
    RaycastHit hit;
    bool playerInAttackRange;
    //used for when the player is in the aggro range
    bool moveTowardsPlayer;
    bool isAttacking;
    bool canAttack;
    bool triggerIsEmpty;
    bool allowMovement = false;
    GameObject attackRangeChild;
    //vector 3 of the direction/mag from this object towards the player
    Vector3 playerDirection;
    RaycastHit[] hitsRight;
    RaycastHit[] hitsLeft;

    //This is where the EnemyAI parent will be called

    //TEMPORARY FIELDS UNTIL ENEMYAIPARENT IS COMPLETED
    public GameObject enemyWeapon;
    public GameObject player;
    public float moveSpeed;
    public float aggroRange;
    public bool attackBool;

    // Use this for initialization
    void Start () 
    {
        //getting components
        //temps values WILL BE CHANGED WHEN PARENTAI IS ADDED
        moveSpeed = 1f;
        attackBool = false;
        allowMovement = true;
        aggroRange = 3f;
    }

    // Update is called once per frame
    void FixedUpdate () {
        playerDirection = new Vector3(gameObject.transform.position.x - player.transform.position.x, gameObject.transform.position.y - player.transform.position.y, 0);
        hitsRight = Physics.RaycastAll(transform.position + (Vector3.up / 5), playerDirection, aggroRange);
        hitsLeft = Physics.RaycastAll(transform.position + (Vector3.up / 5), -playerDirection, aggroRange);
        
        #region Aggro Range and Moves Towards Player
        //temporay update method for manageing 
        if (playerDirection.x > -aggroRange && playerDirection.x < aggroRange)
        {
            //if true moves towards player
            moveTowardsPlayer = true;
        }
        else
        {
            //otherwise continues patrol route
            moveTowardsPlayer = false;
        }

        //If not moving towards player and allowed to move patrols in a line
        if(moveTowardsPlayer == false && allowMovement)
        {
            //move the enemy foward
            transform.Translate(moveSpeed * Time.deltaTime, 0 , 0);
        }

        //checks if the trigger is empty meaning the enemy will go off the cliff
        if (triggerIsEmpty == true)
        {
            //Flip the enemy
            transform.Rotate(0, 180, 0);
            print("Yung trigger is empty");
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

                        print("Letf!");
                    }
                    if (playerDirection.x < 0)
                    {
                        //rotates the enemy around if the player is behind them
                        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);

                        //this gameObject moves towards the player
                        gameObject.transform.Translate(-(new Vector3(playerDirection.x, 0, 0).normalized * (moveSpeed * Time.deltaTime)));

                        print("Right!");
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
    public bool getAllowMovement
    {
        get { return allowMovement; }
    }

    /// <summary>
    /// sets allow movement
    /// </summary>
    public bool setAllowMovement
    {
        set { allowMovement = value; }
    }

    #region Triggers

    void OnCollisionEnter(Collision coll)
    {
    //    print(coll.gameObject.tag);
    //    print(coll.gameObject.transform.rotation.z);
    //    print(new Quaternion(0, 0, 90, 0));
        if (coll.gameObject.tag == "Floor" && coll.gameObject.transform.rotation == new Quaternion(0,0,90,0))
        {
            //Flip the enemy
            print("Floor collision");
            transform.Rotate(0, 180, 0);
        }
    }

    ///// <summary>
    ///// Stuff happens if a trigger exits the collider
    ///// </summary>
    ///// <param name="coll">collider from the object leaving the trigger range</param>
    void OnTriggerExit(Collider coll)
    {
    }
    /// <summary>
    /// Stuff happens if a trigger enters the collider
    /// </summary>
    /// <param name="coll">collider from the object the entered the trigger area</param>
    void OnTriggerEnter(Collider coll)
    {

    }

    ///// <summary>
    ///// Checks if a trigger is staying in the trigger area
    ///// </summary>
    ///// <param name="coll">collider of any object staying in the trigger area</param>
    protected void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Floor")
        {
            //sets trigger is empty to false
            print("Trigger stay");
            triggerIsEmpty = false;
        }
    }


    #endregion

}
*/