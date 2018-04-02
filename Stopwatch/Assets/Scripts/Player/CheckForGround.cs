using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CheckForGround : MonoBehaviour {

    //list of colliders that was from the last frame
    List<Collider> collisions = new List<Collider>();

    /// <summary>
    /// Unity update
    /// </summary>
    void Update()
    {
        //removes null elements from the list
        for (int i = collisions.Count - 1; i >= 0; i--)
        {
            if (!collisions[i] || !collisions[i].enabled)
            {
                collisions.RemoveAt(i);
            }
        }
        GameManager.Instance.Player.IsGrounded(collisions.Count > 0);
    }

    /// <summary>
    /// Adds a collider to the list if it enters our trigger
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        if (GameManager.Instance.GroundLayers.Any(y => y == col.gameObject.layer) && !collisions.Contains(col))
        {
            collisions.Add(col);
        }
    }

    /// <summary>
    /// Removes a collider from the list if it leaves our trigger
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerExit(Collider col)
    {
        collisions.Remove(col);
    }
}
