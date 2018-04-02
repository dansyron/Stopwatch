using UnityEngine;
using System.Collections;

public class Knife : MonoBehaviour
{
    #region Fields
    public PlayerController player;
    public TrailRenderer trail;
    #endregion

    #region Initialization

    // Use this for initialization
    void Awake()
    {
        //despawns after .1 second
        //StartCoroutine(despawn());

        // sets knife's position to be the same as its parent and tilts the knife forward by some amount
        //transform.localPosition = Vector3.zero;
        //transform.localEulerAngles = new Vector3(75.341f, 119.683f, 113.322f);
    }

    #endregion

    #region Public Methods

    // Update is called once per frame
    void Update()
    {
        // sets knife's position to be the same as its parent and tilts the knife forward by some amount
        //transform.localPosition = Vector3.zero;
        //transform.localEulerAngles = new Vector3(75.341f, 119.683f, 113.322f);

        if (player.Action == PlayerController.ActionState.IsAttacking)
        {
            trail.enabled = true;
        }
        else
        {
            trail.enabled = false;
        }
    }

    //timer for despawn
    /*IEnumerator despawn()
    {
        yield return new WaitForSeconds(.75f);
        Destroy(gameObject);
    }*/

    #endregion
}
