using UnityEngine;
using System.Collections;

/// <summary>
/// PlayerHashIDs serves as an interpreter for retreieving and hashing
/// the IDs of different player animation states.
/// </summary>

public class PlayerHashIDs : MonoBehaviour
{
    #region Fields

    // Public Fields
    public int speedFloat;
    public int aliveFloat;
    public int attackFloat;
    public int dodgeBool;

    #endregion

    #region Initialization

    void Awake ()
    {
        // Sets all fields with their respective animation ID
        speedFloat = Animator.StringToHash("Speed");
        aliveFloat = Animator.StringToHash("Alive");
        attackFloat = Animator.StringToHash("Attack");
        dodgeBool = Animator.StringToHash("Dodge");
    }

    #endregion
}
